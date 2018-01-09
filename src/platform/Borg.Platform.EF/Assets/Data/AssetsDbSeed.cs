
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.Assets.Data
{
  public  class AssetsDbSeed
    {
        private readonly AssetsDbContext _db;

        public AssetsDbSeed(AssetsDbContext db)
        {
            _db = db;
        }


        public async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
        }
    }


}
