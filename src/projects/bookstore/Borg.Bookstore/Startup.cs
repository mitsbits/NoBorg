using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Borg.Bookstore.Configuration;
using Borg.Bookstore.Features.Users.Policies;
using Borg.Infra;
using Borg.MVC.BuildingBlocks;
using Borg.Platform.Identity;
using Borg.Platform.Identity.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Borg.Bookstore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment environment)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            Environment = environment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Config(Configuration, () => Settings);

            var configProviders = typeof(ApplicationConfig).GetInterfaces();
            foreach (var contract in configProviders)
            {
                services.Add(new ServiceDescriptor(contract, Settings));
            }

            var assebliesToScan = services.FireUpAssemblyScanners(LoggerFactory);

            services.RegisterAuth(LoggerFactory, Environment, Settings, BackofficePolicies.GetPolicies());

            services.AddBorgFramework(Environment, Settings);

            services.AddMediatR(assebliesToScan);

            services.AddMvc();

        }
        protected IConfiguration Configuration { get; }
        protected ILoggerFactory LoggerFactory { get; }

        protected IHostingEnvironment Environment { get; }

        protected ApplicationConfig Settings { get; set; } = new ApplicationConfig();
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(ConfigureRoutes);
        }


        protected void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            //var scope = ServiceProvider.CreateScope();
            //using (scope)
            //{
            //var provider = scope.ServiceProvider;

            //var provs = provider.GetRequiredService<IEnumerable<IRouteConstraint>>();

            //var root = provs.First(x => x.GetType() == typeof(MenuRootRouteConstraint));
            //var parent = provs.First(x => x.GetType() == typeof(MenuLeafParentRouteConstraint));
            //var child = provs.First(x => x.GetType() == typeof(MenuLeafChildRouteConstraint));

            routeBuilder.MapRoute(
                name: "areaRoute",
                template: "{area:exists}/{controller=Home}/{action=Home}/{id?}");

            //routeBuilder.MapRoute(
            //    name: "menuroot",
            //    template: "{rootmenu}",
            //    defaults: new { controller = "Menus", action = "Root", area = "Presentation", component = default(ComponentPageDescriptor<int>) },
            //    constraints: new { rootmenu = root });

            //routeBuilder.MapRoute(
            //    name: "menuleaf",
            //    template: "{parentmenu}/{childmenu}",
            //    defaults: new { controller = "Menus", action = "Leaf", area = "Presentation", component = default(ComponentPageDescriptor<int>) },
            //    constraints: new { parentmenu = parent, childmenu = child });

            //routeBuilder.MapRoute(
            //    name: "siteroot",
            //    template: "",
            //    defaults: new { controller = "Menus", action = "SiteRoot", area = "Presentation", rootmenu = "home", component = default(ComponentPageDescriptor<int>) });

            routeBuilder.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Home}/{id?}");
        }
    }
}

