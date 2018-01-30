using Borg.Infra.Collections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.DAL
{
    public static class RepositoryExtensions
    {
        public static async Task<IPagedResult<T>> Find<T>(this IQueryRepository<T> repo,
            Expression<Func<T, bool>> predicate, IEnumerable<OrderByInfo<T>> orderBy,
            CancellationToken cancellationToken = default(CancellationToken),
            params Expression<Func<T, dynamic>>[] paths) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await repo.Find(predicate, -1, -1, orderBy, cancellationToken, paths);
        }

        public static async Task<T> Get<T>(this IQueryRepository<T> repo, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken),
            params Expression<Func<T, dynamic>>[] paths) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            var data = await repo.Find(predicate, 1, 1, null, cancellationToken, paths);
            return data.TotalRecords > 0 ? data.Records[0] : null;
        }

        public static async Task<IPagedResult<T>> Find<T>(this IReadRepository<T> repo,
            Expression<Func<T, bool>> predicate, IEnumerable<OrderByInfo<T>> orderBy,
            CancellationToken cancellationToken = default(CancellationToken),
            params Expression<Func<T, dynamic>>[] paths) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await repo.Find(predicate, -1, -1, null, cancellationToken, paths);
        }

        public static async Task<T> Get<T>(this IReadRepository<T> repo, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken),
            params Expression<Func<T, dynamic>>[] paths) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            var data = await repo.Find(predicate, 1, 1, null, cancellationToken, paths);
            return data.TotalRecords > 0 ? data.Records[0] : null;
        }

        public static async Task<bool> Exists<T>(this IQueryRepository<T> repo, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            var data = await repo.Find(predicate, 1, 1, null, cancellationToken);
            return data.TotalRecords > 0 ;
        }
    }
}