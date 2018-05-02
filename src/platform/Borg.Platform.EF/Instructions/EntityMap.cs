using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.Instructions
{
    public interface IEntityMap
    {
        void OnModelCreating(ModelBuilder builder);
    }

    public interface IEntityMap<TEntity, TDbContext> : IEntityMap where TEntity : class where TDbContext : DbContext
    {
    }

    public abstract class EntityMap<TEntity, TDbContext> : IEntityMap<TEntity, TDbContext> where TEntity : class where TDbContext : DbContext
    {
        public abstract void OnModelCreating(ModelBuilder builder);
    }
}