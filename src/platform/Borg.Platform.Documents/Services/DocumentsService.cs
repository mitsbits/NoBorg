using Borg.Infra;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using Borg.Infra.Storage.Documents;
using Borg.Platform.Documents.Data;
using Borg.Platform.Documents.Data.Entities;
using Borg.Platform.EF.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Borg.Platform.Documents.Services
{
    public class DocumentsService : IDocumentsService<int>
    {
        private readonly ILogger _logger;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;
        private readonly IAssetStoreDatabaseService<int> _assetStoreDatabase;
        private readonly IUnitOfWork<DocumentsDbContext> _uow;

        public DocumentsService(ILoggerFactory loggerFactory, IAssetStore<AssetInfoDefinition<int>, int> assetStore, IAssetStoreDatabaseService<int> assetStoreDatabase, IUnitOfWork<DocumentsDbContext> uow)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            Preconditions.NotNull(assetStore, nameof(assetStore));
            Preconditions.NotNull(assetStoreDatabase, nameof(assetStoreDatabase));
            Preconditions.NotNull(uow, nameof(uow));
            _assetStore = assetStore;
            _assetStoreDatabase = assetStoreDatabase;
            _uow = uow;
        }

        public event FileCreatedEventHandler<int> FileCreated;

        public async Task<(int docid, IFileSpec<int> file)> StoreUserDocument(byte[] data, string filename, string userHandle)
        {
            try
            {
                var definition = await _assetStore.Create(Path.GetFileNameWithoutExtension(filename), data, filename);
                if (definition == null) throw new ApplicationException($"Could not create document from {filename}");

                var clock = DateTimeOffset.UtcNow;
                var document = new DocumentState() { IsDeleted = false, IsPublished = false, Id = definition.Id };
                await _uow.ReadWriteRepo<DocumentState>().Create(document);
                var association = new DocumentOwnerState
                {
                    AssociatedOn = clock,
                    Owner = userHandle,
                    Document = document
                };
                await _uow.ReadWriteRepo<DocumentOwnerState>().Create(association);
                var checkout = new DocumentCheckOutState
                {
                    CheckOutVersion = 1,
                    CheckedIn = true,
                    CheckedInBy = userHandle,
                    CheckedOutBy = userHandle,
                    CheckedOutOn = clock,
                    CheckedinOn = clock,
                    Document = document
                };
                await _uow.ReadWriteRepo<DocumentCheckOutState>().Create(checkout);
                await _uow.Save();
                var filespec = definition.CurrentFile.FileSpec.Clone();
                var result = (docid: definition.Id, file: filespec);
                OnFileCreated(filespec);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected virtual Task OnFileCreated(IFileSpec<int> file)
        {
            var handler = FileCreated;
            return handler?.Invoke(file);
        }
    }
}