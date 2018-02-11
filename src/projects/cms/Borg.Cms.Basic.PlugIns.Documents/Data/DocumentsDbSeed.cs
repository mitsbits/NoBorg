using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Data
{
    public class DocumentsDbSeed
    {
        private readonly DocumentsDbContext _db;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;

        public DocumentsDbSeed(DocumentsDbContext db, IAssetStore<AssetInfoDefinition<int>, int> assetStore)
        {
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
    }
}