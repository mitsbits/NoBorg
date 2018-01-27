using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Borg.Platform.EF.CMS.Data
{
    public class CmsDbSeed
    {
        private readonly CmsDbContext _db;

        public CmsDbSeed(CmsDbContext db)
        {
            _db = db;
        }

        public async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
        }
    }
}