using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Data
{
    public class DocumentsDbSeed
    {
        private readonly DocumentsDbContext _db;

        public DocumentsDbSeed(DocumentsDbContext db)
        {
            _db = db;
        }

        public async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
        }
    }
}