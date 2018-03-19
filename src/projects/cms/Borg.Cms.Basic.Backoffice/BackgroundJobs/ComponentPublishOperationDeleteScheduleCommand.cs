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
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Backoffice.BackgroundJobs
{
    public class ComponentPublishOperationDeleteScheduleCommand : CommandBase<CommandResult>
    {
        public int ComponentId { get; set; }
        public int ScheduleId { get; set; }
    }

    public class ComponentPublishOperationDeleteScheduleCommandHandler : AsyncRequestHandler<ComponentPublishOperationDeleteScheduleCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly ISentinel _sentinel;

        public ComponentPublishOperationDeleteScheduleCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, ISentinel sentinel)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _sentinel = sentinel;
        }

        protected override async Task<CommandResult> HandleCore(ComponentPublishOperationDeleteScheduleCommand message)
        {
            try
            {
                await _sentinel.Delete(message.ScheduleId.ToString());
                await _uow.ReadWriteRepo<ComponentJobScheduleState>().Delete(x =>
                    x.ComponentId == message.ComponentId && x.ScheduleId == message.ScheduleId);
                await _uow.Save();
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error deleting publish schedule from {@message} - {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}