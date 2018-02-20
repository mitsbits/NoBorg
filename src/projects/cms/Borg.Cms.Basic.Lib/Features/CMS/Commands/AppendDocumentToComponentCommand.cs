using Borg.Cms.Basic.Lib.Features.CMS.Events;
using Borg.CMS.Documents.Contracts;
using Borg.Infra.DAL;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Platform.EF.CMS;
using Borg.Platform.EF.CMS.Data;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Lib.Features.CMS.Commands
{
    public class AppendDocumentToComponentCommand : CommandBase<CommandResult<ComponentDocumentAssociationState>>
    {
        public int ComponentId { get; set; }
        public IFormFile File { get; set; }

        [Required]
        public string Email { get; set; }
    }

    public class AppendDocumentToComponentCommandHandler : AsyncRequestHandler<AppendDocumentToComponentCommand, CommandResult<ComponentDocumentAssociationState>>
    {
        private readonly ILogger _logger;

        private readonly IUnitOfWork<CmsDbContext> _uow;

        private readonly IMediator _dispatcher;

        private readonly IDocumentsService<int> _documents;

        private readonly IAssetStoreDatabaseService<int> _assetStoreDatabase;

        public AppendDocumentToComponentCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<CmsDbContext> uow, IMediator dispatcher, IDocumentsService<int> documents, IAssetStoreDatabaseService<int> assetStoreDatabase)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _dispatcher = dispatcher;
            _documents = documents;
            _assetStoreDatabase = assetStoreDatabase;
        }

        protected override async Task<CommandResult<ComponentDocumentAssociationState>> HandleCore(AppendDocumentToComponentCommand message)
        {
            try
            {
                ComponentDocumentAssociationEvent @event = null;
                var component = await _uow.ReadWriteRepo<ComponentState>().Get(x => x.Id == message.ComponentId, CancellationToken.None);
                if (component == null)
                {
                    _logger.Warn("Component with id {id} was not found", message.ComponentId);
                    return CommandResult<ComponentDocumentAssociationState>.FailureWithEmptyPayload($"Component with id {message.ComponentId} was not found");
                }

                (int docid, int fileid) definition;
                string filename = ContentDispositionHeaderValue.Parse(message.File.ContentDisposition).FileName.ToString().Trim('"');
                filename = filename.EnsureCorrectFilenameFromUpload();

                using (var stream = new MemoryStream())
                {
                    await message.File.CopyToAsync(stream);
                    stream.Seek(0, 0);
                    definition = await _documents.StoreUserDocument(stream.ToArray(), filename, message.Email);
                }
                if (definition.docid <= 0) return CommandResult<ComponentDocumentAssociationState>.FailureWithEmptyPayload($"Could not created document {filename} for Component with id {message.ComponentId} was not found");

                var state = new ComponentDocumentAssociationState() { ComponentId = message.ComponentId, DocumentId = definition.docid, FileId = definition.fileid };
                var spec = await _assetStoreDatabase.CurrentFile(definition.docid);
                state.MimeType = spec.MimeType;
                state.Uri = spec.FullPath;
                state.Version = 1;

                await _uow.ReadWriteRepo<ComponentDocumentAssociationState>().Create(state);
                await _uow.Save();
                @event = new ComponentDocumentAssociationEvent(state.ComponentId, state.DocumentId);
                _dispatcher.Publish(@event);
                return CommandResult<ComponentDocumentAssociationState>.Success(state);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error settingprimary image for article from {@message} - {exception}", message, ex.ToString());
                return CommandResult<ComponentDocumentAssociationState>.FailureWithEmptyPayload(ex.ToString());
            }
        }
    }
}