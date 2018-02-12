using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Queries
{
    public class TagSuggestionRequest : IRequest<QueryResult<Select2PagedResult>>
    {
        public TagSuggestionRequest(string searchTerm, int pageNum  = 1, int pageSize  = 30)
        {
            SearchTerm = searchTerm;
            PageSize = pageSize;
            PageNum = pageNum;
        }

        public string SearchTerm { get; }
        public int PageSize { get; }
        public int PageNum { get; }
    }

    public class TagSuggestionRequestHandler : AsyncRequestHandler<TagSuggestionRequest, QueryResult<Select2PagedResult>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public TagSuggestionRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<Select2PagedResult>> HandleCore(TagSuggestionRequest message)
        {
            try
            {
                var term = message.SearchTerm.ToUpper();
                var result = await _uow.QueryRepo<TagState>().Find(x => x.TagNormalized.StartsWith(term),
                    message.PageNum, message.PageSize, SortBuilder.Get<TagState>().Add(x => x.Tag).Build());
                var hits = result.Records.Select(x => new Select2Result() { id = x.Tag, text = x.Tag }).ToList();
                var page = new Select2PagedResult() { Total = result.TotalRecords, Results = hits };

                return QueryResult<Select2PagedResult>.Success(page);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<Select2PagedResult>.Failure(e.Message);
            }
        }
    }
}