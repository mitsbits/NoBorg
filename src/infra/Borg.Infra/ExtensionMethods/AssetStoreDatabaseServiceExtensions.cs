using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Borg
{
    public static class AssetStoreDatabaseServiceExtensions
    {
        public static async Task<AssetInfoDefinition<TKey>> Get<TKey>(this IAssetStoreDatabaseService<TKey> store, TKey id) where TKey : IEquatable<TKey>
        {
            var hits = await store.Find(new[] { id });
            return hits.SingleOrDefault();
        }

        public static async Task<IMimeTypeSpec> GetFromFileName(this IMimeTypeStoreDatabaseService service, string fiename)
        {
            var ext = Path.GetExtension(fiename);
            return await service.GetFromExtension(ext);
        }
    }
}