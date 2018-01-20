using Borg.Cms.Basic.Lib;
using Borg.MVC;
using Borg.MVC.PlugIns;
using Borg.MVC.PlugIns.Contracts;
using Borg.MVC.PlugIns.Decoration;
using Borg.MVC.Services.Themes;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.Linq;

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
            services.Config(Configuration.GetSection("Borg"), () => Settings);
            services.AddDistributedMemoryCache();
            var assembliesToScan = services.GetRefAssembliesAndRegsiterDefaultProviders(LoggerFactory);
            PlugInHost = new PlugInHost(LoggerFactory, assembliesToScan);
            services.AddSingleton<IPlugInHost>(provider => PlugInHost);
            services.RegisterCommonFramework(Settings, LoggerFactory);
            services.RegisterAuth(Settings, LoggerFactory, Environment);

            services.RegisterBorg(Settings, LoggerFactory, Environment, assembliesToScan);

            services.AddMediatR(assembliesToScan);

            var entrypointassemblies = assembliesToScan.Where(x =>
                x.GetTypes().Any(t => t.GetCustomAttributes<PlugInEntryPointControllerAttribute>() != null)).Distinct().ToArray();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeViewLocationExpander());

                foreach (var entrypointassembly in entrypointassemblies)
                {
                    options.FileProviders.Add(new EmbeddedFileProvider(entrypointassembly));
                }
            });
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