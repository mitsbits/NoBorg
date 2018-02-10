using Borg.Infra.Collections;
using Borg.Infra.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public delegate Task AssetCreatedEventHandler<TKey>(AssetCreatedEventArgs<TKey> args) where TKey : IEquatable<TKey>;

    public delegate Task VersionCreatedEventHandler<TKey>(VersionCreatedEventArgs<TKey> args) where TKey : IEquatable<TKey>;

    public interface IAssetStore<TAsset, TKey> : IVersionStoreDatabaseService<TKey>, IMimeTypestoreDatabaseService where TKey : IEquatable<TKey> where TAsset : IAssetInfo<TKey>
    {
        Task<TAsset> Create(string name, byte[] content, string fileName);

        Task<TAsset> AddNewVersion(TKey id, byte[] content, string fileName);

        Task<IVersionInfo> CheckOut(TKey id);

        Task<IVersionInfo> CheckIn(TKey id, byte[] content, string fileName);

        Task<IEnumerable<TAsset>> Projections(IEnumerable<TKey> ids);

        Task<Stream> CurrentFile(TKey assetId);

        Task<Stream> VersionFile(TKey assetId, int version);

        event AssetCreatedEventHandler<TKey> AssetCreated;

        event VersionCreatedEventHandler<TKey> VersionCreated;
    }

    public interface IAssetStoreDatabaseService<TKey> : IVersionStoreDatabaseService<TKey>, IMimeTypestoreDatabaseService where TKey : IEquatable<TKey>
    {
        Task<TKey> AssetNextFromSequence();

        Task<TKey> FileNextFromSequence();

        Task<IPagedResult<AssetInfoDefinition<TKey>>> Find(IEnumerable<TKey> ids);

        Task Create(AssetInfoDefinition<TKey> asset);

        Task<VersionInfoDefinition> CheckOut(TKey id);

        Task<VersionInfoDefinition> CheckIn(TKey id, FileSpecDefinition<TKey> fileSpec);

        Task<FileSpecDefinition<TKey>> CurrentFile(TKey id);

        Task<AssetInfoDefinition<TKey>> AddVersion(AssetInfoDefinition<TKey> hit, FileSpecDefinition<TKey> fileSpec, VersionInfoDefinition versionSpec);

        Task<FileSpecDefinition<TKey>> VersionFile(TKey id, int version);
    }

    public interface IMimeTypestoreDatabaseService
    {
        Task<bool> TryAdd(IMimeTypeSpec mimeType);

        Task<IEnumerable<IMimeTypeSpec>> MimeTypes();

        Task<IMimeTypeSpec> GetFromExtension(string extension);
    }

    public interface IVersionStoreDatabaseService<in TKey> where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<IVersionInfo>> AssetVersions(TKey assetId);
    }

    public static class IAssetStoreDatabaseServiceExtensions
    {
        public static async Task<AssetInfoDefinition<TKey>> Get<TKey>(this IAssetStoreDatabaseService<TKey> store, TKey id) where TKey : IEquatable<TKey>
        {
            var hits = await store.Find(new[] { id });
            return hits.SingleOrDefault();
        }

        public static async Task<IMimeTypeSpec> GetFromFileName(this IMimeTypestoreDatabaseService service, string fiename)
        {
            var ext = Path.GetExtension(fiename);
            return await service.GetFromExtension(ext);
        }
    }
}