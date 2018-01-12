using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using MediatR;
using JSar.Membership.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using JSar.Membership.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using JSar.Membership.AzureAdAdapter.Extensions;
using JSar.Membership.AzureAdAdapter.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using FluentValidation;
using JSar.Membership.Domain.Abstractions;
using JSar.Membership.Infrastructure.Logging;
using JSar.Membership.Services;
using JSar.Membership.Services.Account;
using MediatR.Pipeline;
using JSar.Membership.Services.Validation;
using JSar.Web.UI.Logging;

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

            services.AddMvc()
                .AddFeatureFolders();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(new RequireHttpsAttribute());
                options.Filters.Add<LogActionFilter>();
            });

            //
            // APPLICATION SERVICES

            // Azure support
            services.AddSingleton<IClaimsCache, ClaimsCache>();
            services.AddSingleton<IGraphAuthProvider, GraphAuthProvider>();
            services.AddTransient<IGraphSdkHelper, GraphSdkHelper>();

            //
            // AUTOFAC DI/IOC CONFIGURATION
            

            var builder = new ContainerBuilder();

            // Copy existing config from .Net Core IServiceCollection to Autofac
            builder.Populate(services);

            // AUTOFAC DATA PERSISTENCE CONFIG

            builder.RegisterType(typeof(UnitOfWork))
                .As(typeof(IUnitOfWork))
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            // AUTOFAC SERILOG CONFIG

            builder.Register<Serilog.ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                  .ReadFrom.Configuration(Configuration)
                  .CreateLogger();
            }).SingleInstance();

            // AUTOFAC MEDIATR CONFIG

            // Register MediatR as IMediator for injection
            //builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
            builder.RegisterType<Mediator>()
                .Named<IMediator>("mediator")
                .SingleInstance();

            // Mediator decorator for event/request logging
            builder.RegisterDecorator<IMediator>(
                (c, inner) => new MediatrLoggingDecorator(inner, c.Resolve<ILogger>()),
                fromKey: "mediator")
                .SingleInstance();

            // Register the main handlers
            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestHandler<>),
                typeof(INotificationHandler<>)
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                // Register all command handlers in the same assembly as WriteLogMessageCommandHandler
                builder
                    .RegisterAssemblyTypes(typeof(WriteLogMessageCommandHandler).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();

                // Register all QueryHandlers in the same assembly as GetExternalLoginQueryHandler
                builder
                    .RegisterAssemblyTypes(typeof(GetUserByEmailQueryHandler).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            // Register Message(Command/ Query) Validators
            builder.RegisterAssemblyTypes(typeof(ExternalLoginSignInValidator).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces();

            // Pipeline pre/post processors
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(PipelineLoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            // More samples from MediatR
            // builder.RegisterGeneric(typeof(GenericRequestPreProcessor<>)).As(typeof(IRequestPreProcessor<>));
            // builder.RegisterGeneric(typeof(MyCommandPreProcessor<>)).As(typeof(IRequestPreProcessor<>));
            // // builder.RegisterGeneric(typeof(GenericRequestPostProcessor<,>)).As(typeof(IRequestPostProcessor<,>));
            // // builder.RegisterGeneric(typeof(GenericPipelineBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            
            // MediatR factories
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
