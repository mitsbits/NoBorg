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
    public class Startup : BorgStartUp
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment environment) : base(configuration, loggerFactory, environment)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            PopulateSettings(services);

            var assembliesToScan = PopulateAssemblyProviders(services);

            RegisterPlugins(services);

            services.RegisterCommonFramework(Settings, LoggerFactory);
            services.RegisterAuth(Settings, LoggerFactory, Environment);
            services.AddDistributedMemoryCache();
            services.RegisterBorg(Settings, LoggerFactory, Environment, assembliesToScan);

            services.AddMediatR(assembliesToScan);

            var entrypointassemblies = ViewEngineProvidersForPluginThemes(services, assembliesToScan);
            services.AddDistributedMemoryCache();
            var mvcbuilder = services.AddMvc();

            foreach (var entrypointassembly in entrypointassemblies)
            {
                mvcbuilder.AddApplicationPart(entrypointassembly);
            }
   
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

            BranchPlugins(app);

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
    }
}