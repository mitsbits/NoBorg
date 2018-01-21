using Borg.MVC;
using Borg.MVC.Conventions;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.PlugIns.BlogEngine.Areas.BlogEngine.Controllers
{
    [PlugInEntryPointController("BlogEngine")]
    [ControllerTheme("Backoffice")]
    [Authorize(Policy = "Blogger")]
    public  class BlogEngineController : BorgController
    {
        protected BlogEngineController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}