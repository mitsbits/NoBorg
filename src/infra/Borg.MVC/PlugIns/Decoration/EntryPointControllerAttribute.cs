using Microsoft.AspNetCore.Mvc;
using System;

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