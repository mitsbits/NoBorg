using Borg.Bookstore.Configuration;
using Borg.Bookstore.Data;
using Borg.Bookstore.Features.Users.Policies;
using Borg.Infra.Caching;
using Borg.Infra.Caching.Contracts;
using Borg.Infra.Storage;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Documents;
using Borg.Platform.Documents.Data;
using Borg.Platform.Documents.Services;
using Borg.Platform.EF.Contracts;
using Borg.Platform.EF.DAL;
using Borg.Platform.EStore.Data;
using Borg.Platform.ImageSharp;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Borg.Bookstore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment environment)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Config(Configuration, () => Settings);

            var configProviders = typeof(ApplicationConfig).GetInterfaces().Union(typeof(ApplicationConfig).GetBaseTypes().SelectMany(x => x.GetInterfaces())).Distinct();
            foreach (var contract in configProviders)
            {
                services.Add(new ServiceDescriptor(contract, Settings));
            }

            var assebliesToScan = services.FireUpAssemblyScanners(LoggerFactory, x => x.FullName.StartsWith("Borg"));

            services.RegisterAuth(LoggerFactory, Environment, Settings, BackofficePolicies.GetPolicies());

            services.AddBorgFramework(Environment, Settings);

            services.AddDistributedSqlServerCache(options =>
            {
                options.SchemaName = "cache";
                options.TableName = "Store";
                options.ConnectionString = Settings.ConnectionStrings["db"];
            });

            services.AddDbContext<EStoreDbContext>(options =>
            {
                options.UseSqlServer(Settings.ConnectionStrings["db"], x => x.MigrationsHistoryTable("__MigrationsHistory", "estore"));
                options.EnableSensitiveDataLogging(Environment.IsDevelopment());
            });

            services.AddScoped<IDbSeed, EStoreDbSeed>();
            services.AddScoped<EStoreDbSeed>();

            services.AddSingleton<ICacheStore, CacheStore>();

            services.AddDbContext<BookstoreDbContext>(options =>
            {
                options.UseSqlServer(Settings.ConnectionStrings["db"], x => x.MigrationsHistoryTable("__MigrationsHistory", "bookstore"));
                options.EnableSensitiveDataLogging(Environment.IsDevelopment());
            });

            services.AddScoped<IDbSeed, BookstoreDbSeed>();
            services.AddScoped<BookstoreDbSeed>();

            services.AddDbContext<DocumentsDbContext>(options =>
            {
                options.UseSqlServer(Settings.ConnectionStrings["db"], x => x.MigrationsHistoryTable("__MigrationsHistory", "documents"));
                options.EnableSensitiveDataLogging(Environment.IsDevelopment());
            });

            services.AddScoped<IDbSeed, DocumentsDbSeed>();
            services.AddScoped<IStaticImageCacheStore<int>>(provider =>
            {
                return new StaticImageCacheStore(LoggerFactory,
                    provider.GetService<IAssetStore<AssetInfoDefinition<int>, int>>(),
                    () => new FolderFileStorage(Path.Combine(Environment.WebRootPath, Settings.Storage.ImagesCacheFolder), LoggerFactory),
                    provider.GetRequiredService<IAssetDirectoryStrategy<int>>(), Settings, Settings,
                    provider.GetRequiredService<IImageResizer>());
            });
            services.Add(new ServiceDescriptor(typeof(IAssetStore<AssetInfoDefinition<int>, int>),
                p => new AssetService(LoggerFactory,
                    p.GetRequiredService<IAssetDirectoryStrategy<int>>(),
                    p.GetRequiredService<IConflictingNamesResolver>(),
                    () => new FolderFileStorage(Path.Combine(Environment.WebRootPath, Settings.Storage.Folder), LoggerFactory),
                    p.GetRequiredService<IAssetStoreDatabaseService<int>>()),
                ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IAssetDirectoryStrategy<int>),
                p => new RoundUpAssetDirectoryStrategy(10, 50), ServiceLifetime.Scoped));
            services.AddScoped<IConflictingNamesResolver, DefaultConflictingNamesResolver>();
            services.AddScoped<IAssetStoreDatabaseService<int>, EfAssetsSequencedDatabaseService>();

            services.AddScoped<IImageResizer, ImageResizer>();
            services.AddScoped<IDocumentsService<int>, DocumentsService>();
            //services.AddScoped<CacheStaticImagesForWeb>();

            services.AddScoped<IUnitOfWork<DocumentsDbContext>, UnitOfWork<DocumentsDbContext>>();

            services.AddScoped<DocumentsDbSeed>();

            services.AddMediatR(assebliesToScan);

            services.AddHangfire(x => x.UseSqlServerStorage(Settings.ConnectionStrings["db"], new SqlServerStorageOptions() { SchemaName = "hangfire" }));

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();
        }

        protected IConfiguration Configuration { get; }
        protected ILoggerFactory LoggerFactory { get; }

        protected IHostingEnvironment Environment { get; }

        protected ApplicationConfig Settings { get; set; } = new ApplicationConfig();

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            app.UseMvc(ConfigureRoutes);
        }

        protected void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            //var scope = ServiceProvider.CreateScope();
            //using (scope)
            //{
            //var provider = scope.ServiceProvider;

            //var provs = provider.GetRequiredService<IEnumerable<IRouteConstraint>>();

            //var root = provs.First(x => x.GetType() == typeof(MenuRootRouteConstraint));
            //var parent = provs.First(x => x.GetType() == typeof(MenuLeafParentRouteConstraint));
            //var child = provs.First(x => x.GetType() == typeof(MenuLeafChildRouteConstraint));

            routeBuilder.MapRoute(
                name: "areaRoute",
                template: "{area:exists}/{controller=Home}/{action=Home}/{id?}");

            //routeBuilder.MapRoute(
            //    name: "menuroot",
            //    template: "{rootmenu}",
            //    defaults: new { controller = "Menus", action = "Root", area = "Presentation", component = default(ComponentPageDescriptor<int>) },
            //    constraints: new { rootmenu = root });

            //routeBuilder.MapRoute(
            //    name: "menuleaf",
            //    template: "{parentmenu}/{childmenu}",
            //    defaults: new { controller = "Menus", action = "Leaf", area = "Presentation", component = default(ComponentPageDescriptor<int>) },
            //    constraints: new { parentmenu = parent, childmenu = child });

            //routeBuilder.MapRoute(
            //    name: "siteroot",
            //    template: "",
            //    defaults: new { controller = "Menus", action = "SiteRoot", area = "Presentation", rootmenu = "home", component = default(ComponentPageDescriptor<int>) });

            routeBuilder.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Home}/{id?}");
        }
    }
}