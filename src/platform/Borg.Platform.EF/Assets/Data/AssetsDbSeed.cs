using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Borg.Platform.EF.Assets.Data
{
    public class AssetsDbSeed
    {
        private readonly AssetsDbContext _db;
        private readonly ILogger _logger;

        public AssetsDbSeed(AssetsDbContext db, ILoggerFactory loggerFactory)
        {
            _logger = (loggerFactory == null) ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _db = db;
        }

        public async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
            await EnsureDefaultMimeTypes();
        }

        private async Task EnsureDefaultMimeTypes()
        {
            var mappings = FileStorageExtensions._mappings;
            var persisted = await _db.MimeTypeRecords.AsNoTracking().ToListAsync();
            foreach (var mappingsKey in mappings.Keys)
            {
                if (persisted.All(x => x.Extension.ToLower() != mappingsKey.ToLower()))
                {
                    await _db.MimeTypeRecords.AddAsync(new MimeTypeRecord()
                    {
                        Extension = mappingsKey,
                        MimeType = mappings[mappingsKey]
                    });
                    _logger.Info("Adding {mimetype} with extension {ext} to database", mappings[mappingsKey], mappingsKey);
                }
            }
            await _db.SaveChangesAsync();
        }
    }
}