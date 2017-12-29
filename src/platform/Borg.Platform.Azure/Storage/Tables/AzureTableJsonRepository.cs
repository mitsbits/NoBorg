using Borg.Infra.DDD;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.Azure.Storage.Tables
{
    public class AzureTableJsonRepository<T> : IAzureTableStoreRepository<T> where T : IHasCompositeKey<string>, new()
    {
        internal readonly string _tableName = typeof(T).Name;
        private readonly IAzureTableRepository<TableJsonEntity<T>> _inner;

        public AzureTableJsonRepository(string connectionString, string tableName = "")
        {
            if (!string.IsNullOrWhiteSpace(tableName)) _tableName = tableName;
            _inner = new AzureTableRepository<TableJsonEntity<T>>(connectionString, _tableName);
        }

        public async Task<T> Create(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var created = await _inner.Create(new TableJsonEntity<T>(entity), cancellationToken);
            return created.Payload();
        }

        public async Task Delete(CompositeKey<string> key, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _inner.Delete(key, cancellationToken);
        }

        public async Task<IEnumerable<T>> Find(string predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            var found = await _inner.Find(predicate, cancellationToken);
            return found.Select(x => x.Payload());
        }

        public async Task<T> Get(CompositeKey<string> key, CancellationToken cancellationToken = default(CancellationToken))
        {
            var found = await _inner.Get(key, cancellationToken);
            return found.Payload();
        }
    }
}