using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Borg.Bookstore.Configuration;
using Borg.Infra;
using Borg.MVC.Util;
using Borg.Platform.EF.Assets.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Borg.Bookstore
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var appConfig = HostUtility.AppConfiguration(args);
            var settings = new SiteSettings();
            appConfig.Bind(settings);
            Console.Title = settings.ApplicationName;


            BuildWebHost(args).Run();
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

            }
        }
    }


}
