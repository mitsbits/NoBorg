using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.Features.Device.Queries
{
  public  class SectionRequest : IRequest<QueryResult<SectionRecord>>
    {
        public SectionRequest(int recordId)
        {
            RecordId = recordId;
        }
        public int RecordId { get; }
    }

    public class SectionRequestHandler : AsyncRequestHandler<SectionRequest, QueryResult<SectionRecord>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<BorgDbContext> _uow;

        public SectionRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<SectionRecord>> HandleCore(SectionRequest message)
        {
            var repo = _uow.QueryRepo<SectionRecord>();
            var results = await repo.Get(x => x.Id == message.RecordId, CancellationToken.None,
                record => record.Device, record=> record.Slots);
            return QueryResult<SectionRecord>.Success(results);
        }
    }
}
