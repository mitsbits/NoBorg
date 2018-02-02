using Borg.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Borg.Cms.Basic.Lib.Features.Auth.Data
{
    public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {
        private readonly BorgSettings _settings;
        private readonly string _dbConnKey = "db";

        public AuthDbContextFactory()
        {
        }

        public AuthDbContextFactory(BorgSettings settings, string dbConnKey = "db")
        {
            _settings = settings;
            _dbConnKey = dbConnKey;
        }

        public AuthDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
            optionsBuilder.UseSqlServer(_settings != null ? _settings.ConnectionStrings[_dbConnKey] : "Server=db;Database=db;User=sa;Password=Passw0rd;MultipleActiveResultSets=true", x => x.MigrationsHistoryTable("__MigrationsHistory", "auth"));

            return new AuthDbContext(optionsBuilder.Options);
        }
    }
}