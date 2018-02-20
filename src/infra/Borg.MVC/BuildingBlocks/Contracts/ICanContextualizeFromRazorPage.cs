using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface ICanContextualizeFromRazorPage : ICanContextualize
    {
        void Contextualize(PageModel page);
    }
}