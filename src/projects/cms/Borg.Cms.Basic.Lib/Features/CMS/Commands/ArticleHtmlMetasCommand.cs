using Borg.Cms.Basic.Lib.Features.CMS.Events;
using Borg.Infra.DAL;
using Borg.Infra.Services.Slugs;
using Borg.MVC.BuildingBlocks;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Borg.CMS.Components;

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{
    public class ArticleHtmlMetasCommand : CommandBase<CommandResult>
    {
        [Required]
        public int RecordId { get; set; }

        [DisplayName("Html Metas")]
        public string HtmlMetas { get; set; }
    }

    public class ArticleHtmlMetasCommandHandler : AsyncRequestHandler<ArticleHtmlMetasCommand, CommandResult>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly ISlugifierService _slugifier;

        public ArticleHtmlMetasCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, ISlugifierService slugifier)
        {
            _uow = uow;
            _dispatcher = dispatcher;
            _slugifier = slugifier;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected override async Task<CommandResult> HandleCore(ArticleHtmlMetasCommand message)
        {
            try
            {
                ArticleHtmlMetasChangedEvent @event = null;
                var article = await _uow.ReadWriteRepo<ArticleState>().Get(x => x.Id == message.RecordId);
                if (article == null)
                {
                    _logger.Warn("Article with id {id} was not found", message.RecordId);
                    return CommandResult.Failure($"Article with id {message.RecordId} was not found");
                }

                var pagemetadata = await _uow.ReadWriteRepo<PageMetadataState>().Get(x => x.Id == message.RecordId);
                if (pagemetadata == null)
                {
                    pagemetadata = new PageMetadataState() { Id = message.RecordId, HtmlMetaJsonText = message.HtmlMetas };
                    await _uow.ReadWriteRepo<PageMetadataState>().Create(pagemetadata);
                }
                else
                {
                    pagemetadata.HtmlMetaJsonText = message.HtmlMetas;
                    await _uow.ReadWriteRepo<PageMetadataState>().Update(pagemetadata);
                }

                await _uow.Save();
                @event = new ArticleHtmlMetasChangedEvent(message.RecordId, JsonConvert.DeserializeObject<HtmlMeta[]>(pagemetadata.HtmlMetaJsonText));
                _dispatcher.Publish(@event);
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error setting Html Metas for article from {@message} - {exception}", message, ex.ToString());
                return CommandResult<HtmlSnippetState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}