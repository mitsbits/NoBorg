using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Borg.Platform.EF.CMS.Data
{
    public abstract class CmsDbSeedBase
    {
        protected readonly CmsDbContext _db;
 

        protected CmsDbSeedBase(CmsDbContext db)
        {
            _db = db;
        }

        public virtual async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
        }
    }


}