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

namespace JSar.Web.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

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
