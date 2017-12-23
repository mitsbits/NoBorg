using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Domain.Model
{
    public class ModelDbContext : DbContext
    {

        public DbSet<Topic> Topics { get; set; }
        public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Topic>().HasKey(x => x.HashTag).ForSqlServerIsClustered();
            builder.Entity<Topic>().Property(x => x.HashTag).HasColumnType("varchar(128)").ValueGeneratedNever();

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.Relational().Schema = "model";
            }
        }
    }

    public class ModelDbContextFactory : IDesignTimeDbContextFactory<ModelDbContext>
    {
        private readonly AppSettings _settings;
        private readonly string _dbConnKey = "db";

        public ModelDbContextFactory()
        {
            
        }
        public ModelDbContextFactory(AppSettings settings, string dbConnKey = "db")
        {
            _settings = settings;
            _dbConnKey = dbConnKey;
        }

        public ModelDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelDbContext>();
            optionsBuilder.UseSqlServer(_settings != null ? _settings.ConnectionStrings[_dbConnKey]: "Server=db;Database=polemic;User=sa;Password=Passw0rd;MultipleActiveResultSets=true", x => x.MigrationsHistoryTable("__MigrationsHistory", "model"));

            return new ModelDbContext(optionsBuilder.Options);
        }
    }
}
