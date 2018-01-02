using System.Threading;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.Features.Device.Queries
{
    public class DeviceRequest : IRequest<QueryResult<DeviceRecord>>
    {
        public DeviceRequest(int recordId)
        {
            RecordId = recordId;
        }
        public int RecordId { get; }

    }
    public class DeviceRequestHandler : AsyncRequestHandler<DeviceRequest, QueryResult<DeviceRecord>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<BorgDbContext> _uow;

        public DeviceRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<DeviceRecord>> HandleCore(DeviceRequest message)
        {
            var repo = _uow.QueryRepo<DeviceRecord>();
            var results = await repo.Get(x => x.Id == message.RecordId, CancellationToken.None,
                record => record.Sections);
            return QueryResult<DeviceRecord>.Success(results);
        }
    }


     
}