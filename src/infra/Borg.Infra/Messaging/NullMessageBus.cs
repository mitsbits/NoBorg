using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Messaging
{
    public class NullMessageBus : ITopicPublisher
    {
        public static readonly NullMessageBus Instance = new NullMessageBus();

        public string Topic => string.Empty;

        public bool SupportsTopics => false;

        public Task PublishAsync(Type messageType, object message, TimeSpan? delay = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public void Subscribe<T>(Func<T, CancellationToken, Task> handler,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //release resources
            }
        }

        public Task Stop(CancellationToken token = new CancellationToken())
        {
            return Task.CompletedTask;
        }
    }
}