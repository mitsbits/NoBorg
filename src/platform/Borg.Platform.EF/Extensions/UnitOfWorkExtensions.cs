using Borg.Infra.Collections;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg
{
    public static class UnitOfWorkExtensions
    {
        public static async Task<IPagedResult<T>> Fetch<T, TDbContext>(this IUnitOfWork<TDbContext> uow,
            Expression<Func<T, bool>> predicate, int page, int size, IEnumerable<OrderByInfo<T>> orderBy = null,
            CancellationToken cancellationToken = default(CancellationToken), bool readOnly = false,
            Expression<Func<T, dynamic>>[] paths = null) where T : class where TDbContext : DbContext
        {
            return await uow.Context.Fetch(predicate, page, size, orderBy, cancellationToken, readOnly, paths);
        }

        public static async Task<IPagedResult<T>> Fetch<T, TDbContext>(this IUnitOfWork<TDbContext> uow,
            Expression<Func<T, bool>> predicate, IEnumerable<OrderByInfo<T>> orderBy = null,
            CancellationToken cancellationToken = default(CancellationToken), bool readOnly = false,
            Expression<Func<T, dynamic>>[] paths = null) where T : class where TDbContext : DbContext
        {
            return await uow.Context.Fetch(predicate, -1, -1, orderBy, cancellationToken, readOnly, paths);
        }
    }
}