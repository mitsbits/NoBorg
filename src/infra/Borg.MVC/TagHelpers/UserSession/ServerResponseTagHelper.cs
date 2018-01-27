using Borg.MVC.Extensions;
using Borg.MVC.Services.UserSession;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.MVC.TagHelpers
{
    [HtmlTargetElement("server-response")]
    public class ServerResponseTagHelper : UserSessionTagHelper
    {
        private readonly IHtmlHelper _helper;

        public ServerResponseTagHelper(IContextAwareUserSession session, IHtmlHelper helper) : base(session)
        {
            _helper = helper;
        }

        public string View { get; set; } = "~/Areas/Backoffice/Views/Shared/_serverResponse.cshtml";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            _session.TryContextualize(ViewContext);

            if (!_session.Messages.Any())
            {
                output.SuppressOutput();
                return;
            }
            (_helper as IViewContextAware).Contextualize(ViewContext);
            var content = await _helper.PartialAsync(View, _session.Messages);
            output.TagName = "";
            output.Content.SetHtmlContent(content);
        }
    }
}