using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Borg.MVC.Conventions
{
    public class PageThemeAttribute : Attribute, IPageApplicationModelConvention
    {
        private readonly string _theme;

        public PageThemeAttribute(string theme)
        {
            _theme = theme;
        }


        public void Apply(PageApplicationModel model)
        {
            model.Properties["theme"] = _theme;
        }
    }
}