using Microsoft.AspNetCore.Routing;

namespace Borg.MVC.PlugIns.Contracts
{
    public interface IRouteRegistry
    {
        void Register(IRouteBuilder builder);
    }
}