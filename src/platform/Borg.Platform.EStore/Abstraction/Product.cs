using Borg.CMS.Components.Contracts;
using Borg.Infra.Storage.Assets.Contracts;
using Borg.Platform.EStore.Contracts;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EStore.Abstraction
{
    public abstract class Product<TKey> : IProduct<TKey>, IHaveASlug where TKey : IEquatable<TKey>
    {
        public virtual string ComponentKey => Component.Id.ToString();
        public abstract IComponent<TKey> Component { get; }
        public virtual string SKU { get; protected set; }
        public abstract IEnumerable<IPriceList> PriceLists { get; }
        public abstract IEnumerable<IProductAttributeValue<TKey>> Attributes { get; }
        public abstract ITaxonomy<TKey> Category { get; }
        public abstract IEnumerable<IAssetInfo<TKey>> Assets { get; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}