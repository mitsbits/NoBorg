using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        Task Save(CancellationToken cancelationToken = default(CancellationToken));
    }
}