using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Presentation.Queries
{
    public class MenuRootPageContentRequest : IRequest<QueryResult<(int componentId, IPageContent content)>>
    {
        public MenuRootPageContentRequest(string menuSlug)
        {
            MenuSlug = menuSlug;
        }

        public string MenuSlug { get; }
    }

    public class MenuRootPageContentRequestHandler : AsyncRequestHandler<MenuRootPageContentRequest, QueryResult<(int componentId, IPageContent content)>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;
        private readonly IMediator _dispatcher;

        public MenuRootPageContentRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
        }

        protected override async Task<QueryResult<(int componentId, IPageContent content)>> HandleCore(MenuRootPageContentRequest message)
        {
            try
            {
                var qc = from n in _uow.Context.NavigationItemStates.AsNoTracking()
                         where n.Taxonomy.ParentId == 0 && n.Path.ToLower() == message.MenuSlug.ToLower()
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