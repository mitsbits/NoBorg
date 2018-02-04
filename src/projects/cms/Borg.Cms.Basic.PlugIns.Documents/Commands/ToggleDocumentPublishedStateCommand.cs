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
    public class ToggleDocumentPublishedStateCommand : CommandBase<CommandResult>
    {
        public ToggleDocumentPublishedStateCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class ToggleDocumentPublishedStateCommandHandler : AsyncRequestHandler<ToggleDocumentPublishedStateCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<DocumentsDbContext> _uow;
        private readonly IMediator _dispatcher;

        public ToggleDocumentPublishedStateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<DocumentsDbContext> uow, IMediator dispatcher)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
        }

        protected override async Task<CommandResult> HandleCore(ToggleDocumentPublishedStateCommand message)
        {
            try
            {
                var repo = _uow.ReadWriteRepo<DocumentState>();
                var comp = await repo.Get(x => x.Id == message.Id);
                if (comp == null) return CommandResult.Failure($"Document with id {message.Id} is not present");
                var previous = comp.IsPublished;
                var current = !previous;
                comp.IsPublished = current;
                await repo.Update(comp);
                await _uow.Save();
                _dispatcher.Publish(new DocumentPublishedStateChangedEvent(comp.Id, previous, current));
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating {@Document}: {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}