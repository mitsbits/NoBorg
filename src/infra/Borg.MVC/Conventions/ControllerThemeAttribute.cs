using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;

namespace Borg.MVC.Conventions
{
    public class ControllerThemeAttribute : Attribute, IControllerModelConvention
    {
        private readonly string _theme;

        public ControllerThemeAttribute(string theme)
        {
            _theme = theme;
        }

        public void Apply(ControllerModel controllerModel)
        {
            controllerModel.Properties["theme"] = _theme;
        }
    }
}