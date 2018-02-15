using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Extensions;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.TagHelpers
{
    [HtmlTargetElement("page-title")]
    [PulgInTagHelper("Page Title")]
    public sealed class PageTitleTagHelper : TagHelper
    {
        private readonly IPageOrchestrator<IPageContent, IDevice> _orchestrator;

        public PageTitleTagHelper(IPageOrchestrator<IPageContent, IDevice> orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _orchestrator.TryContextualize(ViewContext);

            if (!_orchestrator.Page.Metas.Any())
            {
                output.SuppressOutput();
            }
            else
            {
                output.Attributes.Clear();
                var tag = new TagBuilder("title");
                tag.InnerHtml.Append(_orchestrator.Page.Title);
                output.TagName = "";
            }
        }
    }
}