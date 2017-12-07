using Borg.Infra.DAL;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.Contracts
{
    public interface IUnitOfWork<out TDbContext> : IUnitOfWork, IHaveDbContext<TDbContext> where TDbContext : DbContext
    {
        IQueryRepository<T> QueryRepo<T>() where T : class;
        IReadWriteRepository<T> ReadWriteRepo<T>() where T : class;
    }
}
