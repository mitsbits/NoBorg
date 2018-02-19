using Borg.Infra.DAL;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Queries
{
    public class HtmlSnippetContentRequest : IRequest<QueryResult<string>>
    {
        public HtmlSnippetContentRequest(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }

    public class HtmlSnippetContentRequestHandler : AsyncRequestHandler<HtmlSnippetContentRequest, QueryResult<string>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public HtmlSnippetContentRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<string>> HandleCore(HtmlSnippetContentRequest message)
        {
            try
            {
                var result = await _uow.Context.HtmlSnippetStates.Where(x =>
                    x.Component.IsPublished
                    && !x.Component.IsDeleted
                    && x.Code.ToLower() == message.Key.ToLower())
                    .Select(x => x.HtmlSnippet)
                    .FirstOrDefaultAsync();

                var output = result.IsNullOrWhiteSpace() ? string.Empty : result;

                return QueryResult<string>.Success(output);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<string>.Failure(e.Message);
            }
        }
    }
}