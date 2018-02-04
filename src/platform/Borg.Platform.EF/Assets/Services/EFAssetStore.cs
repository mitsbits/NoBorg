using Borg.Infra.Collections;
using Borg.Infra.Storage;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.Platform.EF.Assets.Services
{
    public abstract class EFAssetsDatabaseService<TKey> : IAssetStoreDatabaseService<TKey> where TKey : IEquatable<TKey>
    {
        public abstract Task<TKey> AssetNextFromSequence();

        public abstract Task<TKey> FileNextFromSequence();

        public abstract Task<IPagedResult<AssetInfoDefinition<TKey>>> Find(IEnumerable<TKey> ids);

        public abstract Task Create(AssetInfoDefinition<TKey> asset);

        public abstract Task<VersionInfoDefinition> CheckOut(TKey id);

        public abstract Task<AssetInfoDefinition<TKey>> AddVersion(AssetInfoDefinition<TKey> hit, FileSpecDefinition<TKey> fileSpec, VersionInfoDefinition versionSpec);
    }
}