using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Auth
{
    public static class AuthExtensions
    {
        public static void AddAuth<TSettings>(this IServiceCollection services, TSettings settings, string dbConnKey = "db") where TSettings : AppSettings
        {
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings[dbConnKey]);
            });

            services.AddIdentity<AuthUser, IdentityRole>()
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

                //// Cookie settings
                //options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(15);
                //options.Cookies.ApplicationCookie.LoginPath = "/login";
                //options.Cookies.ApplicationCookie.LogoutPath = "/logoff";

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.ExpireTimeSpan = TimeSpan.FromDays(15);
                options.LogoutPath = "/logoff";
            });

            //services.AddScoped<IUserSession, UserSession>();
            //services.AddScoped<IContextAwareUserSession, UserSession>();
            //services.AddScoped<IApplicationUserSession, UserSession>();
            // Add application services.
            //services.AddTransient<IEmailSender, AuthMessageSender>();
            //services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        //public static void SeedAuth(this IWebHost host, string[] args, BuiltInUserModel builtInUser)
        //{
        //    IServiceScopeFactory services = host.Services.GetService<IServiceScopeFactory>();
        //    EnsureDatabase(services);
        //    Task.WaitAll(EnsureAdmin(services), EnsureManager(services), EnsureEmployee(services));
        //    Task.WaitAll(EnsureDefaultAdminUser(services, builtInUser));
        //}

        //private static async Task EnsureDefaultAdminUser(IServiceScopeFactory services, BuiltInUserModel builtInUser)
        //{
        //    using (var scope = services.CreateScope())
        //    {
        //        UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

        //        var user = await userManager.FindByNameAsync(builtInUser.UserName);
        //        if (user != null)
        //        {
        //            var passwordValid = await userManager.CheckPasswordAsync(user, builtInUser.Password);
        //            if (!passwordValid)
        //            {
        //                await userManager.DeleteAsync(user);
        //                user = null;
        //            }
        //        }
        //        if (user == null)
        //        {
        //            user = new ApplicationUser { UserName = builtInUser.UserName, Email = builtInUser.UserName };
        //            await userManager.CreateAsync(user, builtInUser.Password);
        //            await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
        //            await userManager.SetLockoutEnabledAsync(user, false);
        //        }
        //    }
        //}

        private static void EnsureDatabase(IServiceScopeFactory services)
        {
            using (var scope = services.CreateScope())
            {
                AuthDbContext dbcontext = scope.ServiceProvider.GetService<AuthDbContext>();
                dbcontext.Database.Migrate();
            }
        }

        //private static async Task<IdentityResult> EnsureAdmin(IServiceScopeFactory services)
        //{
        //    using (var scope = services.CreateScope())
        //    {
        //        var roleName = Roles.Admin.ToString();
        //        RoleManager<IdentityRole> manager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        //        var role = await manager.FindByNameAsync(roleName);
        //        if (role == null)
        //        {
        //            role = new IdentityRole(roleName);
        //            return await manager.CreateAsync(role);
        //        }
        //        return IdentityResult.Success;
        //    }
        //}

        //private static async Task<IdentityResult> EnsureManager(IServiceScopeFactory services)
        //{
        //    using (var scope = services.CreateScope())
        //    {
        //        var roleName = Roles.Manager.ToString();
        //        RoleManager<IdentityRole> manager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        //        var role = await manager.FindByNameAsync(roleName);
        //        if (role == null)
        //        {
        //            role = new IdentityRole(roleName);
        //            return await manager.CreateAsync(role);
        //        }
        //        return IdentityResult.Success;
        //    }
        //}

        //private static async Task<IdentityResult> EnsureEmployee(IServiceScopeFactory services)
        //{
        //    using (var scope = services.CreateScope())
        //    {
        //        var roleName = Roles.Employee.ToString();
        //        RoleManager<IdentityRole> manager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        //        var role = await manager.FindByNameAsync(roleName);
        //        if (role == null)
        //        {
        //            role = new IdentityRole(roleName);
        //            return await manager.CreateAsync(role);
        //        }
        //        return IdentityResult.Success;
        //    }
        //}
    }
}
