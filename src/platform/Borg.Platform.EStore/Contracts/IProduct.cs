using Borg.CMS.Components.Contracts;
using Borg.Infra.Storage.Assets.Contracts;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EStore.Contracts
{
    public interface IProduct<out TKey> : IHaveComponentKey, IAmComponent<TKey>, IHaveTitle, IHaveADescription where TKey : IEquatable<TKey>
    {
        string SKU { get; }
        IEnumerable<IPriceList> PriceLists { get; }
        IEnumerable<IProductAttributeValue<TKey>> Attributes { get; }
        ITaxonomy<TKey> Category { get; }
        IEnumerable<IAssetInfo<TKey>> Assets { get; }
    }
}