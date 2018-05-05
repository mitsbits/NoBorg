using Borg.CMS.Components.Contracts;
using Borg.Platform.EStore.Contracts;
using System;

namespace Borg.Platform.EStore.Abstraction
{
    public abstract class ProductAttributeType<TKey> : IProductAttributeType<TKey> where TKey : IEquatable<TKey>
    {
        public string LanguageCode { get; protected set; }

        public string Title { get; protected set; }

        public virtual double Weight { get; protected set; }

        public virtual string ComponentKey => Component.Id.ToString();
        public IComponent<TKey> Component { get; }
    }
}