using System;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using Microsoft.Extensions.Logging;

namespace Borg.Platform.Documents.Services
{
    public class AssetService : AssetStoreBase<int>
    {
        public AssetService(ILoggerFactory loggerFactory, IAssetDirectoryStrategy<int> assetDirectoryStrategy, IConflictingNamesResolver conflictingNamesResolver, Func<IFileStorage> fileStorageFactory, IAssetStoreDatabaseService<int> assetStoreDatabaseService) : base(loggerFactory, assetDirectoryStrategy, conflictingNamesResolver, fileStorageFactory, assetStoreDatabaseService)
        {
        }
    }


}