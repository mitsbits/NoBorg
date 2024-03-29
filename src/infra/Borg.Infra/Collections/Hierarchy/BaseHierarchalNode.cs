using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infra.Collections.Hierarchy
{
    public abstract class BaseHierarchalNode<TKey> : ITreeNode<TKey>, IHierarchyData where TKey : IEquatable<TKey>
    {
        private readonly List<ITreeNode<TKey>> _source;

        public IHierarchicalEnumerable Container { get; set; }

        protected BaseHierarchalNode(IEnumerable<ITreeNode<TKey>> source)
        {
            _source = new List<ITreeNode<TKey>>(source);
        }

        protected BaseHierarchalNode(BaseHierarchy<TKey> container)
        {
            _source = new List<ITreeNode<TKey>>(container.Source);
            Container = container;
        }

        public TKey Id { get; protected set; } = default(TKey);
        public TKey ParentId { get; protected set; } = default(TKey);
        public TKey[] HierarchyKeys { get; }
        public int Depth { get; protected set; } = default(int);

        public bool HasChildren
        {
            get { return Source.Any(x => !x.ParentId.Equals(default(TKey)) && x.ParentId.Equals(Id)); }
        }

        public abstract object Item { get; }

        public virtual string Tag => GetType().FullName;

        public abstract IHierarchicalEnumerable Children { get; }

        public virtual IHierarchyData Parent
        {
            get { return Source.FirstOrDefault(x => x.Id.Equals(ParentId)) as IHierarchyData; }
        }

        public void AddChild(IHierarchyData child)
        {
            if (child is BaseHierarchalNode<TKey> item && !_source.Contains(item))
            {
                item.ParentId = Id;
                _source.Add(item);
            }
        }

        protected virtual IEnumerable<ITreeNode<TKey>> Source => _source;
    }
}