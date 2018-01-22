using System;

namespace Borg.Infra.DDD.Contracts
{
    public interface IEntity<out TKey> : IEntity where TKey : IEquatable<TKey>
    {
        TKey Id { get; }
    }

    public interface IEntity
    {
    }

    public interface IAgregateRoot<out TKey> : IAgregateRoot, IEntity<TKey> where TKey : IEquatable<TKey>
    {
    }

    public interface IAgregateRoot
    {
        int Version { get; }
    }
}