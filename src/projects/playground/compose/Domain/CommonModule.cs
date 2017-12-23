using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Domain.Model.Data;

namespace Domain
{
    public class CommonModule : Module
    {
        private readonly AppSettings _settings;
        private readonly string _dbConnKey;

        public CommonModule(AppSettings settings, string dbConnKey = "db")
        {
            _settings = settings;
            _dbConnKey = dbConnKey;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //_services.AddDbContextPool<ModelDbContext>(opt =>
            //    opt.UseSqlServer(_settings.ConnectionStrings[_dbConnKey]));

            builder.Register(c =>
            {
                var b = new DbContextOptionsBuilder<ModelDbContext>();
                b.UseSqlServer(_settings.ConnectionStrings[_dbConnKey], x => x.MigrationsHistoryTable("__MigrationsHistory", "model"));
                return new ModelDbContext(b.Options);
            }).InstancePerLifetimeScope();


            builder.Register(c => new ModelDbContextFactory(c.Resolve<AppSettings>(), _dbConnKey)).SingleInstance();
            builder.RegisterType<ModelDbSeed>().As<ModelDbSeed>().InstancePerLifetimeScope();
            //builder.Populate(_services);
        }
    }
}
