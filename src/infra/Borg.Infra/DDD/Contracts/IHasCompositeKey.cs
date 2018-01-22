using System;

namespace Borg.Infra.DDD.Contracts
{
    public interface IHasCompositeKey<T> where T : IEquatable<T>
    {
        CompositeKey<T> CompositeKey { get; }
    }
}