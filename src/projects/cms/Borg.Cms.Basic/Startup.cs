using System.Linq.Expressions;
using Borg.Cms.Basic.Lib;
using Borg.Infra;
using Borg.MVC.Services.Themes;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers;
using Borg.Cms.Basic.Modules.Documents.Area.Documents.Controllers;

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

            var assembliesToScan = services.GetRefAssembliesAndRegsiterDefaultProviders(LoggerFactory);
            services.RegisterCommonFramework(Settings, LoggerFactory);
            services.RegisterAuth(Settings, LoggerFactory, Environment);

            services.RegisterBorg(Settings, LoggerFactory, Environment, assembliesToScan);

            services.AddMediatR(assembliesToScan);

            var fk = new EmbeddedFileProvider(typeof(BackofficeController).GetTypeInfo().Assembly);
            var fs = fk.GetDirectoryContents("");
            var f = fk.GetFileInfo("Areas/Backoffice/Views/Home/Layout/Navigation.cshtml");

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeViewLocationExpander());
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(BackofficeController).GetTypeInfo().Assembly));
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(DocumentsController).GetTypeInfo().Assembly));
            });
            services.AddDistributedMemoryCache();
            services.AddMvc()
                .AddApplicationPart(typeof(BackofficeController).Assembly)
                .AddApplicationPart(typeof(DocumentsController).Assembly);
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

            app.MapWhen(c => c.Request.Path.StartsWithSegments("/backoffice"), path =>
              {
                  path.UseAuthentication();
                  path.UseSession();
                  path.UseMvc(ConfigureRoutes);
              });

            app.MapWhen(c => c.Request.Path.StartsWithSegments("/documents"), path =>
            {
                path.UseAuthentication();
                path.UseSession();
                path.UseMvc(ConfigureRoutes);
            });
            app.MapWhen(c => c.Request.Path.StartsWithSegments("/logout"), path =>
            {
                path.UseAuthentication();
                path.UseSession();
                path.UseMvc(ConfigureRoutes);
            });
            app.MapWhen(c => c.Request.Path.StartsWithSegments("/login"), path =>
            {
                path.UseAuthentication();
                path.UseSession();
                path.UseMvc(ConfigureRoutes);
            });

            app.UseMvc(ConfigureRoutes);
        }

        private static void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "areaRoute",
                template: "{area:exists}/{controller=Home}/{action=Home}/{id?}");

            routeBuilder.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Home}/{id?}");
        }
    }
}