using Borg.Infra.Collections;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Categories.Queries
{
    public class CategoryGroupingIndexRequest : IRequest<QueryResult<IPagedResult<CategoryGroupingState>>>
    {
    }

    public class CategoryGroupingIndexRequestHandler : AsyncRequestHandler<CategoryGroupingIndexRequest, QueryResult<IPagedResult<CategoryGroupingState>>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public CategoryGroupingIndexRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IPagedResult<CategoryGroupingState>>> HandleCore(CategoryGroupingIndexRequest message)
        {
            try
            {
                var hits = await _uow.Context.CategoryGroupingStates.Include(x => x.Component).AsNoTracking()
                    .ToArrayAsync();
                if (hits.Any()) return QueryResult<IPagedResult<CategoryGroupingState>>.Failure($"No Category Groupings");
                return QueryResult<IPagedResult<CategoryGroupingState>>.Success(new PagedResult<CategoryGroupingState>(hits, 1, hits.Length, hits.Length));
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<IPagedResult<CategoryGroupingState>>.Failure(e.Message);
            }
        }
    }
}