using Borg.Infra.DAL;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Queries
{
    public class HtmlSnippetIndex
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        public string Code { get; set; }
    }

    public class HtmlSnippetIndicesRequest : IRequest<QueryResult<IEnumerable<HtmlSnippetIndex>>>
    {
    }

    public class HtmlSnippetIndicesRequestHandler : AsyncRequestHandler<HtmlSnippetIndicesRequest, QueryResult<IEnumerable<HtmlSnippetIndex>>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public HtmlSnippetIndicesRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<IEnumerable<HtmlSnippetIndex>>> HandleCore(HtmlSnippetIndicesRequest message)
        {
            try
            {
                var result = await _uow.Context.HtmlSnippetStates.Include(x => x.Component)
                    .AsNoTracking()
                    .Select(x => new HtmlSnippetIndex()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        IsDeleted = x.Component.IsDeleted,
                        IsPublished = x.Component.IsPublished
                    }).ToListAsync();

                return QueryResult<IEnumerable<HtmlSnippetIndex>>.Success(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<IEnumerable<HtmlSnippetIndex>>.Failure(e.Message);
            }
        }
    }
}