using System;

namespace Borg.Infra.DDD
{
    public interface IHasCompositeKey<T> where T : IEquatable<T>
    {
        CompositeKey<T> CompositeKey { get; }
    }
}