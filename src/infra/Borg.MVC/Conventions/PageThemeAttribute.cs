using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Borg.MVC.Conventions
{
    public class PageThemeAttribute : Attribute, IActionModelConvention
    {
        private readonly string _theme;

        public PageThemeAttribute(string theme)
        {
            _theme = theme;
        }


        public void Apply(ActionModel action)
        {
            action.Properties["theme"] = _theme;
        }
    }
}