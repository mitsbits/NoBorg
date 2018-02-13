using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Borg.MVC.Services.Themes
{
    public class ThemeViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "theme";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context.ActionContext.ActionDescriptor.Properties.ContainsKey(THEME_KEY))
            {
                context.Values[THEME_KEY] = context.ActionContext.ActionDescriptor.Properties[THEME_KEY].ToString();
            }
       
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue(THEME_KEY, out var theme))
            {
                viewLocations = viewLocations.Concat(new[]
                    {
                        //$"Themes/{theme}/{{1}}/{{0}}.cshtml",
                        //$"Themes/{theme}/Shared/{{0}}.cshtml",
                        $"/Themes/{theme}/{{1}}/{{0}}.cshtml",
                        $"/Themes/{theme}/Shared/{{0}}.cshtml",
                    });
            }

            return viewLocations;
        }
    }
}