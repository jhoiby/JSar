using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using MediatR;
using JSar.Membership.Services.CommandHandlers;
using JSar.Membership.Messages.Commands;
using JSar.Membership.Messages;
using JSar.Membership.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using JSar.Membership.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using JSar.Membership.Services.Query.QueryHandlers;
using JSar.Membership.Messages.Queries;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using JSar.Membership.AzureAdAdapter.Extensions;
using JSar.Membership.AzureAdAdapter.Helpers;
using Microsoft.AspNetCore.Mvc;
using JSar.Membership.Messages.Queries.Identity;
using JSar.Membership.Services.CommandPipeline;
using Microsoft.WindowsAzure.Storage.Table.Protocol;
using Autofac.Features.Variance;
using Autofac.Core;
using System.Reflection;

namespace JSar.Web.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Azure AD URIs
        public const string ObjectIdentifierType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        public const string TenantIdType = "http://schemas.microsoft.com/identity/claims/tenantid";

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //
            // DATA SERVICES

            // Domain (command) database context.
            services.AddDbContext<MembershipDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("JSar.MembershipDb")));

            // .Net Core Identity configuration, with extended User and Role classes.
            services.AddIdentity<AppUser, AppRole>()
               .AddEntityFrameworkStores<MembershipDbContext>()
               .AddDefaultTokenProviders();

            // 
            // AUTHENTICATION

            // Add Azure AD authentication option for Office 365 integration
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddAzureAd(options => 
                Configuration.Bind("AzureAd", options))
            .AddCookie(options =>                               
            {
                options.LoginPath = "/Account/SignIn";
                options.LogoutPath = "/Account/SignOut";
            });

            // OLD: For cookie sign-in, pre-AAD
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/Account/SignIn";
            //});
            //
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>                               // TRY ADDING THIS LATER AFTER AddAzureAd if needed.
            //    {
            //        options.LoginPath = "/Account/SignIn";
            //        options.LogoutPath = "/Account/SignOut";
            //    });

            // From test app, working with AAD. To be used later.
            // Add cookie authentication
            //services.AddAuthentication(sharedOptions =>
            //{
            //    sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    // sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;  // Azure AD
            //});

            //
            // MVC OPTIONS

            services.AddMvc();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            //
            // APPLICATION SERVICES

            // Mediator pipeline - Discover and register command/query/event handlers in the
            // ObApp.Services and other projects/assemblies. Need to specify a concrete type, then
            // MediatR will scan the entire assembly it's contained in for additional handlers.
            //services.AddMediatR(
            //    typeof(CommandHandler<WriteLogMessage, CommonResult>).Assembly,
            //    typeof(QueryHandler<GetUserByEmail, CommonResult>).Assembly);

            // Azure support
            services.AddSingleton<IClaimsCache, ClaimsCache>();
            services.AddSingleton<IGraphAuthProvider, GraphAuthProvider>();
            services.AddTransient<IGraphSdkHelper, GraphSdkHelper>();

            //
            // AUTOFAC DI/IOC CONFIGURATION

            // Installed for ability to add decorators required for MediatR pipeline.
            // Injections are slowly being migrated here from the .Net DI configuration above.

            var builder = new ContainerBuilder();

            // Copies existing dependencies from IServiceCollection
            builder.Populate(services);

            // Serilog
            builder.Register<Serilog.ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                  .ReadFrom.Configuration(Configuration)
                  .CreateLogger();
            }).SingleInstance();

            // Add additional registrations here. Examples:
            // 
            // builder.RegisterType<PostRepository>().As<IPostRepository>();
            // builder.RegisterType<SiteAnalyticsServices>();








            var type1 = typeof(CommandHandler<,>);
            var type2 = typeof(ICommandHandler<,>);
            var type3 = typeof(CommandHandlerPipeline<,>);


            // Example to delete asap
            //services.AddMediatR(
            //    typeof(CommandHandler<WriteLogMessage, CommonResult>).Assembly,
            //    typeof(QueryHandler<GetUserByEmail, CommonResult>).Assembly);

            // MEDIATR CONFIG FOR AUTOFAC

            // enables contravariant Resolve() for interfaces with single contravariant ("in") arg
            builder
                .RegisterSource(new ContravariantRegistrationSource());

            // mediator itself
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            // request handlers
            builder
                .Register<SingleInstanceFactory>(ctx => {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => { object o; return c.TryResolve(t, out o) ? o : null; };
                })
                .InstancePerLifetimeScope();

            // notification handlers
            builder
                .Register<MultiInstanceFactory>(ctx => {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
                })
                .InstancePerLifetimeScope();

            // finally register our custom code (individually, or via assembly scanning)
            // - requests & handlers as transient, i.e. InstancePerDependency()
            // - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            // - behaviors as transient, i.e. InstancePerDependency()
            builder.RegisterAssemblyTypes(typeof(CommandHandler<,>).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan
            builder.RegisterAssemblyTypes(typeof(QueryHandler<,>).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan
            //builder.RegisterType<MyHandler>().AsImplementedInterfaces().InstancePerDependency();  

            // Register custom command handlers by scanning assemblies
            // See: https://stackoverflow.com/questions/8140714/autofac-decorating-open-generics-registered-using-assembly-scanning
            //builder.RegisterAssemblyTypes(typeof(CommandHandler<,>).Assembly)
            //    .As(t => t.GetInterfaces()
            //        .Where(i => i.IsClosedTypeOf(typeof(ICommandHandler<,>)))
            //        .Select(i => new KeyedService("commandImplementor", i)));

            Assembly assembly = typeof(CommandHandler<,>).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .As(t => t.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(ICommandHandler<,>)))
                    .Select(i => new KeyedService("commandHandler", i)));

            var interfaces = typeof(CommandHandler<,>).GetInterfaces();
            var interfaces2 = typeof(CommandHandler<,>).GetInterfaces()
                .Where(i => i.IsClosedTypeOf(typeof(ICommandHandler<,>))); // FAILS
            var ks = typeof(CommandHandler<,>).GetInterfaces()
                .Where(i => i.IsClosedTypeOf(typeof(ICommandHandler<,>)))
                .Select(i => new KeyedService("commandHandler", i));


            //var assm2 = typeof(WriteLogMessageCommandHandler).IsClosedTypeOf(typeof(ICommandHandler<,>));

            //builder.RegisterGeneric(typeof(CommandHandler<,>))
            //    .Named("commandImplementor", typeof(ICommandHandler<,>));

            // Add command handler pipeline decorator to command handlers
            builder.RegisterGenericDecorator(
                typeof(CommandHandlerPipeline<,>),
                typeof(ICommandHandler<,>),
                fromKey: "commandHandler");





            // Register the command handlers
            //builder.RegisterGeneric(typeof(CommandHandler<,>))
            //    .Named("commandImplementor", typeof(ICommandHandler<,>));

            // Add command handler pipeline decorator to command handlers
            //builder.RegisterGenericDecorator(
            //    typeof(CommandHandlerPipeline<,>),
            //    typeof(ICommandHandler<,>),
            //    fromKey: "commandImplementor");

            // Finalize
            var container = builder.Build();

            //Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
