using Borg.Infra;
using Borg.Infra.Services.AssemblyProvider;
using Borg.MVC.PlugIns;
using Borg.MVC.PlugIns.Contracts;
using Borg.MVC.PlugIns.Decoration;
using Borg.MVC.Services.Themes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Reflection;

namespace Borg.MVC
{
    public abstract class BorgStartUp
    {
        protected BorgStartUp(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment environment)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            Environment = environment;
        }

        protected IConfiguration Configuration { get; }
        protected ILoggerFactory LoggerFactory { get; }

        protected IHostingEnvironment Environment { get; }

        protected BorgSettings Settings { get; set; } = new BorgSettings();

        protected IPlugInHost PlugInHost { get; set; }

        protected IServiceCollection RegisterPlugins(IServiceCollection services)
        {
            services.TryAddSingleton(typeof(IPlugInHost), provider => PlugInHost);
            var registernmodules = PlugInHost.SpecifyPlugins<IPluginServiceRegistration>();

            foreach (var registernmodule in registernmodules)
            {
                registernmodule.Configure(services, LoggerFactory, Environment, Configuration);
            }
            return services;
        }

        protected void PopulateSettings(IServiceCollection services)
        {
            services.Config(Configuration.GetSection("Borg"), () => Settings);
        }

        protected Assembly[] PopulateAssemblyProviders(IServiceCollection services)
        {
            var assembliesToScan = services.GetRefAssembliesAndRegsiterDefaultProviders(LoggerFactory);
            PlugInHost = new PlugInHost(LoggerFactory, assembliesToScan);
            return assembliesToScan;
        }

        protected static Assembly[] ViewEngineProvidersForPluginThemes(IServiceCollection services, Assembly[] assembliesToScan)
        {
            var entrypointassemblies = assembliesToScan.Where(x =>
                    x.GetTypes().Any(t => t.GetCustomAttributes<PlugInEntryPointControllerAttribute>() != null)).Distinct()
                .ToArray();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeViewLocationExpander());

                foreach (var entrypointassembly in entrypointassemblies)
                {
                    options.FileProviders.Add(new EmbeddedFileProvider(entrypointassembly));
                }
            });
            return entrypointassemblies;
        }

        protected void BranchPlugins(IApplicationBuilder app)
        {
            var mapwhenmodules = PlugInHost.SpecifyPlugins<ICanMapWhen>();

            foreach (var mapwhenmodule in mapwhenmodules)
            {
                app.MapWhen(mapwhenmodule.MapWhenPredicate, path => mapwhenmodule.MapWhenAction(path, ConfigureRoutes));
            }
        }

        protected static void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "areaRoute",
                template: "{area:exists}/{controller=Home}/{action=Home}/{id?}");

            routeBuilder.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Home}/{id?}");
        }
    }

    internal static class Ext
    {
        internal static Assembly[] GetRefAssembliesAndRegsiterDefaultProviders(this IServiceCollection services, ILoggerFactory loggerFactory)
        {
            services.AddScoped<IAssemblyProvider, DepedencyAssemblyProvider>();
            services.AddScoped<IAssemblyProvider, ReferenceAssemblyProvider>();
            var p1 = new DepedencyAssemblyProvider(loggerFactory);
            var p2 = new ReferenceAssemblyProvider(loggerFactory);
            var asmbls = p1.GetAssemblies().Union(p2.GetAssemblies()).Where(x => !x.FullName.StartsWith("Microsoft")).Distinct();
            return asmbls.ToArray();
        }
    }
}