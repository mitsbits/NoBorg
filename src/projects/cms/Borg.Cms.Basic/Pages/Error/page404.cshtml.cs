using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Borg.Cms.Basic.Pages.Error
{

    public class page404Model : PageModel
    {
        private readonly IPageOrchestrator<IPageContent, IDevice> _orchestrator;
        public page404Model(IPageOrchestrator<IPageContent, IDevice> orchestrator)
        {
            _orchestrator = orchestrator;
        }
        public void OnGet()
        {
            var r = this;
        }
    }
}