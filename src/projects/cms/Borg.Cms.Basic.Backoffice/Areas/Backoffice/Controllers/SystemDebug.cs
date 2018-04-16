using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Linq;
using Borg.Infra.Collections;
using Borg.MVC.Services.Editors;
using Newtonsoft.Json;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.Controllers
{
    [Route("[area]/SystemDebug")]
    public class SystemDebugController : BackofficeController
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public SystemDebugController(ILoggerFactory loggerFactory, IMediator dispatcher, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) : base(loggerFactory, dispatcher)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        [HttpGet]
        [HttpPut]
        public IActionResult Routes()
        {
            var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Select(x => new
            {
                Area = x.RouteValues["Area"],
                Action = x.RouteValues["Action"],
                Controller = x.RouteValues["Controller"],
                Name = x.AttributeRouteInfo?.Name,
                Template = x.AttributeRouteInfo?.Template,
                Contraint = x.ActionConstraints,
                Parameters = x.Parameters?.Select(p => new { p.Name, TypeName = p.ParameterType.Name }),
                DisplayName = x.DisplayName
            }).ToList();
            return Ok(routes);
        }
        [HttpGet("RoutesView")]
        public IActionResult RoutesView()
        {
            var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Select(x => new
            {
                Area = x.RouteValues["Area"],
                Action = x.RouteValues["Action"],
                Controller = x.RouteValues["Controller"],
                Name = x.AttributeRouteInfo?.Name,
                Template = x.AttributeRouteInfo?.Template,
                Contraint = x.ActionConstraints,
                Parameters = x.Parameters?.Select(p => new { p.Name, TypeName = p.ParameterType.Name }),
                DisplayName = x.DisplayName
            }).ToList();
            SetPageTitle("Routes");

            return View( new PagedResult<dynamic>(routes, 1, routes.Count, routes.Count));
        }
    }
}