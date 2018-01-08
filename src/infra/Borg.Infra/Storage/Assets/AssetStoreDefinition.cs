using Borg.Infra.Storage.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public abstract class AssetStoreDefinition<TAsset, TKey> : IAssetStore<TAsset, TKey> where TAsset : IAssetInfo<TKey> where TKey : IEquatable<TKey>
    {
        protected readonly ILogger _logger;
        protected readonly IAssetDirectoryStrategy<TKey> _assetDirectoryStrategy;
        protected readonly IConflictingNamesResolver _conflictingNamesResolver;
        protected readonly IFileStorage _fileStorage;
        protected readonly IAssetStoreDatabaseService<TKey> _assetStoreDatabaseService;

        protected AssetStoreDefinition(ILoggerFactory loggerFactory, IAssetDirectoryStrategy<TKey> assetDirectoryStrategy, IConflictingNamesResolver conflictingNamesResolver, IFileStorage fileStorage, IAssetStoreDatabaseService<TKey> assetStoreDatabaseService)
        {
            _assetDirectoryStrategy = assetDirectoryStrategy;
            _conflictingNamesResolver = conflictingNamesResolver;
            _fileStorage = fileStorage;
            _assetStoreDatabaseService = assetStoreDatabaseService;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public abstract Task<TAsset> AddNewVersion(TKey id, byte[] content, string fileName);

        public abstract Task<TAsset> Create(string name, byte[] content, string fileName);

        public abstract Task<IEnumerable<TAsset>> Projections(IEnumerable<TKey> ids);

        public event AssetCreatedEventHandler<TKey> AssetCreated;

        public event VersionCreatedEventHandler<TKey> VersionCreated;

        protected virtual void OnAssetCreated(AssetCreatedEventArgs<TKey> e)
        {
            var handler = AssetCreated;
            handler?.Invoke(e);
        }

        protected virtual void OnVersionCreated(VersionCreatedEventArgs<TKey> e)
        {
            var handler = VersionCreated;
            handler?.Invoke(e);
        }
    }

    public class AssetStoreBase<TKey> : AssetStoreDefinition<AssetInfoDefinition<TKey>, TKey> where TKey : IEquatable<TKey>
    {
        public AssetStoreBase(ILoggerFactory loggerFactory, IAssetDirectoryStrategy<TKey> assetDirectoryStrategy, IConflictingNamesResolver conflictingNamesResolver, IFileStorage fileStorage, IAssetStoreDatabaseService<TKey> assetStoreDatabaseService) : base(loggerFactory, assetDirectoryStrategy, conflictingNamesResolver, fileStorage, assetStoreDatabaseService)
        {
        }

        public override async Task<AssetInfoDefinition<TKey>> AddNewVersion(TKey id, byte[] content, string fileName)
        {
            var hits = await _assetStoreDatabaseService.Find(new[] {id});
            var hit = hits.First();

        }

        public override async Task<IEnumerable<AssetInfoDefinition<TKey>>> Projections(IEnumerable<TKey> ids)
        {
            var results = await _assetStoreDatabaseService.Find(ids);
            return results.Records;
        }

        public override async Task<AssetInfoDefinition<TKey>> Create(string name, byte[] content, string fileName)
        {
            var id = await _assetStoreDatabaseService.AssetNextFromSequence();
            var definition = new AssetInfoDefinition<TKey>(id, name);
            var parentDirecotry = await _assetDirectoryStrategy.ParentFolder(definition);
            var scoped = _fileStorage.Scope(parentDirecotry);
            var exists = await scoped.Exists(fileName);
            if (exists) fileName = await _conflictingNamesResolver.Resolve(fileName);
            await scoped.SaveFile(fileName, new MemoryStream(content), CancellationToken.None);
            var uploaded = await scoped.GetFileInfo(fileName, CancellationToken.None);
            var filespc = new FileSpecDefinition(uploaded.FullPath, name, uploaded.CreationDate, uploaded.LastWrite, uploaded.LastRead, uploaded.SizeInBytes, fileName.GetMimeType());
            var versionspc = new VersionInfoDefinition(1, filespc);
            definition.CurrentFile = versionspc;
            await _assetStoreDatabaseService.Create(definition);
            OnAssetCreated(new AssetCreatedEventArgs<TKey>(id));
            OnVersionCreated(new VersionCreatedEventArgs<TKey>(id, 1));
            return definition;
        }
    }
}