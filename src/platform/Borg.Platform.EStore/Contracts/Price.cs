using System;

namespace Borg.Platform.EStore.Contracts
{
    public abstract class Price<TKey> : IPrice<TKey> where TKey : IEquatable<TKey>
    {
        public abstract TKey Id { get; set; }
    }
}