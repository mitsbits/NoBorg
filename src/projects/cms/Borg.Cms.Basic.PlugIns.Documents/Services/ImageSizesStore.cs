using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Borg.Infra;

namespace Borg.Cms.Basic.PlugIns.Documents.Services
{
    public class ImageSizesStore : IImageSizesStore<int>
    {
        private readonly ILogger _logger;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;
        private readonly Func<IFileStorage> _storageFactory;
        private readonly IAssetDirectoryStrategy<int> _assetDirectoryStrategy;
        private readonly BorgSettings _settings;

        public ImageSizesStore(ILoggerFactory loggerFactory, IAssetStore<AssetInfoDefinition<int>, int> assetStore, Func<IFileStorage> storageFactory, IAssetDirectoryStrategy<int> assetDirectoryStrategy, BorgSettings settings)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _assetStore = assetStore;
            _storageFactory = storageFactory;
            _assetDirectoryStrategy = assetDirectoryStrategy;
            _settings = settings;
        }

        public async Task<IEnumerable<IFileSpec>> PrepareSizes(int fileId)
        {
            var file = await _assetStore.FileSpec(fileId);
            if (file.spec.Extension.ToLower() != ".jpg")
                throw new InvalidOperationException(nameof(IFileSpec.MimeType));
            return null;
        }

        public async Task<Uri> PublicUrl(int fileId, VisualSize size)
        {
            throw new NotImplementedException();
        }
    }
}