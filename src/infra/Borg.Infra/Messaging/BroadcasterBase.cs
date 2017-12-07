using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Messaging
{
    public abstract class BroadcasterBase : IBroadcaster
    {
        protected readonly IEnumerable<IMessagePublisher> _publishers;

        protected BroadcasterBase(ILoggerFactory loggerFactory, IEnumerable<IMessagePublisher> publishers)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            _publishers = publishers ?? throw new ArgumentNullException(nameof(publishers));
        }

        protected ILogger Logger { get; }

        protected IEnumerable<IMessagePublisher> Publishers => _publishers;

        public virtual Task Broadcast(string[] topics, Type messageType, object message,
            TimeSpan? delay = default(TimeSpan?), CancellationToken cancellationToken = default(CancellationToken))
        {
            var broadcastEmpty = !topics.Any();
            var broadcastAll = !broadcastEmpty &&
                               (topics.Length == 1) & topics[0].Equals("ALL", StringComparison.OrdinalIgnoreCase);
            var sanitized = topics.Select(x => x.Trim()).Distinct().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var broadcastTopics = sanitized.Any();

            List<Task> tasks = null;

            Logger.LogDebug("Broadcasting {@message} to {topics} with {delay} on {publishers}", message, topics, delay,
                _publishers);

            if (broadcastTopics)
            {
                tasks = new List<Task>(
                    _publishers.Where(x => x.SupportsTopics)
                        .Cast<ITopicPublisher>()
                        .Where(x => sanitized.Contains(x.Topic.ToLower()))
                        .Select(x => x.PublishAsync(messageType, message, delay, cancellationToken)));
            }
            else
            {
                if (broadcastAll)
                {
                    tasks = new List<Task>(
                        _publishers
                            .Select(x => x.PublishAsync(messageType, message, delay, cancellationToken)));
                }
                else
                {
                    if (broadcastEmpty)
                        tasks = new List<Task>(
                            _publishers.Where(x => !x.SupportsTopics)
                                .Select(x => x.PublishAsync(messageType, message, delay, cancellationToken)));
                }
            }

            return tasks != null && tasks.Any() ? Task.WhenAll(tasks) : Task.CompletedTask;
        }
    }
}