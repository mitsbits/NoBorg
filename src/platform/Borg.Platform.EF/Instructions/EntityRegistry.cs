using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.Instructions
{
    public abstract class EntityRegistry<TEntity> : IEntityRegistry where TEntity : IEntity
    {
        public abstract void RegisterWithDbContext(ModelBuilder builder);
    }
}