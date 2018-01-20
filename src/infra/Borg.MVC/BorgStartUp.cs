using Borg.Infra;
using Borg.MVC.PlugIns.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        protected  void BranchPlugins(IApplicationBuilder app)
        {
            var mapwhenmodules = PlugInHost.Specify<ICanMapWhen>();

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


}