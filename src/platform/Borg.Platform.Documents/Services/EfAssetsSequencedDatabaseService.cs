using Borg.Infra;
using Borg.Infra.Collections;
using Borg.Infra.Storage;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using Borg.Platform.Documents.Data;
using Borg.Platform.Documents.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Platform.Documents.Services
{
    public class EfAssetsSequencedDatabaseService : EFAssetsDatabaseService<int>, IAssetStoreDatabaseService<int>
    {
        protected readonly ILogger _logger;
        protected readonly DocumentsDbContext _db;

        public EfAssetsSequencedDatabaseService(ILoggerFactory loggerFactory, DocumentsDbContext db)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
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
            var sts = await _db.AssetRecords.Include(x => x.Versions).ThenInclude(x => x.FileState)
                .AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();

            var assets = sts.Select(x => new AssetInfoDefinition<int>(x.Id, x.Name, x.DocumentBehaviourState)).ToArray();

            foreach (var assetInfoDefinition in assets)
            {
                var r = sts.Single(x => x.Id == assetInfoDefinition.Id);
                assetInfoDefinition.CurrentFile = new VersionInfoDefinition<int>(r.CurrentVersion, r.Versions.Single(v => v.Version == r.CurrentVersion).FileState);
            }
            return new PagedResult<AssetInfoDefinition<int>>(assets, 1, assets.Length, assets.Length);
        }

        public override async Task Create(AssetInfoDefinition<int> asset)
        {
            try
            {
                var assetRecotd = new AssetState() { Id = asset.Id, Name = asset.Name, CurrentVersion = 1 };
                assetRecotd.Versions.Add(new VersionState()
                {
                    Id = await SequnceInternal("assets.VersionsSQC"),
                    AssetRecordId = asset.Id,
                    Version = 1,
                    FileState = new FileState()
                    {
                        Id = asset.CurrentFile.FileSpec.Id,
                        CreationDate = asset.CurrentFile.FileSpec.CreationDate,
                        FullPath = asset.CurrentFile.FileSpec.FullPath,
                        LastRead = asset.CurrentFile.FileSpec.LastRead,
                        LastWrite = asset.CurrentFile.FileSpec.LastWrite,
                        MimeType = asset.CurrentFile.FileSpec.MimeType,
                        Extension = Path.GetExtension(asset.CurrentFile.FileSpec.FullPath),
                        Name = asset.CurrentFile.FileSpec.Name,
                        SizeInBytes = asset.CurrentFile.FileSpec.SizeInBytes
                    }
                });

                await _db.AssetRecords.AddAsync(assetRecotd);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override async Task<VersionInfoDefinition> CheckOut(int id)
        {
            try
            {
                var asset = await _db.AssetRecords.FirstOrDefaultAsync(x => x.Id == id);
                var versions = await _db.VersionRecords.AsNoTracking().Where(x => x.AssetRecordId == id).ToListAsync();
                if (asset.DocumentBehaviourState == DocumentBehaviourState.InProgress)
                {
                    throw new InvalidOperationException($"Asset with id {id} is in Progress");
                }
                var currentVersion = versions.Single(v => v.Version == asset.CurrentVersion);
                //var currentFile = await _db.FileRecords.AsNoTracking().FirstAsync(x => x.Id == currentVersion.FileRecordId);
                var currentFile = await _db.FileRecords.FirstAsync(x => x.Id == currentVersion.FileRecordId);
                //var newFileRecord = new FileRecord()
                //{
                //    CreationDate = currentFile.CreationDate,
                //    FullPath = currentFile.FullPath,
                //    LastRead = currentFile.LastRead,
                //    LastWrite = currentFile.LastWrite,
                //    MimeType = currentFile.MimeType,
                //    Extension = currentFile.Extension,
                //    Name = currentFile.Name,
                //    SizeInBytes = currentFile.SizeInBytes,
                //    Id = await FileNextFromSequence()
                //};
                //_db.FileRecords.Add(newFileRecord);

                var checkoutversion = new VersionState()
                {
                    Version = currentVersion.Version + 1,
                    AssetRecordId = id,
                    FileRecordId = currentFile.Id,
                    //Id = await SequnceInternal("assets.VersionsSQC")
                };
                await _db.VersionRecords.AddAsync(checkoutversion);
                await _db.SaveChangesAsync();
                asset = _db.AssetRecords.Find(id);
                asset.DocumentBehaviourState = DocumentBehaviourState.InProgress;
                await _db.SaveChangesAsync();
                return new VersionInfoDefinition(checkoutversion.Version, new FileSpecDefinition(currentFile.FullPath, currentFile.Name, currentFile.CreationDate, currentFile.LastWrite, currentFile.LastRead, currentFile.SizeInBytes, currentFile.MimeType));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public override async Task<VersionInfoDefinition> CheckIn(int id, FileSpecDefinition<int> fileSpec)
        {
            try
            {
                var asset = await _db.AssetRecords.Include(x => x.Versions).FirstOrDefaultAsync(x => x.Id == id);
                if (asset.DocumentBehaviourState == DocumentBehaviourState.Commited)
                {
                    throw new InvalidOperationException($"Asset with id {id} is in Commited");
                }
                var currentVersion = asset.Versions.Single(v => v.Version == asset.CurrentVersion);
                var checkinVersion = asset.Versions.Single(v => v.Version == asset.CurrentVersion + 1);
                if (currentVersion == null || checkinVersion == null)
                {
                    throw new InvalidOperationException($"{nameof(currentVersion)} - {nameof(checkinVersion)}");
                }
                var currentFile = await _db.FileRecords.AsNoTracking().FirstAsync(x => x.Id == currentVersion.FileRecordId);
                var newFileRecord = new FileState()
                {
                    CreationDate = fileSpec.CreationDate,
                    FullPath = fileSpec.FullPath,
                    LastRead = fileSpec.LastRead,
                    LastWrite = fileSpec.LastWrite,
                    MimeType = fileSpec.MimeType,
                    Extension = Path.GetExtension(fileSpec.FullPath),
                    Name = fileSpec.Name,
                    SizeInBytes = fileSpec.SizeInBytes,
                    Id = fileSpec.Id
                };
                _db.FileRecords.Add(newFileRecord);
                checkinVersion.FileRecordId = newFileRecord.Id;
                asset.CurrentVersion = checkinVersion.Version;
                asset.DocumentBehaviourState = DocumentBehaviourState.Commited;
                _db.Entry(asset).State = EntityState.Modified;
                _db.Entry(checkinVersion).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return new VersionInfoDefinition(checkinVersion.Version, new FileSpecDefinition(newFileRecord.FullPath, newFileRecord.Name, newFileRecord.CreationDate, newFileRecord.LastWrite, newFileRecord.LastRead, newFileRecord.SizeInBytes, newFileRecord.MimeType));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public override async Task<FileSpecDefinition<int>> CurrentFile(int id)
        {
            var q = from f in _db.FileRecords
                    join v in _db.VersionRecords on f.Id equals v.FileRecordId
                    join a in _db.AssetRecords on v.AssetRecordId equals a.Id
                    where f.VersionState.AssetRecordId == id && f.VersionState.Version == a.CurrentVersion
                    select f;

            var file = await q.FirstAsync();

            return new FileSpecDefinition<int>(file.Id, file.FullPath, file.Name, file.CreationDate, file.LastWrite, file.LastRead, file.SizeInBytes, file.MimeType);
        }

        public override async Task<FileSpecDefinition<int>> VersionFile(int id, int version)
        {
            var q = from f in _db.FileRecords
                    join v in _db.VersionRecords on f.Id equals v.FileRecordId
                    join a in _db.AssetRecords on v.AssetRecordId equals a.Id
                    where f.VersionState.AssetRecordId == id && f.VersionState.Version == version
                    select f;

            var file = await q.FirstAsync();

            return new FileSpecDefinition<int>(file.Id, file.FullPath, file.Name, file.CreationDate, file.LastWrite, file.LastRead, file.SizeInBytes, file.MimeType);
        }

        public override async Task RenameAsset(int id, string newName)
        {
            var hit = await _db.AssetRecords.FirstOrDefaultAsync(x => x.Id == id);
            if (hit == null)
            {
                _logger.Warn("Could not find asset with id {id}", id);
            }
            if (hit.Name != newName) hit.Name = newName;
            await _db.SaveChangesAsync();
        }

        public override async Task<AssetInfoDefinition<int>> AddVersion(AssetInfoDefinition<int> hit, FileSpecDefinition<int> fileSpec, VersionInfoDefinition versionSpec)
        {
            var asset = await _db.AssetRecords.Include(x => x.Versions).FirstOrDefaultAsync(x => x.Id == hit.Id);
            asset.Versions.Add(new VersionState()
            {
                Version = versionSpec.Version,
                FileState = new FileState()
                {
                    Id = fileSpec.Id,
                    CreationDate = fileSpec.CreationDate,
                    FullPath = fileSpec.FullPath,
                    LastRead = fileSpec.LastRead,
                    LastWrite = fileSpec.LastWrite,
                    MimeType = fileSpec.MimeType,
                    Extension = Path.GetExtension(fileSpec.FullPath),
                    Name = fileSpec.Name,
                    SizeInBytes = fileSpec.SizeInBytes
                }
            });
            asset.CurrentVersion = versionSpec.Version;
            await _db.SaveChangesAsync();
            return new AssetInfoDefinition<int>(asset.Id, asset.Name)
            {
                CurrentFile = new VersionInfoDefinition<int>(asset.CurrentVersion, fileSpec )
            };
        }

        public override async Task<bool> TryAdd(IMimeTypeSpec mimeType)
        {
            var hit = await _db.MimeTypeRecords.SingleAsync(x => x.Extension.ToLower() == mimeType.Extension.ToLower());
            MimeTypeState newrecord = null;
            if (hit == null)
            {
                newrecord = new MimeTypeState()
                {
                    Extension = mimeType.Extension,
                    MimeType = mimeType.MimeType
                };
                await _db.MimeTypeRecords.AddAsync(newrecord);
            }
            await _db.SaveChangesAsync();
            var added = newrecord != null;
            if (added) LoggerExtensions.Info(_logger, "Added new mimetype to db {@mime}", newrecord);
            return added;
        }

        public override async Task<IEnumerable<IMimeTypeSpec>> MimeTypes()
        {
            var hits = await _db.MimeTypeRecords.AsNoTracking().OrderBy(x => x.Extension).ToArrayAsync();
            return hits.Select(x => new MimeTypeSpec(x.Extension, x.MimeType));
        }

        public override async Task<IEnumerable<IMimeTypeSpec>> MimeTypes(params string[] extensions)
        {
            var q = from m in _db.MimeTypeRecords join extension in extensions on m.Extension equals extension select m;
            return Enumerable.Select((await q.ToArrayAsync()), x => new MimeTypeSpec(x.Extension, x.MimeType));
        }

        public override async Task<IMimeTypeSpec> GetFromExtension(string extension)
        {
            var hit = await _db.MimeTypeRecords.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Extension.ToLower() == extension.ToLower());
            return new MimeTypeSpec(hit.Extension, hit.MimeType);
        }

        public override async Task<IEnumerable<IVersionInfo>> AssetVersions(int assetId)
        {
            var hits = await Queryable.Where(_db.VersionRecords.Include(v => v.FileState).AsNoTracking(), v => v.AssetRecordId == assetId).ToArrayAsync();
            return hits.Select(x => new VersionInfoDefinition(x.Version,
                new FileSpecDefinition(x.FileState.FullPath, x.FileState.Name, x.FileState.CreationDate, x.FileState.LastWrite, x.FileState.LastRead, x.FileState.SizeInBytes, x.FileState.MimeType)));
        }

        public override async Task<IFileSpec<int>> Spec(int fileId)
        {
            var hit = await _db.FileRecords.AsNoTracking().FirstOrDefaultAsync(x => x.Id == fileId);
            if (hit == null)
            {
                _logger.Warn("Could not find file for id {id}", fileId);
                return null;
            }
            return new FileSpecDefinition<int>(hit.Id, hit.FullPath, hit.Name, hit.CreationDate, hit.LastWrite, hit.LastRead, hit.SizeInBytes, hit.MimeType);
        }

        private async Task<int> SequnceInternal(string sqc)
        {
            var conn = _db.Database.GetDbConnection(); //do not dispose connection, it is managed by the context
            using (var command = conn.CreateCommand())
            {
                try
                {
                    command.CommandText = $"SELECT NEXT VALUE FOR {sqc}";
                    if (conn.State == ConnectionState.Closed) await conn.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    return (int)result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}