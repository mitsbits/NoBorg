using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice.TagHelpers
{
    [HtmlTargetElement("button", Attributes = "comp-deleted")]
    [PulgInTagHelper("Component Deleted State Toggle Button")]
    public sealed class ComponentDeletedStateToggleButtonTagHelper : TagHelper
    {
        public bool DeletedState { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Clear();
            var button = new TagBuilder("button");
            button.Attributes.Add("type", "submit");
            button.AddCssClass(DeletedState
                ? "btn btn-danger btn-sm"
                : "btn btn-success btn-sm");
            var tag = new TagBuilder("span");
            tag.AddCssClass(DeletedState
                ? "glyphicon glyphicon-remove"
                : "glyphicon glyphicon-ok");
            button.InnerHtml.AppendHtml(tag.GetString());
            output.Content.AppendHtml(button);
            output.TagName = "";
        }
    }
}