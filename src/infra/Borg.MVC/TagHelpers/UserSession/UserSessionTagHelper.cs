using Borg.MVC.Services.UserSession;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.MVC.TagHelpers
{
    public abstract class UserSessionTagHelper : TagHelper
    {
        protected readonly IContextAwareUserSession _session;

        protected UserSessionTagHelper(IContextAwareUserSession session)
        {
            _session = session;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
    }
}