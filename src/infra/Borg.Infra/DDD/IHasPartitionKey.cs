using System;

namespace Borg.Infra.DDD
{
    public interface IHasPartitionKey<T> where T : IEquatable<T>
    {
        PartitionedKey<T> PartitionedKey { get; }
    }
}