using Borg.Cms.Basic.Lib.Features.CMS.Queries;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.PlugIns.Decoration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.Components
{
    [PulgInViewComponent("Html Snippet")]
    public class HtmlSnippetViewComponent : ViewComponentModule<Tidings>
    {
        private readonly ILogger _logger;
        private readonly IMediator _dispatcher;

        public HtmlSnippetViewComponent(ILoggerFactory loggerFactory, IMediator dispatcher)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _dispatcher = dispatcher;
        }

        public async Task<IViewComponentResult> InvokeAsync(Tidings tidings)
        {
            try
            {
                var key = tidings[Tidings.DefinedKeys.Key];
                var response = await _dispatcher.Send(new HtmlSnippetContentRequest(key));
                var model = response.Succeded ? response.Payload : string.Empty;
                if (tidings.ContainsKey(Tidings.DefinedKeys.View) &&
                    !string.IsNullOrWhiteSpace(tidings[Tidings.DefinedKeys.View]))
                    return View(tidings[Tidings.DefinedKeys.View], model);
                return View("", model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }
    }

    public sealed class HtmlSnippetModuleDescriptor : ViewComponentModuleDescriptor
    {
        public override string FriendlyName => "Html Snippet";
        public override string Summary => "Html Snippet Description";
        public override string ModuleGroup => "System.Content";

        protected override Tidings GetDefaults()
        {
            var result = new Tidings
            {
                new Tiding(Tidings.DefinedKeys.AssemblyQualifiedName, typeof(HtmlSnippetViewComponent).AssemblyQualifiedName),
                new Tiding(Tidings.DefinedKeys.Key, ""),
                new Tiding(Tidings.DefinedKeys.View, "")
            };
            return result;
        }
    }
}