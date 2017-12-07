using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Messaging
{
    public interface IDispatcherInstance : IDisposable
    {
        Task Stop(CancellationToken token = default(CancellationToken));
    }
}