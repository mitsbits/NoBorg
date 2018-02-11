using Borg.Cms.Basic.Lib.Features.Auth.Data;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Infra;
using Borg.Infra.Services;
using Borg.MVC.Util;
using Borg.Platform.EF.Assets.Data;
using Borg.Platform.EF.CMS.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;

namespace Borg.Cms.Basic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Borg Cms Basic";
            var host = BuildWebHost(args);

            var appConfig = HostUtility.AppConfiguration(args);
            var settings = new BorgSettings();
            appConfig.GetSection("atlas").Bind(settings);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Information)
                .WriteTo.ColoredConsole()
                .CreateLogger();

            ApplicationLogging.SetFactory(host.Services.GetRequiredService<ILoggerFactory>());

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
                //var borgseed = scope.ServiceProvider.GetRequiredService<BorgDbSeed>();
                //borgseed.EnsureUp().Wait(TimeSpan.FromMinutes(1));
                var assetseed = scope.ServiceProvider.GetRequiredService<AssetsDbSeed>();
                assetseed.EnsureUp().Wait(TimeSpan.FromMinutes(1));
                var cmsseed = scope.ServiceProvider.GetRequiredService<CmsDbSeed>();
                cmsseed.EnsureUp().Wait(TimeSpan.FromMinutes(1));
                //var discoveryseed = scope.ServiceProvider.GetRequiredService<DiscoveryDbSeed>();
                //discoveryseed.EnsureUp().Wait(TimeSpan.FromMinutes(1));
                var documentseed = scope.ServiceProvider.GetRequiredService<DocumentsDbSeed>();
                documentseed.EnsureUp().Wait(TimeSpan.FromMinutes(1));
            }
        }
    }
}