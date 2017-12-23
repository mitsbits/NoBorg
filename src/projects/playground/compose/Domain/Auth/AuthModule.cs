using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Domain.Auth.Data;
using Domain.Model;
using Domain.Model.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Auth
{

  public  class AuthModule : Module
    {
        private readonly AppSettings _settings;
        private readonly string _dbConnKey;
        private readonly IServiceCollection _services;

        public AuthModule(IServiceCollection services, AppSettings settings, string dbConnKey = "db")
        {
            _services = services;
            _settings = settings;
            _dbConnKey = dbConnKey;
        }



        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var b = new DbContextOptionsBuilder<AuthDbContext>();
                b.UseSqlServer(_settings.ConnectionStrings[_dbConnKey], x => x.MigrationsHistoryTable("__MigrationsHistory", "auth"));
                return new AuthDbContext(b.Options);
            }).InstancePerLifetimeScope();
            _services.AddAuth(_settings, _dbConnKey);
            builder.Register(c => new AuthDbContextFactory(c.Resolve<AppSettings>(), _dbConnKey)).SingleInstance();
            builder.RegisterType<AuthDbSeed>().As<AuthDbSeed>().InstancePerLifetimeScope();
            //builder.Populate(_services);
        }
    }
}
