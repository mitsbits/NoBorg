using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
                var result = await _uow.QueryRepo<ArticleState>().Get(x => x.Id == message.RecordId, CancellationToken.None, a => a.Tags, a => a.Component);

                return QueryResult<ArticleState>.Success(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<ArticleState>.Failure(e.Message);
            }
        }
    }
}
