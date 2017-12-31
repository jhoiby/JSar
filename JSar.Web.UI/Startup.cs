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
using JSar.Membership.Services.Query.QueryHandlers.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal;
using MediatR.Pipeline;

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
            // .NET DI - APPLICATION SERVICES

            // Azure support
            services.AddSingleton<IClaimsCache, ClaimsCache>();
            services.AddSingleton<IGraphAuthProvider, GraphAuthProvider>();
            services.AddTransient<IGraphSdkHelper, GraphSdkHelper>();

            //
            // AUTOFAC DI/IOC CONFIGURATION
            
            // Injections are slowly being migrated here from the .Net DI configuration above.

            var builder = new ContainerBuilder();

            // Copies existing dependencies from IServiceCollection
            builder.Populate(services);

            // AUTOFAC - SERILOG

            builder.Register<Serilog.ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                  .ReadFrom.Configuration(Configuration)
                  .CreateLogger();
            }).SingleInstance();

            // AUTOFAC - MEDIATR

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestHandler<>),
                typeof(INotificationHandler<>)
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                // Register all command handler in the same assembly as WriteLogMessageCommandHandler
                builder
                    .RegisterAssemblyTypes(typeof(WriteLogMessageCommandHandler).GetTypeInfo().Assembly)
                    .Where( t=> t.Name.EndsWith("CommandHandler"))
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();

                // Register all QueryHandlers in the same assembly as GetExternalLoginQueryHandler
                builder
                    .RegisterAssemblyTypes(typeof(GetExternalLoginInfoQueryHandler).GetTypeInfo().Assembly)
                    .Where(t => t.Name.EndsWith("QueryHandler"))
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();

                // See: http://docs.autofac.org/en/latest/register/scanning.html 
            }

            // Pipeline pre/post processors
            // These are processed by Autofac in reverse order
            //builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            //builder.RegisterGeneric(typeof(GenericRequestPreProcessor<>)).As(typeof(IRequestPreProcessor<>));
            //builder.RegisterGeneric(typeof(GenericRequestPostProcessor<,>)).As(typeof(IRequestPostProcessor<,>));
            //builder.RegisterGeneric(typeof(GenericPipelineBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });

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
