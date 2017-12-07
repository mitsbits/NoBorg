using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Messaging
{
    public interface IMessageSubscriber
    {
        void Subscribe<T>(Func<T, CancellationToken, Task> handler,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class;
    }
}