using Borg.Infra;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Services
{
    public class StaticImageCacheStore : IStaticImageCacheStore<int>
    {
        private readonly ILogger _logger;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;
        private readonly Func<IFileStorage> _storageFactory;
        private readonly IAssetDirectoryStrategy<int> _assetDirectoryStrategy;
        private readonly BorgSettings _settings;
        private readonly IImageResizer _resizer;

        public StaticImageCacheStore(ILoggerFactory loggerFactory, IAssetStore<AssetInfoDefinition<int>, int> assetStore, Func<IFileStorage> storageFactory, IAssetDirectoryStrategy<int> assetDirectoryStrategy, BorgSettings settings, IImageResizer resizer)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _assetStore = assetStore;
            _storageFactory = storageFactory;
            _assetDirectoryStrategy = assetDirectoryStrategy;
            _settings = settings;
            _resizer = resizer;
        }

        public async Task<IEnumerable<IFileSpec>> PrepareSizes(int fileId)
        {
            var file = await _assetStore.FileSpec(fileId);

            var result = new List<IFileSpec>();

            switch (file.spec.Extension.ToLower())
            {
                case ".jpg":
                case ".png":
                case ".bmp":
                case ".gif":
                    result.AddRange(await PrepStaticFiles(fileId, file));
                    break;

                default:
                    _logger.Warn($"Can not procees image of type {file.spec.MimeType} - file: {file.spec.Name}");
                    break;
            }

            return result;
        }

        private async Task<IEnumerable<IFileSpec>> PrepStaticFiles(int fileId, (Stream file, IFileSpec<int> spec) file)
        {
            var bucket = new List<IFileSpec>();
            using (file.file)
            {
                foreach (var v in VisualSize.GetMembers().Where(x => x.ToString() != VisualSize.Undefined.ToString()))
                {
                    var pixels = _settings.Visual.SizePixels[v.ToString()];
                    using (var local = new MemoryStream())
                    {
                        file.file.Seek(0, 0);
                        await file.file.CopyToAsync(local);
                        var output = await _resizer.ResizeByLargeSide(local, pixels, file.spec.MimeType);
                        output.Seek(0, 0);
                        var parentDirectory = await _assetDirectoryStrategy.ParentFolder(fileId);

                        using (var storage = _storageFactory.Invoke())
                        using (var scoped = storage.Scope(parentDirectory))
                        {
                            var fileName = $"{fileId}_{v}.jpg";
                            if (await scoped.SaveFile(fileName, output))
                            {
                                var spec = await scoped.GetFileInfo(fileName);
                                bucket.Add(spec);
                            }
                        }
                    }
                }
            }
            return bucket;
        }

        public async Task<Uri> PublicUrl(int fileId, VisualSize size)
        {
            var domain = _settings.Storage.ImagesCacheEndpoint;
            var parentDirectory = await _assetDirectoryStrategy.ParentFolder(fileId);
            var fileName = $"{fileId}_{size}.jpg";
            return new Uri(Path.Combine(domain, _settings.Storage.ImagesCacheFolder, parentDirectory, fileName));
        }
    }
}