using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Presentation.Areas.Presentation.Controllers;
using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Borg.Cms.Basic.Presentation.ActionFilters
{
    public class PopulateComponentFromRouteActionFilter : ActionFilterAttribute
    {
        public string RouteKey { get; set; } = "component";
        public override async Task OnActionExecutionAsync(  ActionExecutingContext context,  ActionExecutionDelegate next)
        {
            bool populated = false;
            if (context.RouteData.Values.TryGetValue(RouteKey, out var value))
            {
                if (value is ComponentPageDescriptor<int> descritor)
                {
                    if (context.Controller is PresentationController controller)
                    {
                        controller.SetContent(descritor.PageContent);
                        controller.PageDevice(descritor);
                        populated = true;
                    }
                }
            }
            if(!populated) throw new InvalidOperationException("no component descriptor for this action");
            // do something before the action executes
            var resultContext = await next();
            // do something after the action executes; resultContext.Result will be set
        }
    }
}
