using System;
using System.Linq;
using System.Threading.Tasks;
using Borg;
using Borg.CMS.Components;
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

//IPageContent
namespace Borg.Cms.Basic.Lib.Features.CMS.Queries
{
    public class ComponentPageContentRequest : IRequest<QueryResult<(int componentId, IPageContent content)>>
    {
        public ComponentPageContentRequest(int recordId)
        {
            RecordId = recordId;
        }
        public int RecordId { get; }
    }

    public class ComponentPageContentRequestHandler : AsyncRequestHandler<ComponentPageContentRequest, QueryResult<(int componentId, IPageContent content)>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public ComponentPageContentRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = LoggerFactoryExtensions.CreateLogger(loggerFactory, GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<(int componentId, IPageContent content)>> HandleCore(ComponentPageContentRequest message)
        {
            try
            {
        

                var q = from a in EntityFrameworkQueryableExtensions.Include<ArticleState, ComponentState>(_uow.Context.ArticleStates, x => x.Component)
                    .Include(x => x.PageMetadata)
                    .Include(x => x.ArticleTags).ThenInclude(x => x.Tag).ThenInclude(x => x.Component)
                    .AsNoTracking()
                        select a;

                var hit = await q.FirstOrDefaultAsync(x => x.Id == message.RecordId);
                if (hit == null) throw new ArgumentOutOfRangeException(nameof(message.RecordId));

                var result = new PageContent()
                {
                    Title = hit.Title,
                    MainContent =  hit.Body ,
                };

                result.Tags.AddRange(hit.Tags.Where(x => x.Component.OkToDisplay()).Select(x => new Tag(x.Tag, x.TagSlug)));
                result.Id = message.RecordId;
                result.ComponentKey = message.RecordId.ToString();

                if (hit.PageMetadata != null)
                {
                    if (!hit.PageMetadata.HtmlMetaJsonText.IsNullOrWhiteSpace())
                        result.Metas.AddRange(JsonConvert.DeserializeObject<HtmlMeta[]>(hit.PageMetadata.HtmlMetaJsonText));

                    result.PrimaryImageFileId = hit.PageMetadata.PrimaryImageFileId.HasValue
                        ? hit.PageMetadata.PrimaryImageFileId.Value.ToString()
                        : string.Empty;
                }

                return QueryResult<(int componentId, IPageContent content)>.Success((componentId: message.RecordId, content: result));
            }
            catch (Exception e)
            {
                LoggerExtensions.Error(_logger, e);
                return QueryResult<(int componentId, IPageContent content)>.Failure(e.Message);
            }
        }
    }
}