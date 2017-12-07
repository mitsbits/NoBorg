using Borg.Infra.DAL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.Azure.Cosmos
{
    public interface IDocumentDbRepository<T> : IRepository<T> where T : class
    {
        Task<T> Get(string id, CancellationToken cancellationToken = default(CancellationToken));

        Task<T> Create(T entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<T> Update(string id, T entity, CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(string id, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate/*, int page, int records, IEnumerable<OrderByInfo<T>> orderBy*/, CancellationToken cancellationToken = default(CancellationToken));
    }
}