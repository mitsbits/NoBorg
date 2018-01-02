using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.Features.Device.Queries
{
    public class DevicesRequest : IRequest<QueryResult<IEnumerable<DeviceRecord>>>
    {
        public DevicesRequest()
        {
         
        }

  
    }
    public class DevicesRequestHandler : AsyncRequestHandler<DevicesRequest, QueryResult<IEnumerable<DeviceRecord>>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<BorgDbContext> _uow;

        public DevicesRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IEnumerable<DeviceRecord>>> HandleCore(DevicesRequest message)
        {
            var repo = _uow.QueryRepo<DeviceRecord>();
            var bldr = SortBuilder.Get<DeviceRecord>().Add(x => x.FriendlyName).Add(x => x.Layout);
            var results = await repo.Find(x => true, bldr.Build());
            return QueryResult<IEnumerable<DeviceRecord>>.Success(results);
        }
    }



}