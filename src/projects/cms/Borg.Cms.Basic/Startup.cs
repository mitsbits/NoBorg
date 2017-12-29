using Borg.Cms.Basic.Lib;
using Borg.MVC;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment environment)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private ILoggerFactory LoggerFactory { get; }

        private IHostingEnvironment Environment { get; }

        private BorgSettings Settings { get; set; } = new BorgSettings();

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Config(Configuration.GetSection("Borg"), () => Settings);
            services.AddDistributedMemoryCache();

            var asmbls = services.GetRefAssembleisAndRegsiterDefaultProviders(LoggerFactory);
            services.RegisterCommonFramework(Settings, LoggerFactory);
            services.RegisterAuth(Settings, LoggerFactory, Environment);
            services.RegisterBorg(Settings, LoggerFactory, Environment);

            services.AddMediatR(asmbls);

            services.AddMvc();

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.Map("/backoffice", path =>
            {
                app.UseMvc(routes =>
                {
                    routes.MapRoute(name: "areaRoute",
                        template: "{area:exists}/{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Home" });
                });
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Home}/{id?}");
            });
        }
    }
}