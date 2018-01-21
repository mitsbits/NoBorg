using System;
using System.Reflection;
using Borg.MVC;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            AddBorgMvc(services);

            services.AddSession();

            ServiceProvider = services.BuildServiceProvider();
            return ServiceProvider;
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

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

            app.UseMvc(ConfigureRoutes);
        }
    }
}