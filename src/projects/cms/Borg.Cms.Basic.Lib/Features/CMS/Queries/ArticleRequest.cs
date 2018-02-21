using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Queries
{
    public class ArticleRequest : IRequest<QueryResult<ArticleState>>
    {
        public ArticleRequest(int recordId)
        {
            RecordId = recordId;
        }

        public int RecordId { get; }
    }

    public class ArticleRequestHandler : AsyncRequestHandler<ArticleRequest, QueryResult<ArticleState>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public ArticleRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<ArticleState>> HandleCore(ArticleRequest message)
        {
            try
            {
                var article = await _uow.Context.ArticleStates.Include(a => a.ArticleTags).ThenInclude(at => at.Tag)
                    .Include(a => a.Component).Include(x=>x.PageMetadata).FirstOrDefaultAsync(x => x.Id == message.RecordId);
                if (article == null)
                {
                    _logger.Warn("Article with id {id} was not found", message.RecordId);
                    return QueryResult<ArticleState>.Failure(
                        $"Article with id {message.RecordId} was not found");
                }

                return QueryResult<ArticleState>.Success(article);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<ArticleState>.Failure(e.Message);
            }
        }
    }
}