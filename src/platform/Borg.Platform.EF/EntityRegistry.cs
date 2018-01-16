using System;
using Borg.Infra.DDD;
using Borg.Platform.EF.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF
{
    public abstract class EntityRegistry<TEntity> : IEntityRegistry where TEntity : IEntity
    {
        public abstract void RegisterWithDbContext(ModelBuilder builder);

    }
}