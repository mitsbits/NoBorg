using System;
using Borg.CMS.BackOfficeInstructions;
using Borg.Infra.DDD.Contracts;

namespace Borg.Platform.EStore.Contracts
{
    public interface IPrice<out TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        
    }
    public interface IPrice
    {
        [UnilingualProperty]
        string SKU { get; }

        [UnilingualProperty]
        decimal StrikeOutPrice { get; }

        [UnilingualProperty]
        decimal CataloguePrice { get; }

        [UnilingualProperty]
        decimal DiscountPrice { get; }

        [UnilingualProperty]
        decimal FinalPrice { get; }

        [UnilingualProperty]
        string DiplayFormat { get; set; }
    }
}