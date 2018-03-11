using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Presentation.Queries
{
    public class ComponentDeviceRequest : IRequest<QueryResult<IDeviceStructureInfo>>
    {
        public ComponentDeviceRequest(int recordId)
        {
            RecordId = recordId;
        }

        public int RecordId { get; }
    }

    public class ComponentDeviceRequestHandler : AsyncRequestHandler<ComponentDeviceRequest, QueryResult<IDeviceStructureInfo>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public ComponentDeviceRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IDeviceStructureInfo>> HandleCore(ComponentDeviceRequest message)
        {
            try
            {
                var q = from d in _uow.Context.DeviceStates
                        .Include(x => x.Sections).ThenInclude(x => x.Slots)
                        .AsNoTracking()
                        join cd in _uow.Context.ComponentDeviceStates.AsNoTracking() on d.Id equals cd.DeviceId
                        where cd.ComponentId == message.RecordId
                        select d;

                var hit = await q.FirstOrDefaultAsync();
                if (hit == null) throw new ArgumentOutOfRangeException(nameof(message.RecordId));

                DeviceStructureInfo info = hit.DeviceStructureInfo();

                return QueryResult<IDeviceStructureInfo>.Success(info);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<IDeviceStructureInfo>.Failure(e.Message);
            }
        }
    }
}