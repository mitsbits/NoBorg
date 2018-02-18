using Borg.Cms.Basic.Lib.Features;
using Borg.Cms.Basic.PlugIns.Documents.Events;
using Borg.Infra.DAL;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Commands
{
    public class RenameAssetCommand : CommandBase<CommandResult>
    {
        public RenameAssetCommand()
        {
        }

        public RenameAssetCommand(int documentId, string newName)
        {
            DocumentId = documentId;
            NewName = newName;
        }

        public int DocumentId { get; set; }
        public string NewName { get; set; }
    }

    public class RenameAssetCommandHandler : AsyncRequestHandler<RenameAssetCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;

        public RenameAssetCommandHandler(ILoggerFactory loggerFactory, IMediator dispatcher, IAssetStore<AssetInfoDefinition<int>, int> assetStore)
        {
            _dispatcher = dispatcher;
            _assetStore = assetStore;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(RenameAssetCommand message)
        {
            try
            {
                AssetRenamedEvent @event = null;

                await _assetStore.RenameAsset(message.DocumentId, message.NewName);
                @event = new AssetRenamedEvent(message.DocumentId, message.NewName);
                if (@event != null) _dispatcher.Publish(@event); //fire and forget
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error renaming document from {@message} - {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}