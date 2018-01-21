using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Caching.Contracts
{
    public interface ICacheStore
    {
        Task<T> Get<T>(string key, CancellationToken token = default(CancellationToken));

        Task Set<T>(string key, T instance, CancellationToken token = default(CancellationToken));

        Task SetAbsolute<T>(string key, T instance, DateTimeOffset absoluteExpiration, CancellationToken token = default(CancellationToken));

        Task SetSliding<T>(string key, T instance, TimeSpan slidingExpiration, CancellationToken token = default(CancellationToken));

        Task Remove(string key, CancellationToken token = default(CancellationToken));

        Task Refresh(string key, CancellationToken token = default(CancellationToken));
    }
}