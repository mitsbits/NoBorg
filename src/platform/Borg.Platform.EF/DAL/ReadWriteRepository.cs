using Borg.Infra.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.EF.DAL
{
    public class ReadWriteRepository<T, TDbContext> : ReadRepository<T, TDbContext>, IWriteRepository<T>, IReadWriteRepository<T> where T : class where TDbContext : DbContext
    {
        public ReadWriteRepository(TDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<T> Create(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task Delete(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var hits = await Context.Set<T>().Where(predicate).ToListAsync(cancellationToken: cancellationToken);
            if (!hits.Any()) return;
            await Task.WhenAll(hits.Select(x => Delete(x, cancellationToken)).ToList());
        }

        public Task Delete(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task<T> Update(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = Context.Update(entity).Entity;
            return Task.FromResult(result);
        }
    }
}