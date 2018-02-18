using Borg.Cms.Basic.Lib.Features;
using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Cms.Basic.PlugIns.Documents.Events;
using Borg.Infra.DAL;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Commands
{
    public class CheckInCommand : CommandBase<CommandResult>
    {
        public CheckInCommand()
        {
        }

        public CheckInCommand(int documentId, string userHandle)
        {
            DocumentId = documentId;
            UserHandle = userHandle;
        }

        public int DocumentId { get; set; }
        public string UserHandle { get; set; }
        public IFormFile File { get; set; }
    }

    public class CheckInCommandHandler : AsyncRequestHandler<CheckInCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;
        private readonly IUnitOfWork<DocumentsDbContext> _uow;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;

        public CheckInCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<DocumentsDbContext> uow, IMediator dispatcher, IAssetStore<AssetInfoDefinition<int>, int> assetStore)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _assetStore = assetStore;
            _logger = loggerFactory.CreateLogger(GetType());
            _assetStore.FileCreated += (args) => _dispatcher.Publish(new FileCreatedEvent(args.RecordId, args.MimeType));
        }

        protected override async Task<CommandResult> HandleCore(CheckInCommand message)
        {
            try
            {
                DocumenCheckInEvent @event = null;

                var checkout =
                    await _uow.Context.DocumentCheckOutStates.SingleAsync(x =>
                        x.DocumentId == message.DocumentId && !x.CheckedIn);

                if (checkout == null) return CommandResult.Failure($"There is no pennding version for document with id {message.DocumentId}");

                using (var stream = new MemoryStream())
                {
                    await message.File.CopyToAsync(stream, CancellationToken.None);
                    stream.Seek(0, 0);
                    var CheckInversion = await _assetStore.CheckIn(message.DocumentId, stream.ToArray(), EnsureCorrectFilename(message.File.FileName));
                    checkout.CheckedIn = true;
                    checkout.CheckedInBy = message.UserHandle;
                    checkout.CheckedinOn = DateTimeOffset.UtcNow;
                    await _uow.ReadWriteRepo<DocumentCheckOutState>().Update(checkout);

                    @event = new DocumenCheckInEvent(message.DocumentId, message.UserHandle, CheckInversion.Version);
                }

                await _uow.Save();
                if (@event != null) _dispatcher.Publish(@event); //fire and forget
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error checking in document to user from {@message} - {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }
    }
}