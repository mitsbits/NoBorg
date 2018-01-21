using Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers;
using Borg.Infra.DTO;
using Borg.MVC.PlugIns.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Borg.Cms.Basic.Backoffice
{
    public sealed class BackofficePlugInDescriptor : IPluginDescriptor, IPlugInArea, IPlugInTheme, ICanMapWhen, IPluginServiceRegistration
    {
        public string Area => "Backoffice";
        public string[] Themes => new[] { "Backoffice" };

        public Tidings BackofficeEntryPointAction => new Tidings
        {
            {"asp-area", Area},
            {"asp-controller", nameof(HomeController).Replace("Controller", string.Empty)},
            {"asp-action", nameof(HomeController.Home)},
            {"asp-route-id", null},
            {"icon-class", ""},
        };

        public Func<HttpContext, bool> MapWhenPredicate => c => c.Request.Path.StartsWithSegments($"/{Area}");

        public Action<IApplicationBuilder, Action<IRouteBuilder>> MapWhenAction => (path, routeHandler) =>
        {
            path.UseAuthentication();
            path.UseSession();
            path.UseMvc(routeHandler);
        };

        public string Title => "Back office";

        public IServiceCollection Configure(IServiceCollection services, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment, IConfiguration Configuration)
        {
            return this.RegisterDiscoveryServices(services);
        }
    }
}