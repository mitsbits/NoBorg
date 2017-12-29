using System;

namespace Borg.Infra.DDD
{
    public class CompositeKey<T> : ValueObject<CompositeKey<T>> where T : IEquatable<T>
    {
        internal CompositeKey(T partition, T row)
        {
            Partition = partition;
            Row = row;
        }

        public T Partition { get; }
        public T Row { get; }

        public static CompositeKey<T> Create(T partition, T row)
        {
            return new CompositeKey<T>(partition, row);
        }

        public override string ToString()
        {
            return $"P:{Partition}|R:{Row}";
        }
    }
}