using Borg;
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
    public class ComponentDeviceCommand : CommandBase<CommandResult>
    {
        public ComponentDeviceCommand(int componentId, int deviceId)
        {
            ComponentId = componentId;
            DeviceId = deviceId;
        }

        public int ComponentId { get; }
        public int DeviceId { get; }
    }

    public class ComponentDeviceCommandHandler : AsyncRequestHandler<ComponentDeviceCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        public ComponentDeviceCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(ComponentDeviceCommand message)
        {
            try
            {
                await _uow.ReadWriteRepo<ComponentDeviceState>().Delete(x => x.ComponentId == message.ComponentId);
                if (await _uow.QueryRepo<DeviceState>().Exists(x => x.Id == message.DeviceId))
                {
                    await _uow.ReadWriteRepo<ComponentDeviceState>().Create(
                        new ComponentDeviceState() { ComponentId = message.ComponentId, DeviceId = message.DeviceId });
                }
                await _uow.Save();
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating/updating html snippet from {@message} - {exception}", message, ex.ToString());
                return CommandResult<HtmlSnippetState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}