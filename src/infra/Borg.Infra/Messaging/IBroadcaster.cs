using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Messaging
{
    public interface IBroadcaster
    {
        Task Broadcast(string[] topics, Type messageType, object message, TimeSpan? delay = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}