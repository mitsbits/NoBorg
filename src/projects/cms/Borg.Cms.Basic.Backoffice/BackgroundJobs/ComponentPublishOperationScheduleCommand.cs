using Borg.Cms.Basic.Lib.Features;
using Borg.Cms.Basic.Lib.System;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.BackgroundJobs
{
    public class ComponentPublishOperationScheduleCommand : CommandBase<CommandResult>
    {
        public int ComponentId { get; set; }
        [UIHint("DateTimeOffsetUTC")]
        public DateTimeOffset TriggerDateUTC { get; set; }
        public ComponentPublishOperation.OperationDirection Direction { get; set; }
    }

    public class ComponentPublishOperationScheduleCommandHandler : AsyncRequestHandler<ComponentPublishOperationScheduleCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly ISentinel _sentinel;

        public ComponentPublishOperationScheduleCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, ISentinel sentinel)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
            _sentinel = sentinel;
        }

        protected override async Task<CommandResult> HandleCore(ComponentPublishOperationScheduleCommand message)
        {
            try
            {
                ComponentPublishOperationScheduleAddedEvent @event = null;
                var jid = await _sentinel.Schedule<ComponentPublishStateJob>(message.TriggerDateUTC,
                    new ComponentPublishOperation(message.ComponentId, message.Direction).JobArgs());
                await _uow.ReadWriteRepo<ComponentJobScheduleState>().Create(new ComponentJobScheduleState()
                {
                    ComponentId = message.ComponentId,
                    ScheduleId = int.Parse(jid)
                });
                await _uow.Save();
                @event = new ComponentPublishOperationScheduleAddedEvent(message.ComponentId, jid );
                _dispatcher.Publish(@event);
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error scheduling publish change for component from {@message} - {exception}", message, ex.ToString());
                return CommandResult<HtmlSnippetState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}