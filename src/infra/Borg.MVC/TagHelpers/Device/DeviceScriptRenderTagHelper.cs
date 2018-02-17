using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace Borg.MVC.TagHelpers
{
    [HtmlTargetElement("script-render", Attributes = "device")]
    public class DeviceScriptRenderTagHelper : DeviceTagHelper
    {
        public DeviceScriptRenderTagHelper(IDeviceAccessor<IDevice> orchestrator) : base(orchestrator)
        {
        }

        public ScriptPosition ScriptPosition { get; set; } = ScriptPosition.Bottom;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _orchestrator.TryContextualize(ViewContext);
            var device = _orchestrator.Device;
            if (device.Scripts.Cast<Tiding>().Count(x => x.Hint == ScriptPosition.ToString()) == 0)
            {
                output.SuppressOutput();
                return;
            }

            var sorted = device.Scripts.Cast<Tiding>().Where(x => x.Hint == ScriptPosition.ToString()).OrderBy(x => x.Weight).ThenBy(x => x.Key);

            foreach (var script in sorted)
            {
                if (script.HumanKey == "script")
                {
                    var s = new TagBuilder("script");
                    s.GenerateId(script.Key, "-");
                    s.InnerHtml.AppendHtml(script.Value);
                    s.TagRenderMode = TagRenderMode.Normal;
                    output.Content.AppendHtml(s);
                }

                if (script.HumanKey == "src")
                {
                    var s = new TagBuilder("script");
                    s.GenerateId(script.Key, "-");
                    if (IaAbsolute(script.Value))
                    {
                        s.MergeAttribute("src", script.Value);
                    }
                    else
                    {
                        s.MergeAttribute("src", device.Domain + script.Value);
                    }

                    output.Content.AppendHtml(s);
                }
            }

            output.TagName = "";
        }
    }
}