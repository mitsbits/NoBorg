using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.TagHelpers
{
    [HtmlTargetElement("span", Attributes = "comp-published")]
    [PulgInTagHelper("Component Published State")]
    public sealed class ComponentPublishedStateTagHelper : TagHelper
    {
        public bool PublishedState { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Clear();
            var tag = new TagBuilder("span");
            tag.AddCssClass(PublishedState
                ? "glyphicon glyphicon-ok-sign text-success"
                : "glyphicon glyphicon-remove-sign text-danger");
            output.Content.AppendHtml(tag.GetString());
            output.TagName = "";
        }
    }
}