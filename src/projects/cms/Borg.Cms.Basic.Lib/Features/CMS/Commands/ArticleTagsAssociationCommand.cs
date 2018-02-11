using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Borg.Infra.DAL;
using Borg.Infra.Services.Slugs;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{
   public class ArticleTagsAssociationCommand : CommandBase<CommandResult>
    {
        public int RecordId { get; }

        public string[] Tags { get; }

    }


    public class ArticleTagsAssociationCommandHandler : AsyncRequestHandler<ArticleTagsAssociationCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly ISlugifierService _slugifier;

        public ArticleTagsAssociationCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, ISlugifierService slugifier)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _slugifier = slugifier;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(ArticleTagsAssociationCommand message)
        {
            try
            {
                var article = await _uow.ReadWriteRepo<ArticleState>().Get(x => x.Id == message.RecordId,
                    CancellationToken.None, x => x.ArticleTags);
                article.ArticleTags.Clear();
                foreach (var messageTag in message.Tags)
                {
                    var tag = messageTag.Trim();
                }
                await _uow.Save();
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating/updating html snippet from {@message} - {exception}", message, ex.ToString());
                return CommandResult<HtmlSnippetState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}
