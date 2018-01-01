using System.Collections.Generic;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Borg.MVC.BuildingBlocks
{
    public abstract class ViewComponentModule<TData> : ViewComponent, IModule<TData> where TData : IDictionary<string, string>
    {
        public ModuleGender ModuleGender => ModuleGender.ViewComponent;
    }
}