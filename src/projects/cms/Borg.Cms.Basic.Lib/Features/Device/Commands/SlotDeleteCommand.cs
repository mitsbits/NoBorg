using Borg.Cms.Basic.Lib.Features.Device.Events;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Commands
{
    public class SlotDeleteCommand : CommandBase<CommandResult>
    {
        public SlotDeleteCommand()
        {
        }

        public SlotDeleteCommand(int recordId)
        {
            RecordId = recordId;
        }

        [Required]
        public int RecordId { get; set; }
    }

    public class SlotDeleteCommandHandler : AsyncRequestHandler<SlotDeleteCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<BorgDbContext> _uow;

        private readonly IMediator _dispatcher;

        public SlotDeleteCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow, IMediator dispatcher)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(SlotDeleteCommand message)
        {
            try
            {
                var repo = _uow.ReadWriteRepo<SlotState>();
                await repo.Delete(x => x.Id == message.RecordId);
                await _uow.Save();
                await _dispatcher.Publish(new SlotRecordStateChanged(message.RecordId, DmlOperation.Delete));
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error deleting slot from {@message} - {exception}", message, ex.ToString());
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}