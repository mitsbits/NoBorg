using Borg.Infra.Collections;
using Borg.Infra.DAL;
using Borg.Platform.EF.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.EF.DAL
{
    public class ReadRepository<T, TDbContext> : IReadRepository<T> where T : class where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public ReadRepository(TDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            if (!_dbContext.EntityIsMapped<T, TDbContext>()) throw new EntityNotMappedException<TDbContext>(typeof(T));
        }

        protected TDbContext Context => _dbContext;

        public async Task<IPagedResult<T>> Find(Expression<Func<T, bool>> predicate, int page, int records, IEnumerable<OrderByInfo<T>> orderBy, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<T, dynamic>>[] paths)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dbContext.Fetch(predicate, page, records, orderBy, cancellationToken, false, paths);
        }
    }
}