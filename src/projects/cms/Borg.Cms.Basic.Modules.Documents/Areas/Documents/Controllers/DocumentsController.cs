using Borg.MVC;
using Borg.MVC.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Modules.Documents.Area.Documents.Controllers
{
    [Area("Documents")]
    [ControllerTheme("Backoffice")]
    public class DocumentsController : BorgController
    {
        public DocumentsController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}