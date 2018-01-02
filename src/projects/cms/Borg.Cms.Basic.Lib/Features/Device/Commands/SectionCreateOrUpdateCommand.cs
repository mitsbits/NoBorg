using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Commands
{
    public class SectionCreateOrUpdateCommand : CommandBase<CommandResult<SectionRecord>>
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
        public string FriendlyName { get; set; }

        public string Identifier { get; set; }
        public int DeviceId { get; set; }
        public string RenderScheme { get; set; } = DeviceRenderScheme.UnSet;
    }

    public class SectionCreateOrUpdateCommandHandler : AsyncRequestHandler<SectionCreateOrUpdateCommand, CommandResult<SectionRecord>>
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

        protected override async Task<CommandResult<SectionRecord>> HandleCore(SectionCreateOrUpdateCommand message)
        {
            try
            {
                var isTransient = message.RecordId == 0;
                var repo = _uow.ReadWriteRepo<SectionRecord>();
                SectionRecord section;
                if (isTransient)
                {
                    section = new SectionRecord() { FriendlyName = message.FriendlyName, DeviceId = message.DeviceId, RenderScheme = message.RenderScheme, Identifier = message.Identifier, Id = message.RecordId };
                    await repo.Create(section);
                    await _uow.Save();
                    _logger.Info("Created sction {@section}", section);
                    await _dispatcher.Publish(new SectionRecordStateChanged(section.Id, DmlOperation.Create));
                    return CommandResult<SectionRecord>.Success(section);
                }

                section = await repo.Get(x => x.Id == message.RecordId);
                if (section == null)
                    return CommandResult<SectionRecord>.FailureWithEmptyPayload(
                        $"No section found for id {message.RecordId}");
                section.FriendlyName = message.FriendlyName;
                section.DeviceId = message.DeviceId;
                section.Identifier = message.Identifier;
                section.RenderScheme = message.RenderScheme;

                await repo.Update(section);
                await _uow.Save();
                await _dispatcher.Publish(new SectionRecordStateChanged(section.Id, DmlOperation.Update));
                return CommandResult<SectionRecord>.Success(section);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating new section from {@message} - {exception}", message, ex.ToString());
                return CommandResult<SectionRecord>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}