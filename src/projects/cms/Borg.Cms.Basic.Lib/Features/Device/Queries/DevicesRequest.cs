using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.Features.Device.Queries
{
    public class DevicesRequest : IRequest<QueryResult<IEnumerable<DeviceState>>>
    {
        public DevicesRequest()
        {
         
        }

  
    }
    public class DevicesRequestHandler : AsyncRequestHandler<DevicesRequest, QueryResult<IEnumerable<DeviceState>>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public DevicesRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IEnumerable<DeviceState>>> HandleCore(DevicesRequest message)
        {
            var repo = _uow.QueryRepo<DeviceState>();
            var bldr = SortBuilder.Get<DeviceState>().Add(x => x.FriendlyName).Add(x => x.Layout);
            var results = await repo.Find(x => true, bldr.Build());
            return QueryResult<IEnumerable<DeviceState>>.Success(results);
        }
    }



}