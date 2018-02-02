using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using Microsoft.Extensions.Logging;
using System;

namespace Borg.Cms.Basic.Lib.Features.Content.Services
{
    public class AssetService : AssetStoreBase<int>
    {
        public AssetService(ILoggerFactory loggerFactory, IAssetDirectoryStrategy<int> assetDirectoryStrategy, IConflictingNamesResolver conflictingNamesResolver, Func<IFileStorage> fileStorageFactory, IAssetStoreDatabaseService<int> assetStoreDatabaseService) : base(loggerFactory, assetDirectoryStrategy, conflictingNamesResolver, fileStorageFactory, assetStoreDatabaseService)
        {
        }
    }

    public class AssetInfoDefinition : AssetInfoDefinition<int>
    {
        public AssetInfoDefinition(int id, string name) : base(id, name)
        {
        }
    }
}