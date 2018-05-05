using Borg.CMS.Components.Contracts;
using System.Collections.Generic;

namespace Borg.Platform.EStore.Contracts
{
    public interface IPriceList : IHaveAFriendlyName, IHaveACode
    {
        IEnumerable<IPrice> Prices { get; }
    }
}