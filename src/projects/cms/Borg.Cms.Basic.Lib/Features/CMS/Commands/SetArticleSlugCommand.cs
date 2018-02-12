using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.Features.CMS.Events;
using Borg.Infra.DAL;
using Borg.Infra.Services.Slugs;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{

    public class SetArticleSlugCommand : CommandBase<CommandResult>
    {
        [Required]
        public int RecordId { get; set; }



        [DisplayName("Slug")]
        public string Slug { get; set; } 
    }

    public class SetArticleSlugCommandHandler : AsyncRequestHandler<SetArticleSlugCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly ISlugifierService _slugifier;

        public SetArticleSlugCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, ISlugifierService slugifier)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _slugifier = slugifier;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(SetArticleSlugCommand message)
        {
            try
            {
                ArticleRenamedEvent @event = null;
                var article = await _uow.ReadWriteRepo<ArticleState>().Get(x => x.Id == message.RecordId);
                if (article == null)
                {
                    _logger.Warn("Article with id {id} was not found", message.RecordId);
                    return CommandResult.Failure($"Article with id {message.RecordId} was not found");
                }

                var oldslug = article.Slug;
                var newslug = message.Slug;
                article.Slug = newslug;
                await _uow.ReadWriteRepo<ArticleState>().Update(article);
                await _uow.Save();
                @event = new ArticleRenamedEvent(message.RecordId, article.Title, article.Title, oldslug, newslug);
                _dispatcher.Publish(@event);
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error renaming articlet from {@message} - {exception}", message, ex.ToString());
                return CommandResult<HtmlSnippetState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}
