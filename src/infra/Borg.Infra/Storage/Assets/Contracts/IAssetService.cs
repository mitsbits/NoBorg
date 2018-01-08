using Borg.Infra.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public delegate Task AssetCreatedEventHandler<TKey>(AssetCreatedEventArgs<TKey> args) where TKey : IEquatable<TKey>;

    public delegate Task VersionCreatedEventHandler<TKey>(VersionCreatedEventArgs<TKey> args) where TKey : IEquatable<TKey>;

    public interface IAssetStore<TAsset, TKey> where TKey : IEquatable<TKey> where TAsset : IAssetInfo<TKey>
    {
        Task<TAsset> Create(string name, byte[] content, string fileName);

        Task<TAsset> AddNewVersion(TKey id, byte[] content, string fileName);

        Task<IEnumerable<TAsset>> Projections(IEnumerable<TKey> ids);

        event AssetCreatedEventHandler<TKey> AssetCreated;

        event VersionCreatedEventHandler<TKey> VersionCreated;
    }

    public interface IAssetStoreDatabaseService<TKey> where TKey : IEquatable<TKey>
    {
        Task<TKey> AssetNextFromSequence();

        Task<TKey> FileNextFromSequence();

        Task<IPagedResult<AssetInfoDefinition<TKey>>> Find(IEnumerable<TKey> ids);

        Task Create(AssetInfoDefinition<TKey> asset);
    }
}