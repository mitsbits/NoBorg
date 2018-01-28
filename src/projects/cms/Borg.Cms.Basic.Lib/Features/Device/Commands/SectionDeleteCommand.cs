using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Borg.Platform.EF.CMS;

namespace Borg.Cms.Basic.Lib.Features.Device.Commands
{
    public class SectionDeleteCommand : CommandBase<CommandResult>
    {
        public SectionDeleteCommand()
        {
        }

        public SectionDeleteCommand(int recordId)
        {
            RecordId = recordId;
        }

        [Required]
        public int RecordId { get; set; }
    }

    public class SectionDeleteCommandHandler : AsyncRequestHandler<SectionDeleteCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<BorgDbContext> _uow;

        private readonly IMediator _dispatcher;

        public SectionDeleteCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(SectionDeleteCommand message)
        {
            try
            {
                var repo = _uow.ReadWriteRepo<SectionState>();
                var slots = await _uow.QueryRepo<SlotState>().Find(x => x.SectionId == message.RecordId, null);
                foreach (var slot in slots)
                {
                    await _dispatcher.Send(new SlotDeleteCommand(slot.Id));
                }
                await repo.Delete(x => x.Id == message.RecordId);
                await _uow.Save();
                await _dispatcher.Publish(new SectionRecordStateChanged(message.RecordId, DmlOperation.Delete));
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error deleting section from {@message} - {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}