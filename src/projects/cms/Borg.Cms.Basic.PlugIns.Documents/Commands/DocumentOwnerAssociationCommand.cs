using Borg.Cms.Basic.Lib.Features;
using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Cms.Basic.PlugIns.Documents.Events;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Commands
{
    public class DocumentOwnerAssociationCommand : CommandBase<CommandResult>
    {
        public DocumentOwnerAssociationCommand(int documentId, string userHandle, DocumentOwnerAssociationOperation associationOperation)
        {
            DocumentId = documentId;
            UserHandle = userHandle;
            AssociationOperation = associationOperation;
        }

        public DocumentOwnerAssociationOperation AssociationOperation { get; }
        public string UserHandle { get; }
        public int DocumentId { get; }
    }

    public class DocumentOwnerAssociationCommandHandler : AsyncRequestHandler<DocumentOwnerAssociationCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;
        private readonly IUnitOfWork<DocumentsDbContext> _uow;

        public DocumentOwnerAssociationCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<DocumentsDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(DocumentOwnerAssociationCommand message)
        {
            try
            {
                DocumentOwnerAssociationEvent @event = null;
                switch (message.AssociationOperation)
                {
                    case DocumentOwnerAssociationOperation.AddToUserCollection:
                        if (!await _uow.QueryRepo<DocumentOwnerState>().Exists(x =>
                            x.DocumentId == message.DocumentId && x.Owner == message.UserHandle))
                        {
                            if (!await _uow.QueryRepo<DocumentState>().Exists(x => x.Id == message.DocumentId))
                            {
                                await _uow.ReadWriteRepo<DocumentState>()
                                    .Create(new DocumentState() { Id = message.DocumentId });
                            }
                            await _uow.ReadWriteRepo<DocumentOwnerState>().Create(new DocumentOwnerState()
                            {
                                Owner = message.UserHandle,
                                DocumentId = message.DocumentId
                            });
                            @event = new DocumentOwnerAssociationEvent(message.DocumentId, message.UserHandle, message.AssociationOperation);
                        }
                        break;

                    case DocumentOwnerAssociationOperation.RemoveFromUserCollection:
                        if (await _uow.QueryRepo<DocumentOwnerState>().Exists(x =>
                            x.DocumentId == message.DocumentId && x.Owner == message.UserHandle))
                        {
                            await _uow.ReadWriteRepo<DocumentOwnerState>()
                                .Delete(x => x.DocumentId == message.DocumentId);
                            @event = new DocumentOwnerAssociationEvent(message.DocumentId, message.UserHandle, message.AssociationOperation);
                        }
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(message.AssociationOperation));
                }
                await _uow.Save();
                if (@event != null) _dispatcher.Publish(@event); //fire and forget
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error associating user to document from {@message} - {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}