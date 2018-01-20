using Microsoft.AspNetCore.Mvc;

namespace Borg.MVC.PlugIns.Decoration
{
    public class PlugInEntryPointControllerAttribute : AreaAttribute
    {
        public PlugInEntryPointControllerAttribute(string areaName) : base(areaName)
        {
        }
    }
}