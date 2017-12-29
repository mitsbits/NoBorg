using Borg.Infra.DDD;
using System;

namespace Borg.Infra.Collections.Hierarchy
{
    public interface IHasParent<out TKey> : IHasParent, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey ParentId { get; }
    }

    public interface IHasParent
    {
        int Depth { get; }
    }
}