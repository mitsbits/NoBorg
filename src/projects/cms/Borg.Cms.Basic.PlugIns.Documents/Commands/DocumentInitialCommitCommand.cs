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
    public class DocumentInitialCommitCommand : CommandBase<CommandResult>
    {
        public DocumentInitialCommitCommand(int documentId, string userHandle)
        {
            DocumentId = documentId;
            UserHandle = userHandle;
        }

        public string UserHandle { get; }
        public int DocumentId { get; }
    }

    public class DocumentInitialCommitCommandHandler : AsyncRequestHandler<DocumentInitialCommitCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;
        private readonly IUnitOfWork<DocumentsDbContext> _uow;

        public DocumentInitialCommitCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<DocumentsDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(DocumentInitialCommitCommand message)
        {
            try
            {
                DocumentOwnerAssociationEvent @event = null;
                var clock = DateTimeOffset.UtcNow;
                var document = new DocumentState() { Id = message.DocumentId, IsDeleted = false, IsPublished = false };
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
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error associating user to document from {@message} - {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}