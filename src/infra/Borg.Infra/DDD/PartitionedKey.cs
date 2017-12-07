using System;

namespace Borg.Infra.DDD
{
    public class PartitionedKey<T> : ValueObject<PartitionedKey<T>> where T : IEquatable<T>
    {
        internal PartitionedKey(T partition, T row)
        {
            Partition = partition;
            Row = row;
        }

        public T Partition { get; }
        public T Row { get; }

        public static PartitionedKey<T> Create(T partition, T row)
        {
            return new PartitionedKey<T>(partition, row);
        }

        public override string ToString()
        {
            return $"P:{Partition}|R:{Row}";
        }
    }
}