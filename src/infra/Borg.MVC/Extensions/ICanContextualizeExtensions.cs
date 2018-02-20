using Borg.Infra;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Borg.MVC.Extensions
{
    public static class ICanContextualizeExtensions
    {
        public static bool TryContextualize(this ICanContextualize service, Controller controller)
        {
            if (!(service is ICanContextualizeFromController srv)) return false;
            Preconditions.NotNull(controller, nameof(controller));
            srv.Contextualize(controller);
            return true;
        }

        public static bool TryContextualize(this ICanContextualize service, ViewContext context)
        {
            if (!(service is ICanContextualizeFromView srv)) return false;
            Preconditions.NotNull(context, nameof(context));
            srv.Contextualize(context);
            return true;
        }

        public static bool TryContextualize(this ICanContextualize service, PageModel page)
        {
            if (!(service is ICanContextualizeFromRazorPage srv)) return false;
            Preconditions.NotNull(page, nameof(page));
            srv.Contextualize(page);
            return true;
        }
    }
}