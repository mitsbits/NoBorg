using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.Features.CMS.Queries
{
    public class HtmlSnippetModel : HtmlSnippetIndex
    {
        [UIHint("CKEDITOR")]
        public string Snippet { get; set; }
    }
    public class HtmlSnippetModelRequest : IRequest<QueryResult<HtmlSnippetModel>>
    {
        public HtmlSnippetModelRequest(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class HtmlSnippetModelRequestHandler : AsyncRequestHandler<HtmlSnippetModelRequest, QueryResult<HtmlSnippetModel>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<CmsDbContext> _uow;

        public HtmlSnippetModelRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
        }

        protected override async Task<QueryResult<HtmlSnippetModel>> HandleCore(HtmlSnippetModelRequest message)
        {
            try
            {
                var result = await _uow.QueryRepo<HtmlSnippetState>().Get(x => x.Id == message.Id);

                return QueryResult<HtmlSnippetModel>.Success(new HtmlSnippetModel() { Id = result.Id, Code = result.Code, IsDeleted = result.IsDeleted, IsPublished = result.IsPublished, Snippet = result.HtmlSnippet });
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<HtmlSnippetModel>.Failure(e.Message);
            }
        }
    }
}