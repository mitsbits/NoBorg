using Borg.Bookstore.Configuration;
using Borg.MVC.Util;
using Borg.Platform.Identity.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Borg.Bookstore.Data;

namespace Borg.Bookstore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appConfig = HostUtility.AppConfiguration(args);
            var settings = new ApplicationConfig();
            appConfig.Bind(settings);
            Console.Title = settings.Tenant.ServiceTag;

            var host = BuildWebHost(args);
            Seed(host);
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        private static void Seed(IWebHost host)
        {
            IServiceScopeFactory services = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = services.CreateScope())
            {
                var authseed = scope.ServiceProvider.GetRequiredService<AuthDbSeed>();
                authseed.EnsureUp().Wait(TimeSpan.FromMinutes(1));
                var bookstoreseed = scope.ServiceProvider.GetRequiredService<BookstoreDbSeed>();
                bookstoreseed.EnsureUp().Wait(TimeSpan.FromMinutes(1));
            }
        }
    }
}