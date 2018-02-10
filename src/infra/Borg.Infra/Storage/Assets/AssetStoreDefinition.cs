using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets
{
    public abstract class AssetStoreDefinition<TAsset, TKey> : IAssetStore<TAsset, TKey> where TAsset : IAssetInfo<TKey> where TKey : IEquatable<TKey>
    {
        protected readonly ILogger _logger;
        protected readonly IAssetDirectoryStrategy<TKey> _assetDirectoryStrategy;
        protected readonly IConflictingNamesResolver _conflictingNamesResolver;
        protected readonly Func<IFileStorage> _fileStorageFactory;
        protected readonly IAssetStoreDatabaseService<TKey> _assetStoreDatabaseService;

        protected AssetStoreDefinition(ILoggerFactory loggerFactory, IAssetDirectoryStrategy<TKey> assetDirectoryStrategy, IConflictingNamesResolver conflictingNamesResolver, Func<IFileStorage> fileStorageFactory, IAssetStoreDatabaseService<TKey> assetStoreDatabaseService)
        {
            _assetDirectoryStrategy = assetDirectoryStrategy;
            _conflictingNamesResolver = conflictingNamesResolver;
            _fileStorageFactory = fileStorageFactory;
            _assetStoreDatabaseService = assetStoreDatabaseService;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public abstract Task<TAsset> AddNewVersion(TKey id, byte[] content, string fileName);

        public async Task<IVersionInfo> CheckOut(TKey id)
        {
            return await _assetStoreDatabaseService.CheckOut(id);
        }

        public abstract Task<IVersionInfo> CheckIn(TKey id, byte[] content, string fileName);

        public abstract Task<TAsset> Create(string name, byte[] content, string fileName);

        public abstract Task<IEnumerable<TAsset>> Projections(IEnumerable<TKey> ids);

        public abstract Task<Stream> VersionFile(TKey assetId, int version);

        public event Contracts.AssetCreatedEventHandler<TKey> AssetCreated;

        public event VersionCreatedEventHandler<TKey> VersionCreated;

        public abstract Task<Stream> CurrentFile(TKey assetId);

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

        public async Task<bool> TryAdd(IMimeTypeSpec mimeType)
        {
            return await _assetStoreDatabaseService.TryAdd(mimeType);
        }

        public async Task<IEnumerable<IMimeTypeSpec>> MimeTypes()
        {
            return await _assetStoreDatabaseService.MimeTypes();
        }

        public async Task<IMimeTypeSpec> GetFromExtension(string extension)
        {
            return await _assetStoreDatabaseService.GetFromExtension(extension);
        }

        public async Task<IEnumerable<IVersionInfo>> AssetVersions(TKey assetId)
        {
            return await _assetStoreDatabaseService.AssetVersions(assetId);
        }
    }

    public class AssetStoreBase<TKey> : AssetStoreDefinition<AssetInfoDefinition<TKey>, TKey> where TKey : IEquatable<TKey>
    {
        public AssetStoreBase(ILoggerFactory loggerFactory, IAssetDirectoryStrategy<TKey> assetDirectoryStrategy, IConflictingNamesResolver conflictingNamesResolver, Func<IFileStorage> fileStorageFactory, IAssetStoreDatabaseService<TKey> assetStoreDatabaseService) : base(loggerFactory, assetDirectoryStrategy, conflictingNamesResolver, fileStorageFactory, assetStoreDatabaseService)
        {
        }

        public override async Task<AssetInfoDefinition<TKey>> AddNewVersion(TKey id, byte[] content, string fileName)
        {
            return await AddNewVersionInternal(id, content, fileName);
        }

        public override async Task<IVersionInfo> CheckIn(TKey id, byte[] content, string fileName)
        {
            try
            {
                var fileId = await _assetStoreDatabaseService.FileNextFromSequence();
                var fileSpec = new FileSpecDefinition<TKey>(fileId);
                var parentDirecotry = await _assetDirectoryStrategy.ParentFolder(fileSpec);

                //upload file
                IFileSpec uploaded;
                using (var storage = _fileStorageFactory.Invoke())
                using (var scoped = storage.Scope(parentDirecotry))
                {
                    var exists = await scoped.Exists(fileName);
                    if (exists) fileName = await _conflictingNamesResolver.Resolve(fileName);
                    using (var stream = new MemoryStream(content))
                    {
                        await scoped.SaveFile(fileName, stream, CancellationToken.None);
                        uploaded = await scoped.GetFileInfo(fileName, CancellationToken.None);
                    }
                }

                fileSpec = new FileSpecDefinition<TKey>(fileId, uploaded.FullPath, fileName, uploaded.CreationDate, uploaded.LastWrite, uploaded.LastRead, uploaded.SizeInBytes, fileName.GetMimeType());
                await _assetStoreDatabaseService.CheckIn(id, fileSpec);
                var asset = await _assetStoreDatabaseService.Get(id);
                return new VersionInfoDefinition(asset.CurrentFile.Version, asset.CurrentFile.FileSpec);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }

        public override async Task<IEnumerable<AssetInfoDefinition<TKey>>> Projections(IEnumerable<TKey> ids)
        {
            return await ProjectionsInternal(ids);
        }

        public override async Task<Stream> CurrentFile(TKey assetId)
        {
            try
            {
                var filespec = await _assetStoreDatabaseService.CurrentFile(assetId);
                var directory = await _assetDirectoryStrategy.ParentFolder(filespec);
                var strean = new MemoryStream();
                using (var storage = _fileStorageFactory.Invoke())
                using (var scoped = storage.Scope(directory))
                {
                    using (var fstream = await scoped.GetFileStream(Path.GetFileName(filespec.FullPath)))
                    {
                        await fstream.CopyToAsync(strean);
                    }
                }
                return strean;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }

        public override async Task<Stream> VersionFile(TKey assetId, int version)
        {
            try
            {
                var filespec = await _assetStoreDatabaseService.VersionFile(assetId, version);
                var directory = await _assetDirectoryStrategy.ParentFolder(filespec);
                var strean = new MemoryStream();
                using (var storage = _fileStorageFactory.Invoke())
                using (var scoped = storage.Scope(directory))
                {
                    using (var fstream = await scoped.GetFileStream(Path.GetFileName(filespec.FullPath)))
                    {
                        await fstream.CopyToAsync(strean);
                    }
                }
                return strean;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }

        public override async Task<AssetInfoDefinition<TKey>> Create(string name, byte[] content, string fileName)
        {
            return await CreateInternal(name, content, fileName);
        }

        #region Internal --to be intercepted

        private async Task<AssetInfoDefinition<TKey>> AddNewVersionInternal(TKey id, byte[] content, string fileName)
        {
            //preperation
            var hits = await _assetStoreDatabaseService.Find(new[] { id });
            var hit = hits.First();
            var fileId = await _assetStoreDatabaseService.FileNextFromSequence();
            var fileSpec = new FileSpecDefinition<TKey>(fileId);
            var directory = await _assetDirectoryStrategy.ParentFolder(fileSpec);

            //upload file
            IFileSpec uploaded;
            using (var storage = _fileStorageFactory.Invoke())
            using (var scoped = storage.Scope(directory))
            {
                var exists = await scoped.Exists(fileName);
                if (exists) fileName = await _conflictingNamesResolver.Resolve(fileName);
                using (var stream = new MemoryStream(content))
                {
                    await scoped.SaveFile(fileName, stream, CancellationToken.None);
                    uploaded = await scoped.GetFileInfo(fileName, CancellationToken.None);
                }
            }

            //persist to database
            var filespc = new FileSpecDefinition(uploaded.FullPath, hit.Name, uploaded.CreationDate, uploaded.LastWrite, uploaded.LastRead, uploaded.SizeInBytes, fileName.GetMimeType());
            var versionSpec = new VersionInfoDefinition(-1, filespc);

            //return
            return await _assetStoreDatabaseService.AddVersion(hit, fileSpec, versionSpec);
        }

        private async Task<IEnumerable<AssetInfoDefinition<TKey>>> ProjectionsInternal(IEnumerable<TKey> ids)
        {
            var results = await _assetStoreDatabaseService.Find(ids);
            return results.Records;
        }

        private async Task<AssetInfoDefinition<TKey>> CreateInternal(string name, byte[] content, string fileName)
        {
            //preperation
            var id = await _assetStoreDatabaseService.AssetNextFromSequence();
            var fileId = await _assetStoreDatabaseService.FileNextFromSequence();
            var fileSpec = new FileSpecDefinition<TKey>(fileId);
            var parentDirecotry = await _assetDirectoryStrategy.ParentFolder(fileSpec);
            var definition = new AssetInfoDefinition<TKey>(id, name);

            //upload file
            IFileSpec uploaded;
            using (var storage = _fileStorageFactory.Invoke())
            using (var scoped = storage.Scope(parentDirecotry))
            {
                var exists = await scoped.Exists(fileName);
                if (exists) fileName = await _conflictingNamesResolver.Resolve(fileName);
                using (var stream = new MemoryStream(content))
                {
                    await scoped.SaveFile(fileName, stream, CancellationToken.None);
                    uploaded = await scoped.GetFileInfo(fileName, CancellationToken.None);
                }
            }

            //persist to database
            var filespc = new FileSpecDefinition<TKey>(fileId, uploaded.FullPath, fileName, uploaded.CreationDate, uploaded.LastWrite, uploaded.LastRead, uploaded.SizeInBytes, fileName.GetMimeType());
            var versionspc = new VersionInfoDefinition(1, filespc);
            definition.CurrentFile = versionspc;
            await _assetStoreDatabaseService.Create(definition);

            //raise events
            OnAssetCreated(new AssetCreatedEventArgs<TKey>(id));
            OnVersionCreated(new VersionCreatedEventArgs<TKey>(id, 1));

            //retuen
            return definition;
        }

        #endregion Internal --to be intercepted
    }
}