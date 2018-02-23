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

        public MenuLeafPageContentRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
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

                var q = from a in _uow.Context.ArticleStates
                    .Include(x => x.Component)
                    .Include(x => x.PageMetadata)
                    .Include(x => x.ArticleTags).ThenInclude(x => x.Tag).ThenInclude(x => x.Component)
                    .AsNoTracking()
                        select a;

                var hit = await q.FirstOrDefaultAsync(x => x.Id == id);
                if (hit == null) throw new ArgumentOutOfRangeException(nameof(id));

                var result = new PageContent()
                {
                    Title = hit.Title,
                    Body = new[] { hit.Body },
                };
                if (hit.PageMetadata != null && !hit.PageMetadata.HtmlMetaJsonText.IsNullOrWhiteSpace())
                {
                    result.Metas.AddRange(JsonConvert.DeserializeObject<HtmlMeta[]>(hit.PageMetadata?.HtmlMetaJsonText));
                }
                result.Tags.AddRange(hit.Tags.Where(x => x.Component.OkToDisplay()).Select(x => new Tag(x.Tag, x.TagSlug)));
                result.ComponentKey = id.ToString();
                if (hit.PageMetadata != null )
                {
                    result.PrimaryImageFileId = hit.PageMetadata.PrimaryImageFileId.HasValue
                        ? hit.PageMetadata.PrimaryImageFileId.Value.ToString()
                        : string.Empty;
                }

                return QueryResult<(int componentId, IPageContent content)>.Success((componentId: id, content: result));
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<(int componentId, IPageContent content)>.Failure(e.Message);
            }
        }
    }
}