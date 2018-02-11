using Borg.Cms.Basic.Lib.Features.CMS.Events;
using Borg.Infra.DAL;
using Borg.Infra.Services.Slugs;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{
    public class RenameArticleCommand : CommandBase<CommandResult>
    {
        [Required]
        public int RecordId { get; set; }

        [Required]
        [MaxLength(1024)]
        [DisplayName("Title")]
        public string NewTitle { get; set; }

        [DisplayName("Set Slug")]
        public bool AlsoSetSlug { get; set; } = false;
    }

    public class RenameArticleCommandHandler : AsyncRequestHandler<RenameArticleCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly ISlugifierService _slugifier;

        public RenameArticleCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, ISlugifierService slugifier)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _slugifier = slugifier;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(RenameArticleCommand message)
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

                var oldTitle = article.Title;
                var newTitle = message.NewTitle;
                var oldslug = article.Slug;
                var newslug = article.Slug;
                if (message.AlsoSetSlug)
                {
                    newslug = _slugifier.Slugify(newTitle, 1024);
                }

                article.Title = newTitle;
                article.Slug = newslug;
                await _uow.ReadWriteRepo<ArticleState>().Update(article);
                await _uow.Save();
                @event = new ArticleRenamedEvent(message.RecordId, oldTitle, newTitle, oldslug, newslug);
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