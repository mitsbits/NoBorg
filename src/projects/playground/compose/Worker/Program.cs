using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Worker
{
    class Program
    {
        private static IConfigurationRoot Configuration { get;  set; }
        static void Main(string[] args)
        {
            Configuration = ConfigureSettings();
            Console.WriteLine("Hello World!");
        }


        private static IConfigurationRoot ConfigureSettings()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
