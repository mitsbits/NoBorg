using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Borg.Platform.EF
{
    public abstract class BorgDbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext> where TDbContext : DbContext
    {
        public TDbContext CreateDbContext()
        {
            var envConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();

            var env = envConfig["ENVIRONMENT"];

            var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));

            return CreateDbContext(basePath, env);
        }

        public TDbContext CreateDbContext(DbContextFactoryOptions options)
        {
            return CreateDbContext(
                Directory.GetCurrentDirectory(),
                Environment.GetEnvironmentVariables()["ASPNETCORE_ENVIRONMENT"].ToString());
        }

        private TDbContext CreateDbContext(string basePath, string environmentName)
        {
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()

                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config["borg:ConnectionStrings:db"];

            if (String.IsNullOrWhiteSpace(connstr) == true)
            {
                throw new InvalidOperationException(
                    "Could not find a connection string named 'db'.");
            }
            else
            {
                return CreateDbContext(connstr);
            }
        }

        private TDbContext CreateDbContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(
                    $"{nameof(connectionString)} is null or empty.",
                    nameof(connectionString));

            var optionsBuilder =
                new DbContextOptionsBuilder<TDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            var instance = Activator.CreateInstance(typeof(TDbContext), BindingFlags.Public, optionsBuilder);
            return (TDbContext)instance;
        }

        public TDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }
    }
}