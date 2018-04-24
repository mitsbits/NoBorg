using Borg.Cms.Basic.Lib.Features.CMS.Events;
using Borg.CMS.Documents.Contracts;
using Borg.Infra.DAL;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{
    public class ArticlePrimaryImageCommand : CommandBase<CommandResult<(int documentId, int fileId)>>
    {
        [Required]
        public int RecordId { get; set; }

        [Required]
        [DisplayName("Clear")]
        public bool RemoveOperation { get; set; }

        public IFormFile File { get; set; }

        [Required]
        public string UserHandle { get; set; }

        public int? ExistingFile { get; set; }
    }

    public class ArticlePrimaryImageCommandHandler : AsyncRequestHandler<ArticlePrimaryImageCommand, CommandResult<(int documentId, int fileId)>>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly IDocumentsService<int> _documents;

        public ArticlePrimaryImageCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, IDocumentsService<int> documents)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
            _documents = documents;
        }

        protected override async Task<CommandResult<(int documentId, int fileId)>> HandleCore(ArticlePrimaryImageCommand message)
        {
            try
            {
                ArticlePrimaryImageChangedEvent @event = null;
                var article = await _uow.ReadWriteRepo<ArticleState>().Get(x => x.Id == message.RecordId, CancellationToken.None, x => x.PageMetadata);
                if (article == null)
                {
                    _logger.Warn("Article with id {id} was not found", message.RecordId);
                    return CommandResult<(int documentId, int fileId)>.FailureWithEmptyPayload($"Article with id {message.RecordId} was not found");
                }

                if (article.PageMetadata == null) article.PageMetadata = new PageMetadataState();
                var prevdoc = article.PageMetadata.PrimaryImageDocumentId;
                var prevfile = article.PageMetadata.PrimaryImageFileId;
                var prev = (documentId: prevdoc, fileId: prevfile);
                (int documentId, int fileId) current;
                if (message.RemoveOperation)
                {
                    current = (documentId: -1, fileId: -1);
                    article.PageMetadata.PrimaryImageDocumentId = current.documentId;
                    article.PageMetadata.PrimaryImageFileId = current.fileId;
                    @event = new ArticlePrimaryImageChangedEvent(message.RecordId, current, prev);
                    await _uow.ReadWriteRepo<ArticleState>().Update(article);
                    await _uow.Save();
                }
                else
                {
                    string filename = ContentDispositionHeaderValue.Parse(message.File.ContentDisposition).FileName.ToString().Trim('"');
                    filename = filename.EnsureCorrectFilenameFromUpload();

                    using (var stream = new MemoryStream())
                    {
                        await message.File.CopyToAsync(stream);
                        stream.Seek(0, 0);
                        var result = await _documents.StoreUserDocument(stream.ToArray(), filename, message.UserHandle);
                        current = (documentId: result.docid, fileId: result.fieldid);
                        @event = new ArticlePrimaryImageChangedEvent(message.RecordId, current, prev);
                        article.PageMetadata.PrimaryImageDocumentId = current.documentId;
                        article.PageMetadata.PrimaryImageFileId = current.fileId;
                        await _uow.ReadWriteRepo<ArticleState>().Update(article);
                        await _uow.Save();
                    }
                }

                _dispatcher.Publish(@event);
                return CommandResult<(int documentId, int fileId)>.Success(current);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error settingprimary image for article from {@message} - {exception}", message, ex.ToString());
                return CommandResult<(int documentId, int fileId)>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}