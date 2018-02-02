using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Queries
{
    public class SlotRequest : IRequest<QueryResult<SlotViewModel>>
    {
        public SlotRequest(int recordId)
        {
            RecordId = recordId;
        }

        public int RecordId { get; }
    }

    public class SlotRequestHandler : AsyncRequestHandler<SlotRequest, QueryResult<SlotViewModel>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public SlotRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<SlotViewModel>> HandleCore(SlotRequest message)
        {
            var result = await _uow.Context.SlotStates.Include(x => x.Section).ThenInclude(x => x.Device)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == message.RecordId);
            var device = await _uow.Context.DeviceStates.Include(x => x.Sections).ThenInclude(x => x.Slots).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == result.Section.DeviceId);
            return QueryResult<SlotViewModel>.Success(new SlotViewModel() { State = result, DeviceState = device });
        }
    }

    public class SlotViewModel
    {
        public SlotState State { get; set; }
        public DeviceState DeviceState { get; set; }
    }
}