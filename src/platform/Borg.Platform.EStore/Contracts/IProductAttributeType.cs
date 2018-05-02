using System;
using Borg.CMS.Components.Contracts;
using Borg.Infra.DTO;

namespace Borg.Platform.EStore.Contracts
{
    public interface IProductAttributeType<out TKey> : IAmComponent<TKey>, IMultilibgualContent, IHaveTitle, IWeighted where TKey : IEquatable<TKey>
    {
    }
}