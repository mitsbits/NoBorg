using Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers;
using Borg.Cms.Basic.Lib;
using Borg.Cms.Basic.Lib.Features.Auth;
using Borg.Infra;
using Borg.Infra.DTO;
using Borg.MVC.PlugIns.Contracts;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Borg.Infra.Services;

namespace Borg.Cms.Basic.Backoffice
{
    public sealed class BackofficePlugInDescriptor : IPluginDescriptor, IPlugInArea, IPlugInTheme, ICanMapWhen, IPluginServiceRegistration, ISecurityPlugIn, ITagHelpersPlugIn
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

        public IServiceCollection Configure(IServiceCollection services, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment, IConfiguration Configuration, BorgSettings settings, Assembly[] assembliesToScan)
        {
            services.RegisterAuth(settings, loggerFactory, hostingEnvironment);
            services.RegisterCommonFramework(settings, loggerFactory);
            services.RegisterBorg(settings, loggerFactory, hostingEnvironment, assembliesToScan);
            return services.RegisterDiscoveryServices(this);
        }

        public string[] DefinedRoles
        {
            get
            {
                var result = new List<string>();

                result.AddRange(EnumUtil.GetValues<SystemRoles>().Select(x => x.ToString()));
                result.AddRange(EnumUtil.GetValues<CmsRoles>().Select(x => x.ToString()));
                return result.ToArray();
            }
        }

        public IDictionary<string, AuthorizationPolicy> Policies => GetPolicies();

        private IDictionary<string, AuthorizationPolicy> GetPolicies()
        {
            var result = new Dictionary<string, AuthorizationPolicy>
            {
                {
                    "ContentEditor",
                    new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireAssertion(c => ContentEditorHandler(c)).Build()
                }
            };
            return result;
        }

        private bool ContentEditorHandler(AuthorizationHandlerContext authorizationHandlerContext)
        {
            var isAdmin = authorizationHandlerContext.User.IsInAnyRole(SystemRoles.Admin.ToString(), SystemRoles.Developer.ToString());
            if (isAdmin) return true;
            var isEditor = authorizationHandlerContext.User.IsInAnyRole(CmsRoles.Author.ToString(), CmsRoles.Editor.ToString()) &&
                           authorizationHandlerContext.User.IsInRole(SystemRoles.Writer.ToString());
            return isEditor;
        }

        public string[] TagHelpers
        {
            get
            {
                var attrs = GetType().Assembly.GetTypes().Select(x => x.GetCustomAttribute<PulgInTagHelperAttribute>());
                if (!attrs.Any(x => x != null)) return new string[0];
                return attrs.Where(x => x != null).Distinct().Select(x => x.Name).ToArray();
            }
        }
    }
}