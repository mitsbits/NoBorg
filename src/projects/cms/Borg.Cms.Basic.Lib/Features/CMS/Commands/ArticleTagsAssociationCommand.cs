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
        public int RecordId { get; set; }

        public string[] Tags { get; set; }

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
                if (article == null)
                {
                    var notfoundmessage = $"Atricle with id {message.RecordId} was not found - {nameof(ArticleTagsAssociationCommandHandler)}";
                    _logger.Warn(notfoundmessage);
                    return CommandResult.Failure(notfoundmessage);
                }
                article.ArticleTags.Clear();
                await _uow.ReadWriteRepo<ArticleState>().Update(article);
                await _uow.Save();
                foreach (var messageTag in message.Tags)
                {
                    var tag = messageTag.Trim();
                    var hit = await _uow.ReadWriteRepo<TagState>().Get(x => x.Tag == tag);
                    if (hit == null)
                    {
                        hit = new TagState()
                        {
                            Tag = tag,
                            TagNormalized = tag.ToUpperInvariant(),
                            TagSlug = _slugifier.Slugify(tag, 1024),
                            Component = new ComponentState() { IsDeleted = false, IsPublished = true }
                        };
                        hit = await _uow.ReadWriteRepo<TagState>().Create(hit);
                    }
                    await _uow.ReadWriteRepo<ArticleTagState>()
                        .Create(new ArticleTagState() {ArticleId = message.RecordId, TagId = hit.Id});
                }
                await _uow.Save();
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error associating Tags to Article from {@message} - {exception}", message, ex.ToString());
                return CommandResult<HtmlSnippetState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}
