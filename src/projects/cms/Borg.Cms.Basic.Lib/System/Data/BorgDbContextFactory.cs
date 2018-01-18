using Borg.Infra;
using Borg.MVC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Borg.Cms.Basic.Lib.System.Data
{
    public class BorgDbContextFactory : IDesignTimeDbContextFactory<BorgDbContext>
    {
        private readonly BorgSettings _settings;
        private readonly string _dbConnKey = "db";

        public BorgDbContextFactory()
        {
        }

        public BorgDbContextFactory(BorgSettings settings, string dbConnKey = "db")
        {
            _settings = settings;
            _dbConnKey = dbConnKey;
        }

        BorgDbContext IDesignTimeDbContextFactory<BorgDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BorgDbContext>();
            optionsBuilder.UseSqlServer(_settings != null ? _settings.ConnectionStrings[_dbConnKey] : "Server=db;Database=polemic;User=sa;Password=Passw0rd;MultipleActiveResultSets=true", x => x.MigrationsHistoryTable("__MigrationsHistory", "model"));

            return new BorgDbContext(optionsBuilder.Options);
        }
    }
}