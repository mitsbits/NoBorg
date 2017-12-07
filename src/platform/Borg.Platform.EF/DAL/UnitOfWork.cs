using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;
using Borg.Platform.EF.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.EF.DAL
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        public UnitOfWork(TDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TDbContext Context => _context;

        public IQueryRepository<T> QueryRepo<T>() where T : class
        {
            return Context.QueryRepo<T, TDbContext>();
        }

        public IReadWriteRepository<T> ReadWriteRepo<T>() where T : class
        {
            return Context.ReadWriteRepo<T, TDbContext>();
        }

        public async Task Save(CancellationToken cancelationToken = default(CancellationToken))
        {
            try
            {
                await Context.SaveChangesAsync(cancelationToken);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                throw new ConcurrentModificationException(
                    "The record you attempted to edit was modified by another " +
                    "user after you loaded it. The edit operation was cancelled and the " +
                    "currect values in the database are displayed. Please try again.", exception);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}