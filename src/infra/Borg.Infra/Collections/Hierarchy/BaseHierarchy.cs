using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infra.Collections.Hierarchy
{
    public abstract class BaseHierarchy<TKey> : IHierarchicalEnumerable where TKey : IEquatable<TKey>
    {
        private readonly ICollection<ITreeNode<TKey>> _source;

        protected BaseHierarchy()
        {
            _source = new HashSet<ITreeNode<TKey>>();
        }

        protected internal ICollection<ITreeNode<TKey>> Source => _source;

        public virtual IEnumerator GetEnumerator()
        {
            return _source.Where(x => x.Depth == _source.Min(s => s.Depth)).GetEnumerator();
        }

        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return enumeratedItem as IHierarchyData;
        }
    }
}