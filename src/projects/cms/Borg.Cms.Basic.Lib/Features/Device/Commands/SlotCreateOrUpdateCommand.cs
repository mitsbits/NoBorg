using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Commands
{
    public class SlotCreateOrUpdateCommand : CommandBase<CommandResult<SlotState>>
    {
        public SlotCreateOrUpdateCommand()
        {
        }

        public SlotCreateOrUpdateCommand(bool isEnabled, int ordinal, int sectionId, string moduleDecriptorJson, string moduleTypeName, string moduleGender, int recordId = 0)
        {
            IsEnabled = isEnabled;
            Ordinal = ordinal;
            SectionId = sectionId;
            ModuleDecriptorJson = moduleDecriptorJson;
            ModuleTypeName = moduleTypeName;
            ModuleGender = moduleGender;
            RecordId = recordId;
        }

        [Required]
        public int RecordId { get; set; }

        [Required]
        [DisplayName("Ordinal")]
        public int Ordinal { get; set; }

        [Required]
        [DisplayName("Enabled")]
        public bool IsEnabled { get; set; }

        [Required]
        public int SectionId { get; set; }

        [Required]
        [DisplayName("Module Type")]
        public string ModuleGender { get; set; }

        [Required]
        [DisplayName("Module Descriptor")]
        public string ModuleTypeName { get; set; }

        [Required]
        [DisplayName("Module Configuration")]
        public string ModuleDecriptorJson { get; set; }
    }

    public class SlotCreateOrUpdateCommandHandler : AsyncRequestHandler<SlotCreateOrUpdateCommand, CommandResult<SlotState>>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        public SlotCreateOrUpdateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult<SlotState>> HandleCore(SlotCreateOrUpdateCommand message)
        {
            try
            {
                var isTransient = message.RecordId == 0;
                var repo = _uow.ReadWriteRepo<SlotState>();
                SlotState slot;
                if (isTransient)
                {
                    slot = new SlotState()
                    {
                        IsEnabled = message.IsEnabled,
                        Ordinal = message.Ordinal,
                        SectionId = message.SectionId,
                        ModuleDecriptorJson = message.ModuleDecriptorJson,
                        Id = message.RecordId,
                        ModuleGender = message.ModuleGender,
                        ModuleTypeName = message.ModuleTypeName
                    };
                    await repo.Create(slot);
                    await _uow.Save();
                    _logger.Info("Created slot {@slot}", slot);
                    await _dispatcher.Publish(new SlotRecordStateChanged(slot.Id, DmlOperation.Create));
                    return CommandResult<SlotState>.Success(slot);
                }

                slot = await repo.Get(x => x.Id == message.RecordId);
                if (slot == null)
                    return CommandResult<SlotState>.FailureWithEmptyPayload(
                        $"No slot found for id {message.RecordId}");
                slot.SectionId = message.SectionId;
                slot.IsEnabled = message.IsEnabled;
                slot.Ordinal = message.Ordinal;
                slot.ModuleDecriptorJson = message.ModuleDecriptorJson;
                slot.ModuleGender = message.ModuleGender;
                slot.ModuleTypeName = message.ModuleTypeName;

                await repo.Update(slot);
                await _uow.Save();
                await _dispatcher.Publish(new SlotRecordStateChanged(slot.Id, DmlOperation.Update));
                slot = await _uow.QueryRepo<SlotState>()
                    .Get(x => x.Id == slot.Id, CancellationToken.None, record => record.Section);
                return CommandResult<SlotState>.Success(slot);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating new section from {@message} - {exception}", message, ex.ToString());
                return CommandResult<SlotState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}