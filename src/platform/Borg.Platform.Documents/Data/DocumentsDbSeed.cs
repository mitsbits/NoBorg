using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Platform.Documents.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Borg.Platform.EF.Contracts;

namespace Borg.Platform.Documents.Data
{
    public class DocumentsDbSeed : IDbSeed
    {
        private readonly DocumentsDbContext _db;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;
        private readonly ILogger _logger;

        public DocumentsDbSeed(ILoggerFactory loggerFactory, DocumentsDbContext db, IAssetStore<AssetInfoDefinition<int>, int> assetStore)
        {
            _logger = (loggerFactory == null) ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _db = db;
            _assetStore = assetStore;
        }

        public async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
            await EnsureImageGrouping();
        }

        private async Task EnsureImageGrouping()
        {
            var groupingName = "Images";
            var mimes = await _assetStore.MimeTypes();
            var imagemimes = mimes.Where(x => x.MimeType.ToLower().Contains("image"));
            var hit = await _db.MimeTypeGroupingStates.Include(x => x.Extensions).FirstOrDefaultAsync(x => x.Name == groupingName);
            if (hit == null)
            {
                hit = new MimeTypeGroupingState() { Name = groupingName };
                foreach (var mimeTypeSpec in imagemimes)
                {
                    hit.Extensions.Add(new MimeTypeGroupingExtensionState() { Extension = mimeTypeSpec.Extension });
                }
                await _db.MimeTypeGroupingStates.AddAsync(hit);
            }
            else
            {
                var newmimes = imagemimes.Where(x => !hit.Extensions.Select(y => y.Extension).Contains(x.Extension));
                foreach (var mimeTypeSpec in newmimes)
                {
                    hit.Extensions.Add(new MimeTypeGroupingExtensionState() { Extension = mimeTypeSpec.Extension });
                }
                _db.Entry(hit).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();
        }

        private async Task EnsureDefaultMimeTypes()
        {
            var mappings = FileStorageExtensions._mappings;
            var persisted = await _db.MimeTypeRecords.AsNoTracking().ToListAsync();
            foreach (var mappingsKey in mappings.Keys)
            {
                if (persisted.All(x => x.Extension.ToLower() != mappingsKey.ToLower()))
                {
                    await _db.MimeTypeRecords.AddAsync(new MimeTypeState()
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