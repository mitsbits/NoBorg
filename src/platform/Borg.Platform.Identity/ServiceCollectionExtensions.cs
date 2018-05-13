using System.Collections.Generic;
using System.Linq;
using Borg.Infra.Configuration.Contracts;
using Borg.Platform.EF.Contracts;
using Borg.Platform.EF.DAL;
using Borg.Platform.Identity.Configuration;
using Borg.Platform.Identity.Data;
using Borg.Platform.Identity.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAuth(this IServiceCollection services, ILoggerFactory loggerFactory, 
            IHostingEnvironment environment, ISettingsProvider<IdentityConfig> settingsProvider,
            IDictionary<string, AuthorizationPolicy> policies)
        {
            var settings = settingsProvider.Config;
            //services.AddScoped<IContextAwareUserSession, BorgUserSession>();

            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(settings.SqlConnectionString, x => x.MigrationsHistoryTable("__MigrationsHistory", "auth"));
                options.EnableSensitiveDataLogging(environment.IsDevelopment());
            });

            services.AddIdentity<GenericUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password = settings.PasswordOptions;

                // Lockout settings
                options.Lockout = settings.LockoutOptions;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = settings.LoginPath;
                options.ExpireTimeSpan = settings.ExpireTimeSpan;
                options.LogoutPath = settings.LogoutPath;
                options.AccessDeniedPath = settings.AccessDeniedPath;

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
            if (policies != null && policies.Any())
            {
                foreach (var policyname in policies.Keys)
                {

                }
            }
            services.AddAuthorization(options =>
            {
                if (policies == null || !policies.Any()) return;
                foreach (var policyname in policies.Keys)
                {
                    options.AddPolicy(policyname, policies[policyname]);
                }
                //options.AddPolicy("Backoffice", policy => policy.RequireRole(CmsRoles.Backoffice.ToString(), SystemRoles.Admin.ToString()));
            });
            //services.AddSingleton<IClaimTypeDisplayProvider, ClaimTypeDisplayProvider>();
            services.AddScoped<IUnitOfWork<AuthDbContext>, UnitOfWork<AuthDbContext>>();
            services.AddScoped<IDbSeed, AuthDbSeed>(); 
   
            services.AddScoped< AuthDbSeed>();
            return services;
        }
    }
}