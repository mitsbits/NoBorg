using System;
using Borg.CMS.BackOfficeInstructions;
using Borg.CMS.Components.Contracts;
using Borg.Infra.DDD.Contracts;

namespace Borg.Platform.EStore.Contracts
{
    public abstract class PriceList<TKey> : IAmComponent<TKey>, IHaveComponentKey, IEntity<TKey>, IMultilibgualContent, IHaveTitle, IHaveACode where TKey : IEquatable<TKey>
    {
        [UnilingualProperty]
        public abstract IComponent<TKey> Component { get; }

        [UnilingualProperty]
        public virtual string ComponentKey => Component.Id.ToString();

        [UnilingualProperty]
        public TKey Id { get; set; }

        [MultilingualProperty]
        public string LanguageCode { get; set; }

        [MultilingualProperty]
        public string Title { get; set; }

        [UnilingualProperty]
        public string Code { get; set; }
    }
}