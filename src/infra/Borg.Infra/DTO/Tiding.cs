using System;

namespace Borg.Infra.DTO
{
    public class Tiding : Catalogued, IWeighted, ICloneable, ICloneable<Tiding>
    {
        public Tiding(string key, string value = "")
        {
            Key = key;
            Value = value;
        }

        public virtual Tidings Children { get; } = new Tidings();
        public virtual double Weight { get; set; } = 0;
        public virtual int Depth { get; set; } = 0;

        #region ICloneable

        object ICloneable.Clone()
        {
            return Clone();
        }

        public Tiding Clone()
        {
            var clone = new Tiding(Key, Value) { Weight = Weight, Flag = Flag, Hint = Hint, HumanKey = HumanKey, Depth = Depth };
            foreach (var tiding in Children)
                clone.Children.Add(tiding.Clone());
            return clone;
        }

        #endregion ICloneable
    }
}