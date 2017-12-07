using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Borg.MVC.Extensions
{
    public static class ICanContextualizeExtensions
    {
        public static bool TryContextualize(this ICanContextualize service, Controller controller)
        {
            if (!(service is ICanContextualizeFromController srv)) return false;
            srv.Contextualize(controller);
            return true;
        }

        public static bool TryContextualize(this ICanContextualize service, ViewContext context)
        {
            if (!(service is ICanContextualizeFromView srv)) return false;
            srv.Contextualize(context);
            return true;
        }
    }
}