using Borg.Infra.DDD.Contracts;
using System;

namespace Borg.Platform.EStore.Contracts
{
    public interface IPrice<out TKey> : IEntity<TKey>, IPrice where TKey : IEquatable<TKey>
    {
    }

    public interface IPrice
    {
        string SKU { get; }

        decimal StrikeOutPrice { get; }

        decimal CataloguePrice { get; }

        decimal DiscountPrice { get; }

        decimal FinalPrice { get; }
    }
}