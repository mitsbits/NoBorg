using Borg.Infra.DAL;
using Borg.Infra.DDD.Contracts;
using System;

namespace Borg.Infra.DDD
{
    public abstract class EntityStateChangedEvent<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        protected EntityStateChangedEvent(TKey id, DmlOperation dmlOperation)
        {
            Id = id;
            DmlOperation = dmlOperation;
        }

        public TKey Id { get; }
        public DmlOperation DmlOperation { get; }
        public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
        public string Type => typeof(TEntity).FullName;
    }
}