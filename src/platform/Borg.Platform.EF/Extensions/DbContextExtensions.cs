using Borg.Infra.Collections;
using Borg.Infra.DAL;
using Borg.Platform.EF.DAL;
using Borg.Platform.EF.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg
{
    public static class DbContextExtensions
    {
        public static async Task<IPagedResult<T>> Fetch<T, TDbContext>(this TDbContext db, Expression<Func<T, bool>> predicate, int page, int size, IEnumerable<OrderByInfo<T>> orderBy, CancellationToken cancellationToken = default(CancellationToken), bool noTracking = false, Expression<Func<T, dynamic>>[] paths = null) where T : class where TDbContext : DbContext
        {
            cancellationToken.ThrowIfCancellationRequested();
            var fetchAll = (page == -1 && size == -1);
            IPagedResult<T> result;

            var query = (noTracking) ? db.Set<T>().AsNoTracking().Where(predicate) : db.Set<T>().Where(predicate);
            var totalRecords = (fetchAll) ? 0 : await query.CountAsync(cancellationToken: cancellationToken);
            var totalPages = (fetchAll) ? 1 : (int)Math.Ceiling((double)totalRecords / size);
            if (page > totalPages)
            {
                page = totalPages;
            }
            if (!fetchAll && totalRecords == 0)
            {
                result = new PagedResult<T>(new List<T>(), page, size, 0);
            }
            else
            {
                if (paths != null && paths.Any())
                {
                    query = paths.Aggregate(query, (current, path) => current.Include(path));
                }

                if (orderBy != null && orderBy.Any())
                {
                    IOrderedQueryable<T> orderedQueryable = query.Apply(orderBy as OrderByInfo<T>[] ?? orderBy.ToArray());
                    query = orderedQueryable;
                }

                if (fetchAll)
                {
                    var data = await query.ToListAsync(cancellationToken: cancellationToken);
                    var count = data.Count();
                    result = new PagedResult<T>(data, 1, count, count);
                }
                else
                {
                    var data = await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken: cancellationToken);
                    result = new PagedResult<T>(data, page, size, totalRecords);
                }
            }
            return result;
        }

        public static IQueryRepository<T> QueryRepo<T, TDbContext>(this TDbContext db) where T : class where TDbContext : DbContext
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (db.EntityIsMapped<T, TDbContext>())
            {
                return new QueryRepository<T, TDbContext>(db);
            }
            throw new EntityNotMappedException(typeof(T));
        }

        public static IReadRepository<T> ReadRepo<T, TDbContext>(this TDbContext db) where T : class where TDbContext : DbContext
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (db.EntityIsMapped<T, TDbContext>())
            {
                return new ReadRepository<T, TDbContext>(db);
            }
            throw new EntityNotMappedException(typeof(T));
        }

        public static IReadWriteRepository<T> ReadWriteRepo<T, TDbContext>(this TDbContext db) where T : class where TDbContext : DbContext
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            return new ReadWriteRepository<T, TDbContext>(db);
        }

        public static bool EntityIsMapped<T, TDbContext>(this TDbContext db) where T : class where TDbContext : DbContext
        {
            return db.Model.GetEntityTypes(typeof(T)).Any();
        }
    }
}