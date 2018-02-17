using Borg.Infra.Collections;
using Borg.Infra.Storage;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
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

        public abstract Task<VersionInfoDefinition> CheckIn(TKey id, FileSpecDefinition<TKey> fileSpec);

        public abstract Task<FileSpecDefinition<TKey>> CurrentFile(TKey id);

        public abstract Task<AssetInfoDefinition<TKey>> AddVersion(AssetInfoDefinition<TKey> hit, FileSpecDefinition<TKey> fileSpec, VersionInfoDefinition versionSpec);

        public abstract Task<FileSpecDefinition<TKey>> VersionFile(TKey id, int version);

        public abstract Task RenameAsset(TKey id, string newName);

        public abstract Task<bool> TryAdd(IMimeTypeSpec mimeType);

        public abstract Task<IEnumerable<IMimeTypeSpec>> MimeTypes();
        public abstract  Task<IEnumerable<IMimeTypeSpec>> MimeTypes(params string[] extensions);

        public abstract Task<IMimeTypeSpec> GetFromExtension(string extension);

        public abstract Task<IEnumerable<IVersionInfo>> AssetVersions(TKey assetId);
        public abstract Task<IFileSpec<TKey>> Spec(TKey fileId);
    }
}