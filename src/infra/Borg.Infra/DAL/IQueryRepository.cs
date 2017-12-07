using Borg.Infra.Collections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.DAL
{
    public interface IQueryRepository<T> : IRepository<T> where T : class
    {
        Task<IPagedResult<T>> Find(Expression<Func<T, bool>> predicate, int page, int records,
            IEnumerable<OrderByInfo<T>> orderBy, CancellationToken cancellationToken = default(CancellationToken),
            params Expression<Func<T, dynamic>>[] paths);
    }
}