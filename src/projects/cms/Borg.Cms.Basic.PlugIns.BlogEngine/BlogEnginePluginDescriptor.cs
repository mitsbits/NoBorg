﻿using Borg.Cms.Basic.PlugIns.BlogEngine.Areas.Blogs.Controllers;
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
using System.Collections.Generic;
using System.Reflection;
using Borg.Cms.Basic.Lib.Features.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Borg.Cms.Basic.PlugIns.BlogEngine
{
    public sealed class BlogEnginePluginDescriptor : IPluginDescriptor, IPlugInArea, ICanMapWhen, IPluginServiceRegistration, ISecurityPlugIn
    {
        public string Area => "BlogEngine";
        public string Title => "Blog Engine";

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
            {"icon-class", "fa fa-newspaper-o"}
        };

        public Func<HttpContext, bool> MapWhenPredicate => c => c.Request.Path.StartsWithSegments($"/{Area}");

        public Action<IApplicationBuilder, Action<IRouteBuilder>> MapWhenAction => (path, routeHandler) =>
        {
            path.UseAuthentication();
            path.UseSession();
            path.UseMvc(routeHandler);
        };

        public string[] DefinedRoles => new[] {CmsRoles.Blogger.ToString()};
        public IDictionary<string, AuthorizationPolicy> Policies => GetPolicies();

        private IDictionary<string, AuthorizationPolicy> GetPolicies()
        {
            var result = new Dictionary<string, AuthorizationPolicy>
            {
                {
                    "Blogger",
                    new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireAssertion(c => BloggerHandler(c)).Build()
                }
            };
            return result;
        }

        private static bool BloggerHandler(AuthorizationHandlerContext authorizationHandlerContext)
        {
            var isAdmin = authorizationHandlerContext.User.IsInAnyRole(SystemRoles.Admin.ToString(), SystemRoles.Developer.ToString());
            if (isAdmin) return true;
            var isBloggerr = authorizationHandlerContext.User.IsInRole(CmsRoles.Blogger.ToString());
            return isBloggerr;
        }
    }
}