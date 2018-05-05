using Borg.CMS.Components.Contracts;
using Borg.Infra.DTO;
using System;

namespace Borg.Platform.EStore.Contracts
{
    public interface IProductAttributeType<out TKey> : IAmComponent<TKey>, IHaveComponentKey, IMultilibgualContent, IHaveTitle, IWeighted where TKey : IEquatable<TKey>
    {
    }
}