using Borg.Infra.DTO;
using Borg.MVC.Services.Breadcrumbs;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface ICanRenderParentViewElements
    {
        Tidings Scripts { get; }
        Breadcrumbs Breadcrumbs { get; }
    }
}