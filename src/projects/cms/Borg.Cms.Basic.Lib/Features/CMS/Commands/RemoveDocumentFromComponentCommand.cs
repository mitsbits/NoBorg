using Borg.Cms.Basic.Lib.Features.CMS.Events;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{
    public class RemoveDocumentFromComponentCommand : CommandBase<CommandResult>
    {
        public int DocumentId { get; set; }
        public int ComponentId { get; set; }
    }

    public class RemoveDocumentFromComponentCommandHandler : AsyncRequestHandler<RemoveDocumentFromComponentCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        public RemoveDocumentFromComponentCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
        }

        protected override async Task<CommandResult> HandleCore(RemoveDocumentFromComponentCommand message)
        {
            try
            {
                ComponentDocumentDisassociationEvent @event = null;
                await _uow.ReadWriteRepo<ComponentDocumentAssociationState>().Delete(x =>
                    x.DocumentId == message.DocumentId && x.ComponentId == message.ComponentId);
                await _uow.Save();
                @event = new ComponentDocumentDisassociationEvent(message.ComponentId, message.DocumentId);
                _dispatcher.Publish(@event);
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error disassociating document for article from {@message} - {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}