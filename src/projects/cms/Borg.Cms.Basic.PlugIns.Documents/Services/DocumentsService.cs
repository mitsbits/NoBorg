using Borg.Cms.Basic.PlugIns.Documents.Commands;
using Borg.Infra.Storage;
using Borg.Infra.Storage.Contracts;
using Borg.Infra.Storage.Documents;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Services
{
    public class DocumentsService : IDocumentsService<int>
    {
        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;

        public DocumentsService(ILoggerFactory loggerFactory, IMediator dispatcher)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _dispatcher = dispatcher;
        }

        public event FileCreatedEventHandler<int> FileCreated;

        public async Task<(int docid, IFileSpec<int> file)> StoreUserDocument(byte[] data, string filename, string userHandle)
        {
            try
            {
                var command = new StoreUserDocumentCommand(userHandle, filename, data);
                var result = await _dispatcher.Send(command);
                return result.Succeded ? result.Payload : (docid: -1, file: default(FileSpecDefinition<int>));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating document from {message} - {exception}", filename, ex.ToString());
                return (docid: -1, file: default(FileSpecDefinition<int>));
            }
        }
    }
}