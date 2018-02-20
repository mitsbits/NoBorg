using Borg.Cms.Basic.Lib.Features;
using Borg.Cms.Basic.PlugIns.Documents.Events;
using Borg.Infra.DAL;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Commands
{
    public class StoreUserDocumentCommand : CommandBase<CommandResult<(int docid, int fileid)>>
    {
        public StoreUserDocumentCommand(string userHandle, string filename, byte[] file)
        {
            UserHandle = userHandle;
            Filename = filename;
            File = file;
        }

        public byte[] File { get; }
        public string Filename { get; }
        public string UserHandle { get; }
    }

    public class StoreUserDocumentCommandHandler : AsyncRequestHandler<StoreUserDocumentCommand, CommandResult<(int docid, int fileid)>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;
        private readonly IAssetStoreDatabaseService<int> _assetStoreDatabase;

        public StoreUserDocumentCommandHandler(ILoggerFactory loggerFactory, IMediator dispatcher, IAssetStore<AssetInfoDefinition<int>, int> assetStore, IAssetStoreDatabaseService<int> assetStoreDatabase)
        {
            _dispatcher = dispatcher;
            _assetStore = assetStore;
            _assetStoreDatabase = assetStoreDatabase;
            _logger = loggerFactory.CreateLogger(GetType());
            _assetStore.FileCreated += args => _dispatcher.Publish(new FileCreatedEvent(args.RecordId, args.MimeType));
        }

        protected override async Task<CommandResult<(int docid, int fileid)>> HandleCore(StoreUserDocumentCommand message)
        {
            try
            {
                var filename = message.Filename.EnsureCorrectFilenameFromUpload();
                var definition = await _assetStore.Create(Path.GetFileNameWithoutExtension(filename), message.File, filename);
                if (definition == null) return CommandResult<(int docid, int fileid)>.FailureWithEmptyPayload($"Could not create document from {filename}");
                var commandResult = await _dispatcher.Send(new DocumentInitialCommitCommand(definition.Id, message.UserHandle));
                if (!commandResult.Succeded) return CommandResult<(int docid, int fileid)>.FailureWithEmptyPayload(commandResult.Errors);
                var currentspec = await _assetStoreDatabase.CurrentFile(definition.Id);
                return CommandResult<(int docid, int fileid)>.Success((docid: definition.Id, fileid: currentspec.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating document from {@message} - {exception}", message, ex.ToString());
                return CommandResult<(int docid, int fileid)>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}