using Microsoft.AspNetCore.Mvc.Rendering;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface ICanContextualizeFromView : ICanContextualize
    {
        void Contextualize(ViewContext context);
    }
}