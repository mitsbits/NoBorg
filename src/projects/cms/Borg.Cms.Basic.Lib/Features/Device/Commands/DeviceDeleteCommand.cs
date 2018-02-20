using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Commands
{
    public class DeviceDeleteCommand : CommandBase<CommandResult>
    {
        public DeviceDeleteCommand()
        {
        }

        public DeviceDeleteCommand(int recordId)
        {
            RecordId = recordId;
        }

        [Required]
        public int RecordId { get; set; }
    }

    public class DeviceDeleteCommandHandler : AsyncRequestHandler<DeviceDeleteCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        public DeviceDeleteCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(DeviceDeleteCommand message)
        {
            try
            {
                var repo = _uow.ReadWriteRepo<DeviceState>();
                var sections = await _uow.QueryRepo<SectionState>().Find(x => x.DeviceId == message.RecordId, null);
                foreach (var section in sections)
                {
                    await _dispatcher.Send(new SectionDeleteCommand(section.Id));
                }
                await repo.Delete(x => x.Id == message.RecordId);
                await _uow.Save();
                await _dispatcher.Publish(new DeviceRecordStateChanged(message.RecordId, DmlOperation.Delete));
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error deleting device from {@message} - {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}