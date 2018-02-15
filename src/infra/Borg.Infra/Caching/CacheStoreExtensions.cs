using Borg.Infra.Caching.Contracts;
using System;
using System.Threading.Tasks;

namespace Borg
{
    public static class CacheStoreExtensions
    {/// <summary>
     ///
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="store"></param>
     /// <param name="key"></param>
     /// <param name="instance"></param>
     /// <param name="segments">seconds, minutes, hours, days</param>
     /// <returns></returns>
        public static async Task SetFor<T>(this ICacheStore store, string key, T instance, params int[] segments)
        {
            TimeSpan period;
            switch (segments.Length)
            {
                case 1:
                    period = TimeSpan.FromSeconds(segments[0]);
                    break;

                case 2:
                    period = new TimeSpan(0, segments[1], segments[0]);
                    break;

                case 3:
                    period = new TimeSpan(segments[2], segments[1], segments[0]);
                    break;

                case 4:
                    period = new TimeSpan(segments[3], segments[2], segments[1], segments[0]);
                    break;

                default: period = TimeSpan.Zero; break;
            }

            var expiration = DateTimeOffset.UtcNow.Add(period);
            await store.SetAbsolute<T>(key, instance, expiration);
        }

        /// <summary>
        /// forever is ten years from now
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="store"></param>
        /// <param name="key"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static async Task SetForEver<T>(this ICacheStore store, string key, T instance)
        {
            var expiration = DateTimeOffset.UtcNow.AddYears(10);
            await store.SetAbsolute<T>(key, instance, expiration);
        }
    }
}