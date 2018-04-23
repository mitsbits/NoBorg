using Borg.CMS.Documents.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Microsoft.Extensions.Logging.Abstractions;
using Borg.Infra;
using System.IO;
using Borg.Platform.Documents.Data;

using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Platform.Documents.Services
{
    public class DocumentsService : IDocumentsService<int>
    {
        private readonly ILogger _logger;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;
        private readonly IAssetStoreDatabaseService<int> _assetStoreDatabase;
        private readonly IUnitOfWork<DocumentsDbContext> _uow;

        public DocumentsService(ILoggerFactory loggerFactory, IAssetStore<AssetInfoDefinition<int>, int>  assetStore, IAssetStoreDatabaseService<int> assetStoreDatabase, IUnitOfWork<DocumentsDbContext> uow)
        {
            _logger =(loggerFactory == null)? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            Preconditions.NotNull(assetStore, nameof(assetStore));
            Preconditions.NotNull(assetStoreDatabase, nameof(assetStoreDatabase));
            Preconditions.NotNull(uow, nameof(uow));
            _assetStore = assetStore;
            _assetStoreDatabase = assetStoreDatabase;
            _uow = uow;
        }

        public async Task<(int docid, int fileid)> StoreUserDocument(byte[] data, string filename, string userHandle)
        {
            var definition = await _assetStore.Create(Path.GetFileNameWithoutExtension(filename),  data, filename);
            if (definition == null)  throw  new ApplicationException($"Could not create document from {filename}");

       
            var clock = DateTimeOffset.UtcNow;
            var document = new DocumentState() { IsDeleted = false, IsPublished = false };
            await _uow.ReadWriteRepo<DocumentState>().Create(document);
            var association = new DocumentOwnerState() { AssociatedOn = clock, DocumentId = message.DocumentId, Owner = message.UserHandle };
            await _uow.ReadWriteRepo<DocumentOwnerState>().Create(association);
            var checkout = new DocumentCheckOutState() { DocumentId = message.DocumentId, CheckOutVersion = 1, CheckedIn = true, CheckedInBy = message.UserHandle, CheckedOutBy = message.UserHandle, CheckedOutOn = clock, CheckedinOn = clock };
            await _uow.ReadWriteRepo<DocumentCheckOutState>().Create(checkout);
            await _uow.Save();
            @event = new DocumentOwnerAssociationEvent(message.DocumentId, message.UserHandle, DocumentOwnerAssociationOperation.AddToUserCollection);
            if (@event != null) _dispatcher.Publish(@event); //fire and forget
            return CommandResult.Success();


        }
    }
}