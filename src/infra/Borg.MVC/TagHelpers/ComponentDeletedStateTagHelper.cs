using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.MVC.TagHelpers
{
    [HtmlTargetElement("span", Attributes = "comp-deleted")]
    [PulgInTagHelper("Component Deleted State")]
    public sealed class ComponentDeletedStateTagHelper : TagHelper
    {
        public bool DeletedState { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Clear();
            var tag = new TagBuilder("span");
            tag.AddCssClass(DeletedState
                ? "glyphicon glyphicon-remove text-danger"
                : "glyphicon glyphicon-ok text-success");
            output.Content.AppendHtml(tag.GetString());
            output.TagName = "";
        }
    }
}