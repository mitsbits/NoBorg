using Borg.Cms.Basic.Lib.System.Data;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Content.Queries
{
    public class ContentItemRequest : IRequest<QueryResult<ContentItemRecord>>
    {
        public ContentItemRequest(int recordId)
        {
            RecordId = recordId;
        }

        public int RecordId { get; }
    }

    public class ContentItemRequestHandler : AsyncRequestHandler<ContentItemRequest, QueryResult<ContentItemRecord>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<BorgDbContext> _uow;

        public ContentItemRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<BorgDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<ContentItemRecord>> HandleCore(ContentItemRequest message)
        {
            try
            {
                var result = await _uow.Context.ContentItemRecords
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == message.RecordId);

                return QueryResult<ContentItemRecord>.Success(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<ContentItemRecord>.Failure(e.Message);
            }
        }
    }
}