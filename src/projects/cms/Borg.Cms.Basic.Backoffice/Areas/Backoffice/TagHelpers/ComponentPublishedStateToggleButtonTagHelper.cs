using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.TagHelpers
{
    [HtmlTargetElement("button", Attributes = "comp-published")]
    [PulgInTagHelper("Component Published State Toggle Button")]
    public sealed class ComponentPublishedStateToggleButtonTagHelper : TagHelper
    {
        public bool PublishedState { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Clear();
            var button = new TagBuilder("button");
            button.Attributes.Add("type", "submit");
            button.AddCssClass(PublishedState
                ? "btn btn-success btn-sm"
                : "btn btn-danger btn-sm");
            var tag = new TagBuilder("span");
            tag.AddCssClass(PublishedState
                ? "glyphicon glyphicon-ok-sign"
                : "glyphicon glyphicon-remove-sign");
            button.InnerHtml.AppendHtml(tag.GetString());
            output.Content.AppendHtml(button);
            output.TagName = "";
        }
    }
}