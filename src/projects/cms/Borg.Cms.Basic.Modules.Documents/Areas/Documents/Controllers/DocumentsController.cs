﻿using Borg.MVC;
using Borg.MVC.Conventions;
using Borg.MVC.Modules.Decoration;
using Microsoft.Extensions.Logging;

namespace Borg.Cms.Basic.Modules.Documents.Area.Documents.Controllers
{
    [ModuleEntryPointController("Documents")]
    [ControllerTheme("Backoffice")]
    public class DocumentsController : BorgController
    {
        public DocumentsController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}