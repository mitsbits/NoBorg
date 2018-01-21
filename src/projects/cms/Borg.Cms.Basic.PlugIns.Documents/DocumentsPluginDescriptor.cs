using Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.Controllers;
using Borg.Infra;
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
using System.Reflection;

namespace Borg.Cms.Basic.PlugIns.Documents
{
    public sealed class DocumentsPluginDescriptor : IPluginDescriptor, IPlugInArea, ICanMapWhen, IPluginServiceRegistration
    {
        public string Area => "Documents";
        public string Title => "Documents Management";

        public IServiceCollection Configure(IServiceCollection services, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment, IConfiguration Configuration, BorgSettings settings, Assembly[] assembliesToScan)
        {
            return services.RegisterDiscoveryServices(this);
        }

        public Tidings BackofficeEntryPointAction => new Tidings
        {
            {"asp-area", Area},
            {"asp-controller", nameof(HomeController).Replace("Controller", string.Empty)},
            {"asp-action", nameof(HomeController.Home)},
            {"asp-route-id", null},
            {"icon-class", "fa fa-book"}
        };

        public Func<HttpContext, bool> MapWhenPredicate => c => c.Request.Path.StartsWithSegments($"/{Area}");

        public Action<IApplicationBuilder, Action<IRouteBuilder>> MapWhenAction => (path, routeHandler) =>
        {
            path.UseAuthentication();
            path.UseSession();
            path.UseMvc(routeHandler);
        };
    }
}