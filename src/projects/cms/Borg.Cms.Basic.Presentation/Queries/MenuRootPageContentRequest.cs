using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Borg.Cms.Basic.Presentation.Queries
{
    public class MenuRootPageContentRequest : IRequest<QueryResult<IPageContent>>
    {
        public MenuRootPageContentRequest(string menuSlug)
        {
            MenuSlug = menuSlug;
        }

        public string MenuSlug { get; }
    }

    public class MenuRootPageContentRequestHandler : AsyncRequestHandler<MenuRootPageContentRequest, QueryResult<IPageContent>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public MenuRootPageContentRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IPageContent>> HandleCore(MenuRootPageContentRequest message)
        {
            try
            {
                var qc = from n in _uow.Context.NavigationItemStates.AsNoTracking()
                         where n.Taxonomy.ParentId == 0 && n.Path.ToLower() == message.MenuSlug.ToLower()
                         select n.Id;
                var id = await qc.FirstOrDefaultAsync();
                if (id == default(int)) throw new ArgumentOutOfRangeException(nameof(id));

                var q = from a in _uow.Context.ArticleStates
                    .Include(x => x.Component)
                    .Include(x => x.PageMetadata)
                    .Include(x => x.ArticleTags).ThenInclude(x => x.Tag)
                    .AsNoTracking()
                        select a;

                var hit = await q.FirstOrDefaultAsync(x => x.Id == id);
                if (hit == null) throw new ArgumentOutOfRangeException(nameof(id));

                var result = new PageContent()
                {
                    Title = hit.Title,
                    Body = new[] { hit.Body },
                };
                result.Metas.AddRange(JsonConvert.DeserializeObject<HtmlMeta[]>(hit.PageMetadata.HtmlMetaJsonText));
                return QueryResult<IPageContent>.Success(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<IPageContent>.Failure(e.Message);
            }
        }
    }
}
