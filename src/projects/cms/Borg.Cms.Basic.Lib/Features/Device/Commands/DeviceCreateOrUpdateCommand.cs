﻿using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.CMS.BuildingBlocks;
using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Commands
{
    public class DeviceCreateOrUpdateCommand : CommandBase<CommandResult<DeviceState>>
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

        [DisplayName("Theme")]
        public string Theme { get; set; }

        [Required]
        [DisplayName("Render Scheme")]
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
    }

    public class DeviceCreateOrUpdateCommandHandler : AsyncRequestHandler<DeviceCreateOrUpdateCommand, CommandResult<DeviceState>>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly IDeviceLayoutFileProvider _deviceLayoutFiles;

        public DeviceCreateOrUpdateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, IDeviceLayoutFileProvider deviceLayoutFiles)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _deviceLayoutFiles = deviceLayoutFiles;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult<DeviceState>> HandleCore(DeviceCreateOrUpdateCommand message)
        {
            try
            {
                var isTransient = message.RecordId == 0;
                var repo = _uow.ReadWriteRepo<DeviceState>();
                DeviceState device;
                if (isTransient)
                {
                    device = new DeviceState() { FriendlyName = message.FriendlyName, Layout = message.Layout, RenderScheme = message.RenderScheme, Theme = message.Theme };
                    var file = (await _deviceLayoutFiles.LayoutFiles()).FirstOrDefault(x => x.MatchesPath(message.Layout));
                    foreach (var fileSectionIdentifier in file.SectionIdentifiers)
                    {
                        device.Sections.Add(new SectionState() { FriendlyName = fileSectionIdentifier, Identifier = fileSectionIdentifier, RenderScheme = message.RenderScheme });
                    }
                    await repo.Create(device);
                    await _uow.Save();
                    _logger.Info("Created device {@device}", device);
                    await _dispatcher.Publish(new DeviceRecordStateChanged(device.Id, DmlOperation.Create));
                    return CommandResult<DeviceState>.Success(device);
                }

                device = await repo.Get(x => x.Id == message.RecordId);
                if (device == null)
                    return CommandResult<DeviceState>.FailureWithEmptyPayload(
                        $"No device found for id {message.RecordId}");
                device.FriendlyName = message.FriendlyName;
                device.Layout = message.Layout;
                device.RenderScheme = message.RenderScheme;
                device.Theme = message.Theme;
                await repo.Update(device);
                await _uow.Save();
                await _dispatcher.Publish(new DeviceRecordStateChanged(device.Id, DmlOperation.Update));
                return CommandResult<DeviceState>.Success(device);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating new device from {@message} - {exception}", message, ex.ToString());
                return CommandResult<DeviceState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}