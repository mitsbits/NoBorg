using System;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Borg.Cms.Basic.Presentation.Queries
{
    public class MenuLeafPageContentRequest : IRequest<QueryResult<(int componentId, IPageContent content)>>
    {
        public MenuLeafPageContentRequest(string parentSlug, string childSlug)
        {
            ParentSlug = parentSlug;
            ChildSlug = childSlug;
        }

        public string ParentSlug { get; }
        public string ChildSlug { get; }
    }
    public class MenuLeafPageContentRequestHandler : AsyncRequestHandler<MenuLeafPageContentRequest, QueryResult<(int componentId, IPageContent content)>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly IMediator _dispatcher;

        public MenuLeafPageContentRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
        }

        protected override async Task<QueryResult<(int componentId, IPageContent content)>> HandleCore(MenuLeafPageContentRequest message)
        {
            try
            {
                var qc = from n in _uow.Context.NavigationItemStates.AsNoTracking()
                         join p in _uow.Context.NavigationItemStates.AsNoTracking() on n.Taxonomy.ParentId equals p.Taxonomy.Id
                         where n.Taxonomy.ParentId > 0 && n.Path.ToLower() == message.ChildSlug.ToLower()
                         && p.Path.ToLower() == message.ParentSlug.ToLower()
                         select n.Id;
                var id = await qc.FirstOrDefaultAsync();
                if (id == default(int)) throw new ArgumentOutOfRangeException(nameof(id));

                return await _dispatcher.Send(new ComponentPageContentRequest(id));
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<(int componentId, IPageContent content)>.Failure(e.Message);
            }
        }
    }
}