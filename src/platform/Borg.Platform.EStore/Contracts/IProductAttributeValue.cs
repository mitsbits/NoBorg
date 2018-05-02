using System;
using Borg.CMS.BackOfficeInstructions;
using Borg.CMS.Components.Contracts;

namespace Borg.Platform.EStore.Contracts
{
    public interface IProductAttributeValue<out TKey> : IAmComponent<TKey>, IMultilibgualContent where TKey : IEquatable<TKey>
    {
        [MultilingualProperty]
        IProductAttributeType<TKey> AttributeType { get; }

        [UnilingualProperty]
        string SKU { get; }

        [MultilingualProperty]
        string Value { get; }
    }
}