using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.Navigation.Queries
{
    public class MenuGroupRecordsRequest : IRequest<QueryResult<IEnumerable<NavigationItemState>>>
    {
        public MenuGroupRecordsRequest(string @group, bool excludeSupressed = false)
        {
            Group = @group;
            ExcludeSupressed = excludeSupressed;
        }

        public string Group { get; }
        public bool ExcludeSupressed { get; } = false;
    }

    public class MenuGroupRecordsRequestHandler : AsyncRequestHandler<MenuGroupRecordsRequest, QueryResult<IEnumerable<NavigationItemState>>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public MenuGroupRecordsRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IEnumerable<NavigationItemState>>> HandleCore(MenuGroupRecordsRequest message)
        {
            try
            {
                Expression<Func<NavigationItemState, bool>> predicate = (x) => x.GroupCode.ToLower() == message.Group.ToLower();

                if (message.ExcludeSupressed)
                {
                    predicate = (x) => x.GroupCode.ToLower() == message.Group.ToLower() && x.Taxonomy.Component.IsPublished && !x.Taxonomy.Component.IsDeleted;
                }
                var set = await _uow.Context.NavigationItemStates.Include(x => x.Taxonomy).ThenInclude(x => x.Component)
                        .Include(x => x.Taxonomy).ThenInclude(x => x.Article)
                        .Where(predicate).ToListAsync();

                return QueryResult<IEnumerable<NavigationItemState>>.Success(set);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<IEnumerable<NavigationItemState>>.Failure(e.Message);
            }
        }
    }
}