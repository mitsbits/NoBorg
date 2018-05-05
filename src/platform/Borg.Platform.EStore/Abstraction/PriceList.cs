using Borg.CMS.BackOfficeInstructions;
using Borg.CMS.Components.Contracts;
using Borg.Infra.DDD.Contracts;
using Borg.Platform.EStore.Contracts;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EStore.Abstraction
{
    public abstract class PriceList<TKey> : IPriceList, IAmComponent<TKey>, IHaveComponentKey, IEntity<TKey>,  IHaveTitle where TKey : IEquatable<TKey>
    {
        [UnilingualProperty]
        public abstract IComponent<TKey> Component { get; }

        [UnilingualProperty]
        public virtual string ComponentKey => Component.Id.ToString();

        [UnilingualProperty]
        public TKey Id { get; set; }


        [MultilingualProperty]
        public string Title { get; set; }

        [UnilingualProperty]
        public string Code { get; set; }

        public abstract IEnumerable<IPrice> Prices { get; }

        public virtual string FriendlyName { get; set; }
    }
}