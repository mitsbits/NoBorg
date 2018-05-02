using System.Collections.Generic;
using Borg.CMS.Components.Contracts;

namespace Borg.Platform.EStore.Contracts
{
    public interface IPriceList : IHaveAFriendlyName, IHaveACode
    {
        IEnumerable<IPrice> Prices { get; }
    }
}