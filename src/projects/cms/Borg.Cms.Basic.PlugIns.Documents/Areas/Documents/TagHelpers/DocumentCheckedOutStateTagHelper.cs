using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.TagHelpers
{
    [HtmlTargetElement("span", Attributes = "doc-checkout")]
    [PulgInTagHelper("Document Checkd Out State")]
    public sealed class DocumentCheckedOutStateTagHelper : TagHelper
    {
        public bool CheckedOutState { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Clear();
            var tag = new TagBuilder("span");
            tag.MergeAttribute("title", CheckedOutState ? "Checked Out" : "Commited");
            tag.AddCssClass(CheckedOutState
                ? "glyphicon glyphicon-lock text-black"
                : "glyphicon glyphicon-thumbs-up text-primary");
            output.Content.AppendHtml(tag.GetString());
            output.TagName = "";
        }
    }
}