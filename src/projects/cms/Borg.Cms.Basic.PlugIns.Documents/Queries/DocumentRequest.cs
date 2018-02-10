using Borg.Cms.Basic.PlugIns.Documents.Data;
using Borg.Cms.Basic.PlugIns.Documents.ViewModels;
using Borg.Infra.DAL;
using Borg.Infra.Storage.Assets;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Platform.EF.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.PlugIns.Documents.Queries
{
    public class DocumentRequest : IRequest<QueryResult<DocumentViewModel>>
    {
        public DocumentRequest(int documentId)
        {
            DocumentId = documentId;
        }

        public int DocumentId { get; }
    }

    public class DocumentRequestHandler : AsyncRequestHandler<DocumentRequest, QueryResult<DocumentViewModel>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<DocumentsDbContext> _uow;
        private readonly IAssetStore<AssetInfoDefinition<int>, int> _assetStore;

        public DocumentRequestHandler(ILoggerFactory loggerFactory, IUnitOfWork<DocumentsDbContext> uow, IAssetStore<AssetInfoDefinition<int>, int> assetStore)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _uow = uow;
            _assetStore = assetStore;
        }

        protected override async Task<QueryResult<DocumentViewModel>> HandleCore(DocumentRequest message)
        {
            try
            {
                var result = new DocumentViewModel
                {
                    Document = (await _uow.QueryRepo<DocumentState>().Find(x => x.Id == message.DocumentId, null,
                        CancellationToken.None, d => d.Owners, d => d.CheckOuts))[0],
                    Asset = (await _assetStore.Projections(new[] { message.DocumentId })).First()
                };
                return QueryResult<DocumentViewModel>.Success(result);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return QueryResult<DocumentViewModel>.Failure(e.Message);
            }
        }
    }
}