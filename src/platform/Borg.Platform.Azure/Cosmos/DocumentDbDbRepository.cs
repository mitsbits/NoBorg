using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.Azure.Cosmos
{
    public class DocumentDbRepository<T> : IDocumentDbRepository<T> where T : class
    {
        private readonly ILogger _logger;
        internal readonly DocDbConfig _settings;
        private DocumentClient _client;

        private Uri UriForCollection() => UriFactory.CreateDocumentCollectionUri(_settings.Database, _settings.Collection);

        public DocumentDbRepository(ILoggerFactory loggerfactory, DocDbConfig settings)
        {
            _logger = loggerfactory.CreateLogger(GetType());
            _settings = settings;

            if (string.IsNullOrWhiteSpace(_settings.Collection)) _settings.Collection = typeof(T).Name;
            Initialize();
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var watch = Stopwatch.StartNew();
            _logger.Debug("Fetching from {collection} for clause: {@prrdicate} EL:{watch}", _settings.Collection, predicate.Body, watch.Elapsed);
            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
                    UriForCollection(),
                    new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>(cancellationToken));
            }
            _logger.Debug("{Query} fetched {count} rows from {collection} for clause: {@prdicate} EL:{watch}", query.ToString(), results.Count, _settings.Collection, predicate, watch.Elapsed);
            watch.Stop();
            return results;
        }

        public async Task<T> Get(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_settings.Database, _settings.Collection, id));
            return (T)(dynamic)response.Resource;
        }

        public async Task<T> Create(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            Document created = await _client.CreateDocumentAsync(UriForCollection(), entity);
            return (T)(dynamic)created;
        }

        public async Task<T> Update(string id, T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_settings.Database, _settings.Collection, id), entity);
            return (T)(dynamic)response;
        }

        public async Task Delete(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = _client.CreateDocumentQuery<T>(
                    UriForCollection(),
                    new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            var tasks = new List<Task>();
            while (query.HasMoreResults)
            {
                var local = await query.ExecuteNextAsync(cancellationToken);
                foreach (Document item in local)
                {
                    var id = item.Id;
                    tasks.Add(_client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_settings.Database, _settings.Collection, id)));
                }
            }
            await Task.WhenAll(tasks).AnyContext();
        }

        public async Task Delete(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_settings.Database, _settings.Collection, id));
        }

        private void Initialize()
        {
            _client = new DocumentClient(new Uri(_settings.Endpoint), _settings.AuthKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await _client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_settings.Database));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateDatabaseAsync(new Database { Id = _settings.Database });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_settings.Database, _settings.Collection));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_settings.Database),
                        new DocumentCollection { Id = _settings.Collection },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}