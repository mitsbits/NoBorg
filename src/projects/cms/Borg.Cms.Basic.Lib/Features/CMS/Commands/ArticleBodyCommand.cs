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
    public class ArticleBodyCommand : CommandBase<CommandResult>
    {
        [Required]
        public int RecordId { get; set; }

        [DisplayName("Body")]
        [UIHint("CKEDITOR4")]
        public string Body { get; set; }
    }

    public class ArticleBodyCommandHandler : AsyncRequestHandler<ArticleBodyCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly ISlugifierService _slugifier;

        public ArticleBodyCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, ISlugifierService slugifier)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _slugifier = slugifier;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(ArticleBodyCommand message)
        {
            try
            {
                ArticleBodyChangedEvent @event = null;
                var article = await _uow.ReadWriteRepo<ArticleState>().Get(x => x.Id == message.RecordId);
                if (article == null)
                {
                    _logger.Warn("Article with id {id} was not found", message.RecordId);
                    return CommandResult.Failure($"Article with id {message.RecordId} was not found");
                }

                var oldbosy = article.Body;
                var newBody = message.Body;
                article.Body = newBody;

                await _uow.ReadWriteRepo<ArticleState>().Update(article);
                await _uow.Save();
                @event = new ArticleBodyChangedEvent(message.RecordId, newBody, oldbosy);
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