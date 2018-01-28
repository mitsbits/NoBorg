using System;
using Microsoft.AspNetCore.Mvc;

namespace Borg.MVC.PlugIns.Decoration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class PlugInEntryPointControllerAttribute : AreaAttribute
    {
        public PlugInEntryPointControllerAttribute(string areaName) : base(areaName)
        {
        }
    }
}