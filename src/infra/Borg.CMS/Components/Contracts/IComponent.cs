using Borg.Infra.DDD.Contracts;
using System;

namespace Borg.CMS.Components.Contracts
{
    public interface IComponent<out TKey> : IEntity<TKey>, ICanBeDeleted, ICanBePublished where TKey : IEquatable<TKey>
    {
    }
}