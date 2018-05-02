using Borg.CMS.Components.Contracts;
using System;
using System.Collections.Generic;
using Borg.Infra.Storage.Assets.Contracts;

namespace Borg.Platform.EStore.Contracts
{
    public interface IProduct<out TKey> : IHaveComponentKey, IAmComponent<TKey> where TKey : IEquatable<TKey>
    {
        string SKU { get; }
        IEnumerable<IPriceList> PriceLists { get; }
        IEnumerable<IProductAttributeValue<TKey>> Attributes { get; }
        ITaxonomy<TKey> Category { get; }
        IEnumerable<IAssetInfo<TKey>> Assets { get; }
    }
}