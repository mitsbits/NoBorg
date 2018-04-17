using Borg.Cms.Basic.Lib.Discovery.Data;
using Borg.Cms.Basic.Lib.Features.Auth;
using Borg.Cms.Basic.Lib.Features.Auth.Data;
using Borg.Cms.Basic.Lib.Features.CMS.Data;
using Borg.Cms.Basic.Lib.Features.Content.Services;
using Borg.Cms.Basic.Lib.Features.Device.Services;
using Borg.Cms.Basic.Lib.Features.Navigation.Contracts;
using Borg.Cms.Basic.Lib.Features.Navigation.Services;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra;
using Borg.Infra.Caching;
using Borg.Infra.Caching.Contracts;
using Borg.Infra.DTO;
using Borg.Infra.Storage;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Filters;
using Borg.MVC.PlugIns.Contracts;
using Borg.MVC.PlugIns.Decoration;
using Borg.MVC.Services;
using Borg.MVC.Services.Editors;
using Borg.MVC.Services.ServerResponses;
using Borg.MVC.Services.UserSession;
using Borg.Platform.EF.Assets.Data;
using Borg.Platform.EF.Assets.Services;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using Borg.Platform.EF.DAL;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterCommonFramework(this IServiceCollection services, BorgSettings settings, ILoggerFactory loggerFactory)
        {
            services.AddPagination(() => settings.PaginationInfoStyle);

            services.AddScoped<IPageOrchestrator<IPageContent, IDevice>, PageOrchestrator>();
            services.AddScoped<IPageOrchestrator, PageOrchestrator>();
            services.AddScoped<IDeviceAccessor<IDevice>, PageOrchestrator>();
            services.AddScoped<IPageContentAccessor<IPageContent>, PageOrchestrator>();
            services.AddSingleton<ISerializer, JsonNetSerializer>();
            services.AddScoped<ISessionServerResponseProvider, TempDataResponseProvider>();
            services.AddTransient(factory =>
            {
                IFileStorage Accesor(string key)
                {
                    var root = new FolderFileStorage(settings.Storage.Folder, loggerFactory);
                    return !string.IsNullOrWhiteSpace(key) ? root.Scope(key) : root;
                }
                return (Func<string, IFileStorage>)Accesor;
            });
            services.AddScoped<DeviceLayoutFilter>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddBorgDefaultSlugifier();

            services.AddDistributedSqlServerCache(options =>
            {
                options.SchemaName = "cache";
                options.TableName = "Store";
                options.ConnectionString = settings.ConnectionStrings["db"];
            });
            services.AddScoped<ISerializer, JsonNetSerializer>();
            services.AddScoped<ICacheStore, CacheStore>();

            return services;
        }

        public static IServiceCollection RegisterAuth(this IServiceCollection services, BorgSettings settings, ILoggerFactory loggerFactory, IHostingEnvironment environment)
        {
            services.AddScoped<IContextAwareUserSession, BorgUserSession>();

            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings["db"], x => x.MigrationsHistoryTable("__MigrationsHistory", "auth"));
                options.EnableSensitiveDataLogging(environment.IsDevelopment() || environment.EnvironmentName.EndsWith("local"));
            });

            services.AddIdentity<CmsUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                //options.LoginPath = settings.Auth.LoginPath;
                //options.ExpireTimeSpan = TimeSpan.FromDays(15);
                //options.LogoutPath = settings.Auth.LogoutPath;
                //options.AccessDeniedPath = settings.Auth.AccessDeniedPath;

                options.Cookie.Name = "Borg.Auth";
                options.Cookie.HttpOnly = true;

                options.Events.OnValidatePrincipal = context =>
                {
                    //var c = context;
                    return Task.CompletedTask;
                };
                options.Events.OnSignedIn = context =>
                {
                    //var lf = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
                    //var log = lf.CreateLogger("signin");
                    //log.Info("{user} signed in", context.Principal.Identity.Name);
                    return Task.CompletedTask;
                };
            });

            //services.AddAuthorization(options =>
            //{
            //    //options.AddPolicy("Backoffice", policy => policy.RequireRole(CmsRoles.Backoffice.ToString(), SystemRoles.Admin.ToString()));
            //});
            services.AddSingleton<IClaimTypeDisplayProvider, ClaimTypeDisplayProvider>();
            services.AddScoped<IUnitOfWork<AuthDbContext>, UnitOfWork<AuthDbContext>>();
            services.AddScoped<AuthDbSeed>();
            return services;
        }

        public static IServiceCollection RegisterBorg(this IServiceCollection services, BorgSettings settings, ILoggerFactory loggerFactory, IHostingEnvironment environment, IEnumerable<Assembly> assembliesToScan)
        {
            services.AddDbContext<BorgDbContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings["db"], x => x.MigrationsHistoryTable("__MigrationsHistory", "borg")).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                options.EnableSensitiveDataLogging(environment.IsDevelopment() || environment.EnvironmentName.EndsWith("local"));
            });
            services.AddScoped<IUnitOfWork<BorgDbContext>, UnitOfWork<BorgDbContext>>();
            services.AddScoped<BorgDbSeed>();
            services.AddScoped<IMenuProvider, MenuProvider>();

            services.AddDbContext<AssetsDbContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings["db"], x => x.MigrationsHistoryTable("__MigrationsHistory", "assets")).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                options.EnableSensitiveDataLogging(environment.IsDevelopment() || environment.EnvironmentName.EndsWith("local"));
            });
            services.AddDbContext<DiscoveryDbContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings["db"], x => x.MigrationsHistoryTable("__MigrationsHistory", "discovery")).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                options.EnableSensitiveDataLogging(environment.IsDevelopment() || environment.EnvironmentName.EndsWith("local"));
            });

            services.AddScoped<AssetsDbSeed>();
            services.AddScoped<CmsDbSeed>();

            services.AddDbContext<CmsDbContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings["db"], x => x.MigrationsHistoryTable("__MigrationsHistory", "cms")).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                options.EnableSensitiveDataLogging(environment.IsDevelopment() || environment.EnvironmentName.EndsWith("local"));
            });
            services.AddScoped<IUnitOfWork<CmsDbContext>, UnitOfWork<CmsDbContext>>();

            services.Add(new ServiceDescriptor(typeof(IAssetStore<AssetInfoDefinition<int>, int>),
                p => new AssetService(loggerFactory,
                p.GetRequiredService<IAssetDirectoryStrategy<int>>(),
                p.GetRequiredService<IConflictingNamesResolver>(),
                //() => new AzureFileStorage(settings.Storage.AzureStorageConnection, settings.Storage.AssetStoreContainer),
                    () => new FolderFileStorage(Path.Combine(environment.WebRootPath, settings.Storage.AssetStoreContainer), loggerFactory),
                p.GetRequiredService<IAssetStoreDatabaseService<int>>()),
                ServiceLifetime.Scoped));

            services.AddScoped<IAssetStoreDatabaseService<int>, EfAssetsSequencedDatabaseService>();
            services.AddScoped<IConflictingNamesResolver, DefaultConflictingNamesResolver>();
            services.Add(new ServiceDescriptor(typeof(IAssetDirectoryStrategy<int>),
                p => new RoundUpAssetDirectoryStrategy(10, 50), ServiceLifetime.Scoped));

            foreach (var assembly in assembliesToScan)
            {
                var moduledscrs = assembly.GetTypes().Where(t =>
                    t.IsNonAbstractClass(false) && t.GetInterface(nameof(IModuleDescriptor), true) != null);
                if (moduledscrs.Any())
                {
                    foreach (var moduledscr in moduledscrs)
                    {
                        var intrfs = moduledscr.GetInterfaces();
                        foreach (var intrf in intrfs)
                        {
                            services.Add(new ServiceDescriptor(intrf, moduledscr, ServiceLifetime.Singleton));
                        }
                        services.TryAdd(new ServiceDescriptor(typeof(IModuleDescriptor), moduledscr, ServiceLifetime.Singleton));
                        services.TryAdd(new ServiceDescriptor(typeof(IModuleDescriptor<Tidings>), moduledscr, ServiceLifetime.Singleton));
                    }
                }
            }
            var decriptors = assembliesToScan.SelectMany(x =>
                    x.GetTypes().Where(t => t.IsSealed && t.ImplementsInterface(typeof(IEditorDescriptor))))
                .Distinct();
            foreach (var decriptor in decriptors)
            {
                services.Add(new ServiceDescriptor(typeof(IEditorDescriptor), decriptor, ServiceLifetime.Scoped));
            }
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CorrelationBehavior<,>));

            services.AddSingleton<IModuleDescriptorProvider, ModuleDescriptorProvider>();

            services.AddSingleton<IDeviceLayoutFileProvider, DeviceLayoutFileProvider>();

            var entrypointassemblies = assembliesToScan.Where(x =>
                    x.GetTypes().Any(t => t.GetCustomAttributes<PlugInEntryPointControllerAttribute>() != null)).Distinct()
                .ToArray();

            services.Add(new ServiceDescriptor(typeof(IDeviceLayoutFileProvider),
                (p) => new DeviceLayoutFileProvider(environment, settings, p.GetRequiredService<IPlugInHost>(),
                    p.GetRequiredService<IRazorViewEngine>(), entrypointassemblies), ServiceLifetime.Singleton));

            services.AddScoped<IDeviceStructureProvider, DeviceStructureProvider>();

            services.AddScoped<DiscoveryDbSeed>();
            return services;
        }
    }
}