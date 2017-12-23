using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Domain.Auth
{
    public class AuthDbContext : IdentityDbContext<AuthUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.Relational().Schema = "auth";
            }
        }
    }
    public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {
        private readonly AppSettings _settings;
        private readonly string _dbConnKey = "db";

        public AuthDbContextFactory()
        {

        }
        public AuthDbContextFactory(AppSettings settings, string dbConnKey = "db")
        {
            _settings = settings;
            _dbConnKey = dbConnKey;
        }

        public AuthDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
            optionsBuilder.UseSqlServer(_settings != null ? _settings.ConnectionStrings[_dbConnKey] : "Server=db;Database=polemic;User=sa;Password=Passw0rd;MultipleActiveResultSets=true", x => x.MigrationsHistoryTable("__MigrationsHistory", "auth"));

            return new AuthDbContext(optionsBuilder.Options);
        }
    }
    public class AuthUser : IdentityUser
    {
    }
}