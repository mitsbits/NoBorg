using Borg.MVC.PlugIns.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace Borg.Cms.Basic.PlugIns.Documents
{
    public sealed class DocumentsPluginDescriptor : IPluginDescriptor, IPlugInArea, ICanMapWhen
    {
        public string Area => "Documents";
        public Func<HttpContext, bool> MapWhenPredicate => c => c.Request.Path.StartsWithSegments($"/{Area}");

        public Action<IApplicationBuilder, Action<IRouteBuilder>> MapWhenAction => (path, routeHandler) =>
        {
            path.UseAuthentication();
            path.UseSession();
            path.UseMvc(routeHandler);
        };
    }
}