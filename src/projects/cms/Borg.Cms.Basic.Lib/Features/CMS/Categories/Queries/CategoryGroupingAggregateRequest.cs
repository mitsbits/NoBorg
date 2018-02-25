using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Categories.Queries
{
    public class CategoryGroupingAggregateRequest : IRequest<QueryResult<CategoryGroupingState>>
    {
        public CategoryGroupingAggregateRequest(int recordId)
        {
            RecordId = recordId;
        }

        public int RecordId { get; }
    }

    public class CategoryGroupingAggregateRequestHandler : AsyncRequestHandler<CategoryGroupingAggregateRequest, QueryResult<CategoryGroupingState>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public CategoryGroupingAggregateRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<CategoryGroupingState>> HandleCore(CategoryGroupingAggregateRequest message)
        {
            try
            {
                var hit = await _uow.Context.CategoryGroupingStates.Include(x => x.Component).Include(x => x.Categories)
                    .ThenInclude(x => x.Component).AsNoTracking().FirstOrDefaultAsync(x => x.Id == message.RecordId);
                return hit == null ? QueryResult<CategoryGroupingState>.Failure($"No Category Grouping for {message.RecordId}") : QueryResult<CategoryGroupingState>.Success(hit);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<CategoryGroupingState>.Failure(e.Message);
            }
        }
    }
}