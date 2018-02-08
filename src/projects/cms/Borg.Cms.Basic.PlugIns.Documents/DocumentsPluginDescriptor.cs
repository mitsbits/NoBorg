using Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.Controllers;
using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Infra;
using Borg.Infra.DTO;
using Borg.MVC.PlugIns.Contracts;
using Borg.Platform.EF.Contracts;
using Borg.Platform.EF.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using Borg.MVC.PlugIns.Decoration;

namespace Borg.Cms.Basic.PlugIns.Documents
{
    public sealed class DocumentsPluginDescriptor : IPluginDescriptor, IPlugInArea, ICanMapWhen, IPluginServiceRegistration, ITagHelpersPlugIn
    {
        public string Area => "Documents";
        public string Title => "Documents";

        public IServiceCollection Configure(IServiceCollection services, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment, IConfiguration Configuration, BorgSettings settings, Assembly[] assembliesToScan)
        {
            services.RegisterDiscoveryServices(this);
            services.AddDbContext<DocumentsDbContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings["db"], x => x.MigrationsHistoryTable("__MigrationsHistory", "documents")).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                options.EnableSensitiveDataLogging(hostingEnvironment.IsDevelopment() || hostingEnvironment.EnvironmentName.EndsWith("local"));
            });
            services.AddScoped<IUnitOfWork<DocumentsDbContext>, UnitOfWork<DocumentsDbContext>>();
            services.AddScoped<DocumentsDbSeed>();
            return services;
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