using Borg.Cms.Basic.Lib.Discovery;
using Borg.Cms.Basic.Lib.Discovery.Contracts;
using Borg.Cms.Basic.Lib.Features.Auth;
using Borg.Cms.Basic.PlugIns.BlogEngine.Domain;
using Borg.Infra;
using Borg.Infra.DTO;
using Borg.MVC.PlugIns.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Borg.Cms.Basic.PlugIns.BlogEngine.Areas.BlogEngine.Controllers;

namespace Borg.Cms.Basic.PlugIns.BlogEngine
{
    public sealed class BlogEnginePluginDescriptor : IPluginDescriptor, IPlugInArea, ICanMapWhen,
        IPluginServiceRegistration, ISecurityPlugIn, IPlugInEfEntityRegistration
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

        public string[] DefinedRoles => new[] { CmsRoles.Blogger.ToString() };
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

        public IDictionary<Type, Func<ModelBuilder, bool>> Entities
        {
            get
            {
                bool Empty(ModelBuilder builder) { return false; }

                var dict = GetType().Assembly.GetTypes().Where(x => x.GetCustomAttribute<EntityAttribute>() != null)
                    .ToDictionary(x => x, x => (Func<ModelBuilder, bool>)Empty);

                dict[typeof(BloggerBlog)] = builder =>
                {
                    builder.Entity<BloggerBlog>().HasKey(x => new { x.BlogId, x.BloggerId });
                    return true;
                };
                return dict;
            }
        }
    }
}