﻿using Borg.Cms.Basic.PlugIns.Documents.Commands;
using Borg.CMS.Documents.Contracts;
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

        public async Task<(int docid, int fileid)> StoreUserDocument(byte[] data, string filename, string userHandle)
        {
            try
            {
                var command = new StoreUserDocumentCommand(userHandle, filename, data);
                var result = await _dispatcher.Send(command);
                return result.Succeded ? result.Payload : (docid: -1, fileid: -1);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error creating document from {message} - {exception}", filename, ex.ToString());
                return (docid: -1, fileid: -1);
            }
        }
    }
}