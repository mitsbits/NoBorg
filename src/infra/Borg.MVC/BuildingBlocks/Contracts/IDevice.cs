using Borg.Infra.DTO;
using Borg.MVC.Services.Breadcrumbs;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IDevice : ICanContextualize
    {
        string FriendlyName { get; }
        string Layout { get; set; }
        string Path { get; }
        string QueryString { get; }
        string Domain { get; }
        string Area { get; }
        string Controller { get; }
        string Action { get; }

        Tidings Scripts { get; }
        Breadcrumbs Breadcrumbs { get; }
    }
}