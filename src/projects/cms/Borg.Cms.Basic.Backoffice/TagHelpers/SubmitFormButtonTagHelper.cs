﻿using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace Borg.Cms.Basic.Backoffice.TagHelpers
{
    [PulgInTagHelper("Submit Form Button")]
    [HtmlTargetElement("button", Attributes = "submit-form")]
    public class SubmitFormButtonTagHelper : TagHelper
    {
        public string IconClass { get; set; } = "fa fa-check";
        public string ColourClass { get; set; } = "primary";

        public bool PullRight { get; set; } = true;

        public bool Block { get; set; } = false;

        public string Tooltip { get; set; } = "Save";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {


            output.Attributes.Clear();
            output.Attributes.SetAttribute("type", "submit");

            output.Attributes.Add("class", $"btn btn-{ColourClass} {(PullRight ? "pull-right" : "")} {(Block?"btn-block":"")}");
            output.Attributes.SetAttribute("title", Tooltip);


            var icon = new TagBuilder("i");
            icon.AddCssClass(IconClass);
            output.Content.SetHtmlContent(icon);

        }
    }
}