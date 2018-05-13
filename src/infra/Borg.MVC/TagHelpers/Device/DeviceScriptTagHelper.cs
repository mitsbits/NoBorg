using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Borg.MVC.TagHelpers
{
    [HtmlTargetElement("script", Attributes = "device")]
    public class DeviceScriptTagHelper : DeviceTagHelper
    {
        public DeviceScriptTagHelper(IDeviceAccessor<IDevice> orchestrator) : base(orchestrator)
        {
        }

        public ScriptPosition ScriptPosition { get; set; } = ScriptPosition.Bottom;

        public double Weight { get; set; } = -1;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            _orchestrator.TryContextualize(ViewContext);
            var device = _orchestrator.Device;
            if (ScriptPosition == ScriptPosition.Inline) return;
            var isUrl = context.AllAttributes.ContainsName("src");
            Tiding tiding;
            if (isUrl)
            {
                tiding = new Tiding(context.AllAttributes["id"] != null
                    ? context.AllAttributes["id"].Value.ToString()
                    : nameof(DeviceScriptTagHelper).ToLower())
                {
                    Value = context.AllAttributes["src"].Value.ToString(),
                    Weight = Weight,
                    Hint = ScriptPosition.ToString(),
                    HumanKey = "src"
                };
            }
            else
            {
                tiding = new Tiding(context.AllAttributes["id"] != null
                   ? context.AllAttributes["id"].Value.ToString()
                   : nameof(DeviceScriptTagHelper).ToLower())
                {
                    Value = (await output.GetChildContentAsync()).GetContent(),
                    Weight = Weight,
                    Hint = ScriptPosition.ToString(),
                    HumanKey = "script"
                };
            }

            device.Scripts.Add(tiding);

            output.SuppressOutput();
        }
    }
}