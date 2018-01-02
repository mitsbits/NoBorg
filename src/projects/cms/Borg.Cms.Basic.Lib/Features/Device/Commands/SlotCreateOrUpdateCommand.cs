using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Commands
{
    public class SlotCreateOrUpdateCommand : CommandBase<CommandResult<SlotRecord>>
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
        public int Ordinal { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        [Required]
        public int SectionId { get; set; }

        [Required]
        public string ModuleGender { get; set; }

        [Required]
        public string ModuleTypeName { get; set; }

        public string ModuleDecriptorJson { get; set; }
    }

    public class SlotCreateOrUpdateCommandHandler : AsyncRequestHandler<SlotCreateOrUpdateCommand, CommandResult<SlotRecord>>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<BorgDbContext> _uow;

        private readonly IMediator _dispatcher;

        public SlotCreateOrUpdateCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult<SlotRecord>> HandleCore(SlotCreateOrUpdateCommand message)
        {
            try
            {
                var isTransient = message.RecordId == 0;
                var repo = _uow.ReadWriteRepo<SlotRecord>();
                SlotRecord slot;
                if (isTransient)
                {
                    slot = new SlotRecord()
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
                    return CommandResult<SlotRecord>.Success(slot);
                }

                slot = await repo.Get(x => x.Id == message.RecordId);
                if (slot == null)
                    return CommandResult<SlotRecord>.FailureWithEmptyPayload(
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
                slot = await _uow.QueryRepo<SlotRecord>()
                    .Get(x => x.Id == slot.Id, CancellationToken.None, record => record.Section);
                return CommandResult<SlotRecord>.Success(slot);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating new section from {@message} - {exception}", message, ex.ToString());
                return CommandResult<SlotRecord>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}