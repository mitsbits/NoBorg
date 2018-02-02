using Borg.Infra.Collections.Hierarchy;
using System;

namespace Borg.Infra.DTO
{
    public class Step : Tiding, IHierarchyData
    {
        public Step(string key, string value = "") : base(key, value)
        {
        }

        #region IHierarchyData

        public bool HasChildren => Children.Count > 0;
        public object Item => this;
        public virtual string Tag => GetType().FullName;

        IHierarchicalEnumerable IHierarchyData.Children => Children as Steps;

        public IHierarchyData Parent { get; protected internal set; }

        void IHierarchyData.AddChild(IHierarchyData child)
        {
            AddChild(child as Step);
        }

        #endregion IHierarchyData

        public virtual void AddChild(Step child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            child.Parent = this;
            Children.Add(child);
        }
    }
}