using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Extensions;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.Cms.Basic.Presentation.Areas.Presentation.TagHelpers
{
    [HtmlTargetElement("page-title")]
    [PulgInTagHelper("Page Title")]
    public sealed class HeadPageTitleTagHelper : TagHelper
    {
        private readonly IPageOrchestrator<IPageContent, IDevice> _orchestrator;

        public HeadPageTitleTagHelper(IPageOrchestrator<IPageContent, IDevice> orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _orchestrator.TryContextualize(ViewContext);

            output.Attributes.Clear();
            output.TagName = "title";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.Append(_orchestrator.Page.Title);
        }
    }
}