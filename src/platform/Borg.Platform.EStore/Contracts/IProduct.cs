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

    public abstract class Product<TKey> : IProduct<TKey> where TKey : IEquatable<TKey>
    {
        public virtual string ComponentKey => Component.Id.ToString();
        public abstract IComponent<TKey> Component { get; }
        public virtual string SKU { get; protected set; }
        public abstract IEnumerable<IPriceList> PriceLists { get; }
        public abstract IEnumerable<IProductAttributeValue<TKey>> Attributes { get; }
        public abstract ITaxonomy<TKey> Category { get; }
        public abstract IEnumerable<IAssetInfo<TKey>> Assets { get; }
    }
}