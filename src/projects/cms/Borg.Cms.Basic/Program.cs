using System;
using Borg.Cms.Basic.Lib.Discovery.Data;
using Borg.Cms.Basic.Lib.Features.Auth.Data;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Platform.EF.Assets.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Borg.Cms.Basic
{
    public class Program
    {
        public static void Main(string[] args)
        {

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
                var borgseed = scope.ServiceProvider.GetRequiredService<BorgDbSeed>();
                borgseed.EnsureUp().Wait(TimeSpan.FromMinutes(1));
                var assetseed = scope.ServiceProvider.GetRequiredService<AssetsDbSeed>();
                assetseed.EnsureUp().Wait(TimeSpan.FromMinutes(1));
                var discoveryseed = scope.ServiceProvider.GetRequiredService<DiscoveryDbSeed>();
                discoveryseed.EnsureUp().Wait(TimeSpan.FromMinutes(1));
            }
        }
    }
}