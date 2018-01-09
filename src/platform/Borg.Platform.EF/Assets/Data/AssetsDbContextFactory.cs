using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Borg.Platform.EF.Assets.Data
{
    public class AssetsDbContextFactory : IDesignTimeDbContextFactory<AssetsDbContext>
    {
        private readonly string _dbConnKey = "db";

        public AssetsDbContextFactory()
        {
        }



        AssetsDbContext IDesignTimeDbContextFactory<AssetsDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AssetsDbContext>();
            optionsBuilder.UseSqlServer( "Server=db;Database=polemic;User=sa;Password=Passw0rd;MultipleActiveResultSets=true", x => x.MigrationsHistoryTable("__MigrationsHistory", "assets"));

            return new AssetsDbContext(optionsBuilder.Options);
        }
    }
}