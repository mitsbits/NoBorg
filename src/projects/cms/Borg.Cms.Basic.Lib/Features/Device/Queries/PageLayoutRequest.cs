using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Queries
{
    public class PageLayoutRequest : IRequest<QueryResult<IDeviceStructureInfo>>
    {
        public PageLayoutRequest(int recordId)
        {
            RecordId = recordId;
        }

        public int RecordId { get; }
    }

    public class PageLayoutRequestHandler : AsyncRequestHandler<PageLayoutRequest, QueryResult<IDeviceStructureInfo>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public PageLayoutRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IDeviceStructureInfo>> HandleCore(PageLayoutRequest message)
        {
            var hit = await _uow.Context.DeviceStates.Include(x => x.Sections).ThenInclude(x => x.Slots)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == message.RecordId);
            if (hit == null) return QueryResult<IDeviceStructureInfo>.Failure($"No device for id {message.RecordId}");

            var result = hit.DeviceStructureInfo();
            return QueryResult<IDeviceStructureInfo>.Success(result);
        }
    }
}