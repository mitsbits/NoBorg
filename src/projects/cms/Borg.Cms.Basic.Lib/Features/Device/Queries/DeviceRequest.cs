using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;

namespace Borg.Cms.Basic.Lib.Features.Device.Queries
{
    public class DeviceRequest : IRequest<QueryResult<DeviceState>>
    {
        public DeviceRequest(int recordId)
        {
            RecordId = recordId;
        }

        public int RecordId { get; }
    }

    public class DeviceRequestHandler : AsyncRequestHandler<DeviceRequest, QueryResult<DeviceState>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public DeviceRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<DeviceState>> HandleCore(DeviceRequest message)
        {
            try
            {
                var result = await _uow.Context.DeviceStates.Include(x => x.Sections).ThenInclude(x => x.Slots)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == message.RecordId);

                return QueryResult<DeviceState>.Success(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<DeviceState>.Failure(e.Message);
            }
        }
    }
}