using Borg.Infra.Caching.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Caching
{
    public class CacheStore : ICacheStore
    {
        private readonly ILogger _logger;
        private readonly IDistributedCache _cache;
        private readonly ISerializer _serializer;

        public CacheStore(IDistributedCache cache, ISerializer serializer, ILoggerFactory loggerFactory = null)
        {
            _logger = (loggerFactory == null) ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            Preconditions.NotNull(cache, nameof(cache));
            Preconditions.NotNull(serializer, nameof(serializer));
            _cache = cache;
            _serializer = serializer;
        }

        #region ICacheStore

        public async Task<T> Get<T>(string key, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            Preconditions.NotEmpty(key, nameof(key));
            var hit = await _cache.GetAsync(key, token);
            if (hit != null && hit.Length > 0)
            {
                return await _serializer.DeserializeAsync<T>(hit);
            }
            return default(T);
        }

        public async Task Set<T>(string key, T instance, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            Preconditions.NotEmpty(key, nameof(key));
            var bytes = await _serializer.SerializeAsync(instance);
            await _cache.SetAsync(key, bytes, token);
        }

        public async Task SetAbsolute<T>(string key, T instance, DateTimeOffset absoluteExpiration,
            CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            Preconditions.NotEmpty(key, nameof(key));
            var bytes = await _serializer.SerializeAsync(instance);
            await _cache.SetAsync(key, bytes, new DistributedCacheEntryOptions { AbsoluteExpiration = absoluteExpiration }, token);
        }

        public async Task SetSliding<T>(string key, T instance, TimeSpan slidingExpiration,
            CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            Preconditions.NotEmpty(key, nameof(key));
            var bytes = await _serializer.SerializeAsync(instance);
            await _cache.SetAsync(key, bytes, new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration }, token);
        }

        public async Task Remove(string key, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            Preconditions.NotEmpty(key, nameof(key));
            await _cache.RemoveAsync(key, token);
        }

        public async Task Refresh(string key, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            Preconditions.NotEmpty(key, nameof(key));
            await _cache.RefreshAsync(key, token);
        }

        #endregion ICacheStore
    }
}