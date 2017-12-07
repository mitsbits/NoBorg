using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.RegularExpressions;

namespace Borg.MVC.TagHelpers
{
    public abstract class DeviceTagHelper : TagHelper
    {
        protected readonly IDeviceAccessor<IDevice> _orchestrator;
        private readonly Regex httpCheck = new Regex("^(http|https)://");

        protected DeviceTagHelper(IDeviceAccessor<IDevice> orchestrator)
        {
            _orchestrator = orchestrator;
        }

        protected bool IaAbsolute(string source)
        {
            return httpCheck.IsMatch(source);
        }
    }
}