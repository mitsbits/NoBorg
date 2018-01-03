using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Commands
{
    public class DeviceCreateOrUpdateCommand : CommandBase<CommandResult<DeviceRecord>>
    {
        public DeviceCreateOrUpdateCommand()
        {
        }

        public DeviceCreateOrUpdateCommand(string friendlyName, string layout, string deviceRenderScheme, int recordId = 0)
        {
            FriendlyName = friendlyName;
            Layout = layout;
            RecordId = recordId;
            RenderScheme = deviceRenderScheme;
        }

        public int RecordId { get; set; }

        [Required]
        [DisplayName("Name")]
        public string FriendlyName { get; set; }

        [Required]
        [DisplayName("Layout")]
        public string Layout { get; set; }

        [Required]
        [DisplayName("Render Scheme")]
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
    }

    public class DeviceCreateOrUpdateCommandHandler : AsyncRequestHandler<DeviceCreateOrUpdateCommand, CommandResult<DeviceRecord>>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<BorgDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly IDeviceLayoutFileProvider _deviceLayoutFiles;

        public DeviceCreateOrUpdateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow, IMediator dispatcher, IDeviceLayoutFileProvider deviceLayoutFiles)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _deviceLayoutFiles = deviceLayoutFiles;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult<DeviceRecord>> HandleCore(DeviceCreateOrUpdateCommand message)
        {
            try
            {
                var isTransient = message.RecordId == 0;
                var repo = _uow.ReadWriteRepo<DeviceRecord>();
                DeviceRecord device;
                if (isTransient)
                {
                    device = new DeviceRecord() { FriendlyName = message.FriendlyName, Layout = message.Layout , RenderScheme = message.RenderScheme};
                    var file = (await _deviceLayoutFiles.LayoutFiles()).FirstOrDefault(x => x.MatchesPath(message.Layout));
                    foreach (var fileSectionIdentifier in file.SectionIdentifiers)
                    {
                        device.Sections.Add(new SectionRecord() { FriendlyName = fileSectionIdentifier, Identifier = fileSectionIdentifier, RenderScheme = message.RenderScheme });
                    }
                    await repo.Create(device);
                    await _uow.Save();
                    _logger.Info("Created device {@device}", device);
                    await _dispatcher.Publish(new DeviceRecordStateChanged(device.Id, DmlOperation.Create));
                    return CommandResult<DeviceRecord>.Success(device);
                }

                device = await repo.Get(x => x.Id == message.RecordId);
                if (device == null)
                    return CommandResult<DeviceRecord>.FailureWithEmptyPayload(
                        $"No device found for id {message.RecordId}");
                device.FriendlyName = message.FriendlyName;
                device.Layout = message.Layout;
                device.RenderScheme = message.RenderScheme;
                await repo.Update(device);
                await _uow.Save();
                await _dispatcher.Publish(new DeviceRecordStateChanged(device.Id, DmlOperation.Update));
                return CommandResult<DeviceRecord>.Success(device);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating new device from {@message} - {exception}", message, ex.ToString());
                return CommandResult<DeviceRecord>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}