using Borg.MVC.PlugIns.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace Borg.Cms.Basic.Backoffice
{
    public sealed class BackofficePlugInDescriptor : IPluginDescriptor, IPlugInArea, IPlugInTheme, ICanMapWhen
    {
        public string Area => "Backoffice";
        public string[] Themes => new[] { "Backoffice" };

        public Func<HttpContext, bool> MapWhenPredicate => c => c.Request.Path.StartsWithSegments($"/{Area}");

        public Action<IApplicationBuilder, Action<IRouteBuilder>> MapWhenAction => (path, routeHandler) =>
        {
            path.UseAuthentication();
            path.UseSession();
            path.UseMvc(routeHandler);
        };
    }
}