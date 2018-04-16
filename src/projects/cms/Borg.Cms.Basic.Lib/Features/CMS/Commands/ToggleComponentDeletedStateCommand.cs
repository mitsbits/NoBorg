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
    public class ToggleComponentDeletedStateCommand : CommandBase<CommandResult>
    {
        public ToggleComponentDeletedStateCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class ToggleComponentDeletedStateCommandHandler : AsyncRequestHandler<ToggleComponentDeletedStateCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly IMediator _dispatcher;

        public ToggleComponentDeletedStateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
        }

        protected override async Task<CommandResult> HandleCore(ToggleComponentDeletedStateCommand message)
        {
            try
            {
                var repo = _uow.ReadWriteRepo<ComponentState>();
                var comp = await repo.Get(x => x.Id == message.Id);
                if (comp == null) return CommandResult.Failure($"Component with id {message.Id} is not present");
                var previous = comp.IsDeleted;
                var current = !previous;
                if (current)
                {
                    comp.Delete();
                }
                else
                {
                    comp.Recover();
                }
                await repo.Update(comp);
                await _uow.Save();
                _dispatcher.Publish(new ComponentDeletedStateChangedEvent(comp.Id, previous, current));
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating {@component}: {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}