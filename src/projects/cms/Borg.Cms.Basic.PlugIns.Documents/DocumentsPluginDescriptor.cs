using Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.Controllers;
using Borg.Cms.Basic.PlugIns.Documents.BackgroundJobs;
using Borg.Cms.Basic.PlugIns.Documents.Data;

using Borg.Infra;
using Borg.Infra.DTO;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.MVC.PlugIns.Contracts;
using Borg.MVC.PlugIns.Decoration;
using Borg.Platform.Azure.Storage.Blobs;
using Borg.Platform.EF.Contracts;
using Borg.Platform.EF.DAL;
using Borg.Platform.ImageSharp;
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
using Borg.Infra.Storage;
using Borg.Infra.Storage.Documents;
using Borg.Platform.Documents.Services;
using Microsoft.Extensions.FileProviders;
using DocumentsService = Borg.Cms.Basic.PlugIns.Documents.Services.DocumentsService;

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

            services.AddScoped<IStaticImageCacheStore<int>>(provider =>
            {
                return new StaticImageCacheStore(loggerFactory,
                    provider.GetService<IAssetStore<AssetInfoDefinition<int>, int>>(),
                    () => new FolderFileStorage(settings.Storage.ImagesCacheFolder, loggerFactory),
                    //() => new AzureFileStorage(settings.Storage.AzureStorageConnection, settings.Storage.ImagesCacheFolder),
                    provider.GetRequiredService<IAssetDirectoryStrategy<int>>(), settings, settings,
                    provider.GetRequiredService<IImageResizer>());
            });


            //services.AddScoped<AzureBlobStorageFileProvider>(provider => new AzureBlobStorageFileProvider(
            //    new AzureFileStorage(settings.Storage.AzureStorageConnection, settings.Storage.ImagesCacheFolder), "img"));
            //services.AddScoped<AzureBlobStorageFileProvider>(provider => new PhysicalFileProvider( settings.Storage.ImagesCacheFolder));

            services.AddScoped<IImageResizer, ImageResizer>();
            services.AddScoped<IDocumentsService<int>, DocumentsService>();
            services.AddScoped<CacheStaticImagesForWeb>();

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