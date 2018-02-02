using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.CMS.BuildingBlocks;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Commands
{
    public class SectionCreateOrUpdateCommand : CommandBase<CommandResult<SectionState>>
    {
        public SectionCreateOrUpdateCommand()
        {
        }

        public SectionCreateOrUpdateCommand(string friendlyName, int deviceId, string renderScheme, string identifier, int recordId = 0)
        {
            FriendlyName = friendlyName;
            RecordId = recordId;
            DeviceId = deviceId;
            RenderScheme = renderScheme;
            Identifier = identifier;
        }

        public int RecordId { get; set; }

        [Required]
        [DisplayName("Name")]
        public string FriendlyName { get; set; }

        [Required]
        [DisplayName("Identifier")]
        public string Identifier { get; set; }

        [Required]
        [DisplayName("Device")]
        public int DeviceId { get; set; }

        [Required]
        [DisplayName("Render Scheme")]
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
    }

    public class SectionCreateOrUpdateCommandHandler : AsyncRequestHandler<SectionCreateOrUpdateCommand, CommandResult<SectionState>>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<BorgDbContext> _uow;

        private readonly IMediator _dispatcher;

        public SectionCreateOrUpdateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult<SectionState>> HandleCore(SectionCreateOrUpdateCommand message)
        {
            try
            {
                var isTransient = message.RecordId == 0;
                var repo = _uow.ReadWriteRepo<SectionState>();
                SectionState section;
                if (isTransient)
                {
                    section = new SectionState() { FriendlyName = message.FriendlyName, DeviceId = message.DeviceId, RenderScheme = message.RenderScheme, Identifier = message.Identifier, Id = message.RecordId };
                    await repo.Create(section);
                    await _uow.Save();
                    _logger.Info("Created sction {@section}", section);
                    await _dispatcher.Publish(new SectionRecordStateChanged(section.Id, DmlOperation.Create));
                    return CommandResult<SectionState>.Success(section);
                }

                section = await repo.Get(x => x.Id == message.RecordId);
                if (section == null)
                    return CommandResult<SectionState>.FailureWithEmptyPayload(
                        $"No section found for id {message.RecordId}");
                section.FriendlyName = message.FriendlyName;
                section.DeviceId = message.DeviceId;
                section.Identifier = message.Identifier;
                section.RenderScheme = message.RenderScheme;

                await repo.Update(section);
                await _uow.Save();
                await _dispatcher.Publish(new SectionRecordStateChanged(section.Id, DmlOperation.Update));
                return CommandResult<SectionState>.Success(section);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating new section from {@message} - {exception}", message, ex.ToString());
                return CommandResult<SectionState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}