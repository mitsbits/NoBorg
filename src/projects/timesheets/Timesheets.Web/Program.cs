using Borg.MVC;
using Borg.MVC.Util;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using Timesheets.Web.Auth;
using Timesheets.Web.Domain;

namespace Timesheets.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Borg Web Client";

            var appConfig = TimesheetsConfiguration(args);
            var settings = new BorgSettings();
            appConfig.GetSection("borg").Bind(settings);

            var buildInUser = new BuiltInUserModel();
            appConfig.GetSection("BuildInUser").Bind(buildInUser);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                //.WriteTo.RollingFile("log-{Date}.txt")
                //.WriteTo.AzureTableStorageWithProperties(storage, LogEventLevel.Verbose, null, "AtlasDebugLog")
                //.WriteTo.AzureTableStorageWithProperties(storage).Filter.ByExcluding(Matching.FromSource("Microsoft")).MinimumLevel.Verbose()
                //.WriteTo.Logger(l => l.Filter.ByExcluding(Matching.FromSource("Microsoft")).WriteTo.AzureTableStorage(storage, LogEventLevel.Information, null, "AtlasLog"))
                //.WriteTo.Logger(l => l.Filter.ByIncludingOnly(Matching.FromSource("Microsoft")).WriteTo.AzureTableStorage(storage, LogEventLevel.Warning, null, "AtlasLog"))
                //.WriteTo.Logger(l => l.Filter.ByExcluding(Matching.FromSource("Microsoft")).WriteTo.AzureTableStorage(storage, LogEventLevel.Debug, null, "AtlasDebugLog"))
                .WriteTo.ColoredConsole(LogEventLevel.Debug)
                .CreateLogger();

            var host = BuildWebHost(args);

            host.SeedAuth(args, buildInUser);
            host.SeedDomain(args);

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    IHostingEnvironment env = builderContext.HostingEnvironment;

                    config.AddJsonFile("config.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);
                })
                .UseIISIntegration()
                .UseSerilog()
                .Build();

        private static IConfigurationRoot TimesheetsConfiguration(string[] args)
        {
            var env = HostUtility.CurrentEnvironment(args);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddCommandLine(args)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .AddJsonFile("config.json", optional: true)
                .AddJsonFile($"config.{env}.json", optional: true)
                .Build();
            return config;
        }
    }
}