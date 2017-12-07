using Microsoft.Extensions.Configuration;
using System.IO;

namespace Borg.MVC.Util
{
    public class HostUtility
    {
        public static IConfigurationRoot HostConfiguration(string[] args)
        {
            var env = CurrentEnvironment(args);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddCommandLine(args)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .AddJsonFile("hosting.json", optional: true)
                .AddJsonFile($"hosting.{env}.json", optional: true)
                .Build();
            return config;
        }

        public static IConfigurationRoot AppConfiguration(string[] args)
        {
            var env = CurrentEnvironment(args);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddCommandLine(args)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .Build();
            return config;
        }

        public static string CurrentEnvironment(string[] args)
        {
            var envConfig = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();

            var env = envConfig["ENVIRONMENT"];
            return env;
        }
    }
}