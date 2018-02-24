using System.Linq;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Extensions;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.MVC.TagHelpers
{
    [HtmlTargetElement("metas")]
    [PulgInTagHelper("Html Metas")]
    public sealed class HtmlMetaTagHelper : TagHelper
    {
        private readonly IPageOrchestrator<IPageContent, IDevice> _orchestrator;

        public HtmlMetaTagHelper(IPageOrchestrator<IPageContent, IDevice> orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ICanContextualizeExtensions.TryContextualize((ICanContextualize) _orchestrator, (ViewContext) ViewContext);
            if (!Enumerable.Any<HtmlMeta>(_orchestrator.Page.Metas))
            {
                output.SuppressOutput();
            }
            else
            {
                output.Attributes.Clear();

                foreach (var meta in _orchestrator.Page.Metas)
                {
                    if (meta.IsBasic)
                    {
                        var tag = new TagBuilder("meta");
                        tag.MergeAttribute("name", meta.Name);
                        tag.MergeAttribute("content", meta.Content);
                        output.Content.AppendHtml(tag.GetString());
                    }
                    if (meta.IsHttpEquiv)
                    {
                        var tag = new TagBuilder("meta");
                        tag.MergeAttribute("http-equiv", meta.HttpEquiv);
                        tag.MergeAttribute("content", meta.Content);
                        output.Content.AppendHtml(tag.GetString());
                    }
                }

                output.TagName = "";
            }
        }
    }
}