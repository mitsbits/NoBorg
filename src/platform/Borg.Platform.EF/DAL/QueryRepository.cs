using Borg.Infra.Collections;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.EF.DAL
{
    public class QueryRepository<T, TDbContext> : IQueryRepository<T>, IHaveDbContext<TDbContext> where T : class where TDbContext : DbContext
    {

        private readonly TDbContext _dbContext;

        public QueryRepository(TDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public TDbContext Context => _dbContext;

        public async Task<IPagedResult<T>> Find(Expression<Func<T, bool>> predicate, int page, int records, IEnumerable<OrderByInfo<T>> orderBy, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<T, dynamic>>[] paths)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dbContext.Fetch(predicate, page, records, orderBy, cancellationToken, true, paths);
        }
    }

}
