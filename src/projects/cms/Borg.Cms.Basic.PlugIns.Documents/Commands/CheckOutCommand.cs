using Borg.Cms.Basic.Lib.Features;
using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Cms.Basic.PlugIns.Documents.Events;
using Borg.Infra.DAL;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Commands
{
    public class CheckOutCommand : CommandBase<CommandResult>
    {
        public CheckOutCommand(int documentId, string userHandle)
        {
            DocumentId = documentId;
            UserHandle = userHandle;
        }

        public int DocumentId { get; }

        public string UserHandle { get; }
    }

    public class CheckOutCommandHandler : AsyncRequestHandler<CheckOutCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;
        private readonly IUnitOfWork<DocumentsDbContext> _uow;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;

        public CheckOutCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<DocumentsDbContext> uow, IMediator dispatcher, IAssetStore<AssetInfoDefinition<int>, int> assetStore)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _assetStore = assetStore;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(CheckOutCommand message)
        {
            try
            {
                DocumenCheckOutEvent @event = null;

                var checkoutversion = await _assetStore.CheckOut(message.DocumentId);
                if (checkoutversion != null)
                {
                    var doccheckout = new DocumentCheckOutState()
                    {
                        DocumentId = message.DocumentId,
                        CheckOutVersion = checkoutversion.Version,
                        CheckedIn = false,
                        CheckedOutBy = message.UserHandle,
                        CheckedOutOn = DateTimeOffset.UtcNow
                    };
                    await _uow.ReadWriteRepo<DocumentCheckOutState>().Create(doccheckout);
                    @event = new DocumenCheckOutEvent(message.DocumentId, message.UserHandle, checkoutversion.Version);
                }

                await _uow.Save();
                if (@event != null) _dispatcher.Publish(@event); //fire and forget
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error checking out document to user from {@message} - {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}