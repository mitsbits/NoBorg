using Borg.Cms.Basic.Lib.Features;
using Borg.Cms.Basic.Lib.System;
using Borg.Cms.Basic.Presentation.Areas.Presentation.Controllers;
using Borg.Cms.Basic.Presentation.RouteConstraints;
using Borg.MVC;
using Borg.MVC.PlugIns.Contracts;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Borg.Platform.Azure.Storage.Blobs;
using Microsoft.Extensions.FileProviders;

namespace Borg.Cms.Basic
{
    public class Startup : BorgStartUp
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment environment) : base(configuration, loggerFactory, environment)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            PopulateSettings(services);
            RegisterPlugins(services);

            services.AddDistributedSqlServerCache(options =>
            {
                options.SchemaName = "cache";
                options.TableName = "Store";
                options.ConnectionString = Settings.ConnectionStrings["db"];
            });
            services.AddMediatR(AssembliesToScan);
            services.AddDistributedMemoryCache();

            var builder = AddBorgMvc(services);
            builder.AddApplicationPart(typeof(PresentationController).Assembly);
            //builder.ConfigureApplicationPartManager(p =>
            //    p.FeatureProviders.Add(new EntityControllerFeatureProvider(PlugInHost)));

            services.AddAuthorization(options =>
            {
                foreach (var sec in PlugInHost.SecurityPlugIns())
                {
                    options.AddAuthorizationPolicies(sec);
                }
            });

            services.AddHangfire(x => x.UseSqlServerStorage(Settings.ConnectionStrings["db"], new SqlServerStorageOptions() { SchemaName = "hangfire" }));

            services.AddScoped<IRouteConstraint, MenuRootRouteConstraint>();
            services.AddScoped<IRouteConstraint, MenuLeafParentRouteConstraint>();
            services.AddScoped<IRouteConstraint, MenuLeafChildRouteConstraint>();
            //services.AddScoped< MenuRootRouteConstraint>();
            //services.AddScoped<MenuLeafParentRouteConstraint>();
            //services.AddScoped<MenuLeafChildRouteConstraint>();
            //services.Configure<RouteOptions>(options =>
            //{
            //    options.ConstraintMap.Add(MenuRootRouteConstraint.ROUTE_IDENTIFIER, typeof(MenuRootRouteConstraint));
            //    options.ConstraintMap.Add(MenuLeafParentRouteConstraint.ROUTE_IDENTIFIER, typeof(MenuLeafParentRouteConstraint));
            //    options.ConstraintMap.Add(MenuLeafChildRouteConstraint.ROUTE_IDENTIFIER, typeof(MenuLeafChildRouteConstraint));
            //});

            services.AddSession();
            services.AddSingleton<IHostedService, Sentinel>();
            services.AddSingleton<ISentinel, Sentinel>();
            ServiceProvider = services.BuildServiceProvider();
            return ServiceProvider;
        }

        protected void BranchPlugins(IApplicationBuilder app)
        {
            var mapwhenmodules = PlugInHost.FilterPluginsTo<ICanMapWhen>();

            foreach (var mapwhenmodule in mapwhenmodules)
            {
                app.MapWhen(mapwhenmodule.MapWhenPredicate, path => mapwhenmodule.MapWhenAction(path, ConfigureRoutes));
            }
        }

        protected void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            var scope = ServiceProvider.CreateScope();
            using (scope)
            {
                var provider = scope.ServiceProvider;

                var provs = provider.GetRequiredService<IEnumerable<IRouteConstraint>>();

                var root = provs.First(x => x.GetType() == typeof(MenuRootRouteConstraint));
                var parent = provs.First(x => x.GetType() == typeof(MenuLeafParentRouteConstraint));
                var child = provs.First(x => x.GetType() == typeof(MenuLeafChildRouteConstraint));

                routeBuilder.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Home}/{id?}");

                routeBuilder.MapRoute(
                    name: "menuroot",
                    template: "{rootmenu}",
                    defaults: new { controller = "Menus", action = "Root", area = "Presentation" },
                    constraints: new { rootmenu = root });

                routeBuilder.MapRoute(
                    name: "menuleaf",
                    template: "{parentmenu}/{childmenu}",
                    defaults: new { controller = "Menus", action = "Leaf", area = "Presentation" },
                    constraints: new { parentmenu = parent, childmenu = child });

                routeBuilder.MapRoute(
                    name: "siteroot",
                    template: "",
                    defaults: new { controller = "Menus", action = "SiteRoot", area = "Presentation" });

                routeBuilder.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Home}/{id?}");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePagesWithReExecute("/error/page{0}");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new CompositeFileProvider(
                    new PhysicalFileProvider(env.WebRootPath),
                    serviceProvider.GetRequiredService<AzureBlobStorageFileProvider>()
                )
            });

            BranchPlugins(app);

            app.MapWhen(c => c.Request.Path.StartsWithSegments(Settings.Auth.LoginPath), path =>
            {
                path.UseAuthentication();
                path.UseSession();
                path.UseMvc(ConfigureRoutes);
            });
            app.MapWhen(c => c.Request.Path.StartsWithSegments(Settings.Auth.LogoutPath), path =>
            {
                path.UseAuthentication();
                path.UseSession();
                path.UseMvc(ConfigureRoutes);
            });
            app.MapWhen(c => c.Request.Path.StartsWithSegments(Settings.Auth.AccessDeniedPath), path =>
            {
                path.UseAuthentication();
                path.UseSession();
                path.UseMvc(ConfigureRoutes);
            });
            GlobalConfiguration.Configuration
                .UseActivator(new ContainerJobActivator(serviceProvider));
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            app.UseMvc(ConfigureRoutes);
        }
    }
}