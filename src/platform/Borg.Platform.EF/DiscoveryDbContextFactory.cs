﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Borg.Platform.EF
{
   public abstract class DiscoveryDbContextFactory<TDbContext> where TDbContext : DbContext
    {
        public TDbContext Create()
        {
            var environmentName =
                Environment.GetEnvironmentVariable(
                    "Hosting:Environment");

            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName);
        }

        public TDbContext Create(DbContextFactoryOptions options)
        {
            return Create(
                Directory.GetCurrentDirectory(),
                Environment.GetEnvironmentVariables()["ASPNETCORE_ENVIRONMENT"].ToString());
        }

        private TDbContext Create(string basePath, string environmentName)
        {
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var settings = new Borg

            var connstr = config.GetConnectionString("db");

            if (String.IsNullOrWhiteSpace(connstr) == true)
            {
                throw new InvalidOperationException(
                    "Could not find a connection string named 'db'.");
            }
            else
            {
                return Create(connstr);
            }
        }

        private TDbContext Create(string connectionString)
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
    }
}
