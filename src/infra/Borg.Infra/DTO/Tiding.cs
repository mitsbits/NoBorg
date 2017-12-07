using Borg.Infra.Collections.Hierarchy;
using System;

namespace Borg.Infra.DTO
{
    public class Tiding : Catalogued, IWeighted, ICloneable, ICloneable<Tiding>, IHierarchyData
    {
        private IHierarchicalEnumerable _children;

        public Tiding(string key, string value = "")
        {
            Key = key;
            Value = value;
        }

        public virtual Tidings Children { get; } = new Tidings();
        public virtual double Weight { get; set; }

        public virtual void AddChild(Tiding child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            child.Parent = this;
            Children.Add(child);
        }

        #region IHierarchyData

        public bool HasChildren => Children.Count > 0;
        public object Item => this;
        public virtual string Tag => GetType().FullName;

        IHierarchicalEnumerable IHierarchyData.Children => Children;

        public IHierarchyData Parent { get; protected internal set; }

        void IHierarchyData.AddChild(IHierarchyData child)
        {
            AddChild(child as Tiding);
        }

        #endregion IHierarchyData

        #region ICloneable

        object ICloneable.Clone()
        {
            return Clone();
        }

        public Tiding Clone()
        {
            var clone = new Tiding(Key, Value) { Weight = Weight, Flag = Flag, Hint = Hint, HumanKey = HumanKey };
            foreach (var tiding in Children)
                clone.Children.Add(tiding.Clone());
            return clone;
        }
    }

    #endregion ICloneable
}