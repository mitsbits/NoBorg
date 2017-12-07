using Borg.Infra.DAL;
using Borg.Infra.DDD;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.Azure.Storage.Tables
{
    public interface IAzureTableStoreRepository<T> : IRepository<T> where T : IHasPartitionKey<string>
    {
        Task<T> Create(T entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<T> Get(PartitionedKey<string> key, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<T>> Find(string predicate, CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(PartitionedKey<string> key, CancellationToken cancellationToken = default(CancellationToken));
    }
}