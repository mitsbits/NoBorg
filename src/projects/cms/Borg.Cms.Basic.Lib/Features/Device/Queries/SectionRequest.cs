using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Device.Queries
{
    public class SectionRequest : IRequest<QueryResult<SectionState>>
    {
        public SectionRequest(int recordId)
        {
            RecordId = recordId;
        }

        public int RecordId { get; }
    }

    public class SectionRequestHandler : AsyncRequestHandler<SectionRequest, QueryResult<SectionState>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public SectionRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<SectionState>> HandleCore(SectionRequest message)
        {
            var repo = _uow.QueryRepo<SectionState>();
            var results = await repo.Get(x => x.Id == message.RecordId, CancellationToken.None,
                record => record.Device, record => record.Slots);
            return QueryResult<SectionState>.Success(results);
        }
    }
}