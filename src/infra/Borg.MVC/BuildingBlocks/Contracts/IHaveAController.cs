using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IHaveAController
    {
        string Area { get; }
        string Controller { get; }
        string Action { get; }
        IDictionary<string, string> RouteValues { get; }
    }
}