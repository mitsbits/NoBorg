using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Messaging
{
    public static class MessagingExtensions
    {
        private const string ALL = "ALL";

        public static Task BroadcastTopics<T>(this IBroadcaster broadcaster, string[] topics, T message,
            TimeSpan? delay = null) where T : class
        {
            if (topics == null || !topics.Distinct().Any()) throw new ArgumentNullException(nameof(topics));
            if (topics.Length == 1 && topics[0].Equals(ALL, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException(nameof(topics));
            return broadcaster.Broadcast(topics, typeof(T), message, delay);
        }

        public static Task BroadcastMessage<T>(this IBroadcaster broadcaster, T message, TimeSpan? delay = null)
            where T : class
        {
            return broadcaster.Broadcast(new string[0], typeof(T), message, delay);
        }

        public static Task BroadcastAll<T>(this IBroadcaster broadcaster, T message, TimeSpan? delay = null)
            where T : class
        {
            return broadcaster.Broadcast(new[] { ALL }, typeof(T), message, delay);
        }

        public static Task PublishAsync<T>(this IMessagePublisher publisher, T message, TimeSpan? delay = null)
            where T : class
        {
            return publisher.PublishAsync(typeof(T), message, delay);
        }

        public static void Subscribe<T>(this IMessageSubscriber subscriber, Func<T, Task> handler,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            subscriber.Subscribe<T>((msg, token) => handler(msg), cancellationToken);
        }

        public static void Subscribe<T>(this IMessageSubscriber subscriber, Action<T> handler,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            subscriber.Subscribe<T>((msg, token) =>
            {
                handler(msg);
                return Task.CompletedTask;
            }, cancellationToken);
        }
    }
}