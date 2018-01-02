using Borg.Cms.Basic.Lib.Features.Auth;
using Borg.Cms.Basic.Lib.Features.Auth.Data;
using Borg.Cms.Basic.Lib.Features.Navigation.Contracts;
using Borg.Cms.Basic.Lib.Features.Navigation.Services;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra;
using Borg.Infra.Services.AssemblyProvider;
using Borg.Infra.Storage;
using Borg.MVC;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Services.ServerResponses;
using Borg.MVC.Services.UserSession;
using Borg.Platform.EF.Contracts;
using Borg.Platform.EF.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.Content.Modules;
using Borg.Cms.Basic.Lib.Features.Device.Services;
using Borg.Cms.Basic.Lib.Features.Navigation.Modules;
using Borg.Infra.DTO;
using Borg.MVC.Services;

namespace Borg.Cms.Basic.Lib
{
    public static class ServiceCollectionExtensions
    {
        public static IEnumerable<Assembly> GetRefAssembleisAndRegsiterDefaultProviders(this IServiceCollection services, ILoggerFactory loggerFactory)
        {
            services.AddScoped<IAssemblyProvider, DepedencyAssemblyProvider>();
            services.AddScoped<IAssemblyProvider, ReferenceAssemblyProvider>();
            var p1 = new DepedencyAssemblyProvider(loggerFactory);
            var p2 = new ReferenceAssemblyProvider(loggerFactory);
            var asmbls = p1.GetAssemblies().Union(p2.GetAssemblies()).Where(x => !x.FullName.StartsWith("Microsoft")).Distinct();
            return asmbls;
        }

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
                options.LoginPath = "/backoffice" + settings.Auth.LoginPath;
                options.ExpireTimeSpan = TimeSpan.FromDays(15);
                options.LogoutPath = "/backoffice" + settings.Auth.LogoutPath;
                options.AccessDeniedPath = "/backoffice" + settings.Auth.AccessDeniedPath;

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

        public static IServiceCollection RegisterBorg(this IServiceCollection services, BorgSettings settings, ILoggerFactory loggerFactory, IHostingEnvironment environment)
        {
            services.AddDbContextPool<BorgDbContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings["db"], x => x.MigrationsHistoryTable("__MigrationsHistory", "borg"));
                options.EnableSensitiveDataLogging(environment.IsDevelopment() || environment.EnvironmentName.EndsWith("local"));
            });
            services.AddScoped<IUnitOfWork<BorgDbContext>, UnitOfWork<BorgDbContext>>();
            services.AddScoped<BorgDbSeed>();
            services.AddScoped<IMenuProvider, MenuProvider>();

            services.AddSingleton<IModuleDescriptor, MenuModuleDescriptor>();
            services.AddSingleton<IModuleDescriptor<Tidings>, MenuModuleDescriptor>();

            services.AddSingleton<IModuleDescriptor, BodyViewModuleDescriptor>();
            services.AddSingleton<IModuleDescriptor<Tidings>, BodyViewModuleDescriptor>();

            services.AddSingleton<IModuleDescriptorProvider, ModuleDescriptorProvider>();
            services.AddScoped<IDeviceStructureProvider, DeviceStructureProvider>();
            return services;
        }
    }
}