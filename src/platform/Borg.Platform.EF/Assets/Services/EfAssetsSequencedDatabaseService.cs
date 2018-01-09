using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra;
using Borg.Infra.Collections;
using Borg.Infra.Storage;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using Borg.Platform.EF.Assets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Borg.Platform.EF.Assets.Services
{
    public class EfAssetsSequencedDatabaseService : EFAssetsDatabaseService<int>
    {

        protected readonly ILogger _logger;
        protected readonly AssetsDbContext _db;

        public EfAssetsSequencedDatabaseService(ILoggerFactory loggerFactory, AssetsDbContext db)
        {
            _logger = (loggerFactory == null) ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            Preconditions.NotNull(db, nameof(db));
            _db = db;

        }
        public override async Task<int> AssetNextFromSequence()
        {
            return await SequnceInternal("assets.AssetsSQC");
        }

        public override async Task<int> FileNextFromSequence()
        {
            return await SequnceInternal("assets.FilesSQC");
        }

        public override async Task<IPagedResult<AssetInfoDefinition<int>>> Find(IEnumerable<int> ids)
        {
            var versions = await _db.VersionRecords
                .Include(x => x.AssetRecord).Include(x => x.FileRecord)
                .AsNoTracking()
                .Where(x => ids.Contains(x.AssetRecordId))
                .ToListAsync();

            var assets = versions.Select(version =>
                new AssetInfoDefinition<int>(version.AssetRecordId, version.AssetRecord.Name)
                {
                    CurrentFile = new VersionInfoDefinition(version.Version,
                        new FileSpecDefinition<int>(version.FileRecordId, version.FileRecord.FullPath,
                            version.FileRecord.Name, version.FileRecord.CreationDate, version.FileRecord.LastWrite,
                            version.FileRecord.LastRead, version.FileRecord.SizeInBytes, version.FileRecord.MimeType))
                }).ToArray();

            return new PagedResult<AssetInfoDefinition<int>>(assets, 1, assets.Length, assets.Length);
        }

        public override async Task Create(AssetInfoDefinition<int> asset)
        {
            var assetRecotd = new AssetRecord() { Id = asset.Id, Name = asset.Name, CurrentVersion = 1 };
            assetRecotd.Versions.Add(new VersionRecord()
            {
                Id = await SequnceInternal("assets.VersionsSQC"),
                AssetRecordId = asset.Id,
                Version = 1,
                FileRecord = new FileRecord()
                {
                    Id = ((IFileSpec<int>)asset.CurrentFile.FileSpec).Id,
                    CreationDate = asset.CurrentFile.FileSpec.CreationDate,
                    FullPath = asset.CurrentFile.FileSpec.FullPath,
                    LastRead = asset.CurrentFile.FileSpec.LastRead,
                    LastWrite = asset.CurrentFile.FileSpec.LastWrite,
                    MimeType = asset.CurrentFile.FileSpec.MimeType,
                    Name = asset.CurrentFile.FileSpec.Name,
                    SizeInBytes = asset.CurrentFile.FileSpec.SizeInBytes
                }
            });
        }

        public override async Task<AssetInfoDefinition<int>> AddVersion(AssetInfoDefinition<int> hit, FileSpecDefinition<int> fileSpec, VersionInfoDefinition versionSpec)
        {
            var asset = await _db.AssetRecords.Include(x => x.Versions).FirstOrDefaultAsync(x=>x.Id == hit.Id);
            asset.Versions.Add(new VersionRecord()
            {
                Version = versionSpec.Version,
                FileRecord = new FileRecord()
                {
                    Id = fileSpec.Id,
                    CreationDate = fileSpec.CreationDate,
                    FullPath = fileSpec.FullPath,
                    LastRead = fileSpec.LastRead,
                    LastWrite = fileSpec.LastWrite,
                    MimeType = fileSpec.MimeType,
                    Name = fileSpec.Name,
                    SizeInBytes = fileSpec.SizeInBytes
                }
            });
            asset.CurrentVersion = versionSpec.Version;
            await _db.SaveChangesAsync();
            return new AssetInfoDefinition<int>(asset.Id, asset.Name)
            {
                CurrentFile = new VersionInfoDefinition(versionSpec.Version, fileSpec)
            };
        }


        private async Task<int> SequnceInternal(string sqc)
        {
            using (var conn = _db.Database.GetDbConnection())
            using (var command = conn.CreateCommand())
            {
                command.CommandText = $"SELECT NEXT VALUE FOR {sqc}";
                if (conn.State == ConnectionState.Closed) await conn.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                return (int)result;
            }
        }
    }
}