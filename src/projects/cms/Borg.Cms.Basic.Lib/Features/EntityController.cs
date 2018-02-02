using Borg.Infra.DDD.Contracts;
using Borg.MVC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Borg.Cms.Basic.Lib.Features
{
    public abstract class EntityController<TEntity, TKey, TDbContext> : BorgController where TEntity : IEntity<TKey> where TKey : IEquatable<TKey> where TDbContext : DbContext
    {
        protected EntityController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}