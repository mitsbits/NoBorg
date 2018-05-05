using Borg.Platform.EStore.Contracts;
using System;

namespace Borg.Platform.EStore.Abstraction
{
    public abstract class Price<TKey> : IPrice<TKey> where TKey : IEquatable<TKey>
    {
        public abstract TKey Id { get; protected set; }
        public virtual string SKU { get; protected set; }
        public virtual decimal StrikeOutPrice { get; protected set; }
        public virtual decimal CataloguePrice { get; protected set; }
        public virtual decimal DiscountPrice { get; protected set; }
        public virtual decimal FinalPrice { get; protected set; }
    }
}