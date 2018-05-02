using Borg.Infra.DDD.Contracts;
using System;

namespace Borg.Infra.Collections.Hierarchy
{
    public interface ITreeNode<out TKey> : ITreeNode, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey ParentId { get; }
        TKey[] HierarchyKeys { get; }
    }

    public interface ITreeNode
    {
        int Depth { get; }
    }
}