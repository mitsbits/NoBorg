using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
            try
            {
                var result = await _uow.Context.DeviceRecords.Include(x => x.Sections).ThenInclude(x => x.Slots)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == message.RecordId);

                return QueryResult<DeviceRecord>.Success(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<DeviceRecord>.Failure(e.Message);
            }
        }
    }
}