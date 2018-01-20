using Microsoft.AspNetCore.Mvc;

namespace Borg.MVC.Modules.Decoration
{
    public class ModuleEntryPointControllerAttribute : AreaAttribute
    {
        public ModuleEntryPointControllerAttribute(string areaName) : base(areaName)
        {
        }
    }
}