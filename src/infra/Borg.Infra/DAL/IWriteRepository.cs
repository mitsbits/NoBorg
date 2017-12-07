using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.DAL
{
    public interface IWriteRepository<T> where T : class
    {
        Task<T> Create(T entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<T> Update(T entity, CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(T entity, CancellationToken cancellationToken = default(CancellationToken));
    }
}