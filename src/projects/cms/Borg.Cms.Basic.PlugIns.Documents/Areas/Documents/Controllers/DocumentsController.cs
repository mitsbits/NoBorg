using Borg.MVC;
using Borg.MVC.Conventions;
using Borg.MVC.PlugIns.Decoration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.PlugIns.Documents.Areas.Documents.Controllers
{
    [PlugInEntryPointController("Documents")]
    [ControllerTheme("Backoffice")][Authorize]
    public class DocumentsController : BorgController
    {
        public DocumentsController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}