using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Emit;
using Timesheets.Web.Infrastructure;

namespace Timesheets.Web.Features.Workers
{
    public class WorkerCallendarViewComponent : ViewComponent
    {
        private readonly IApplicationUserSession _userSession;
        public WorkerCallendarViewComponent(IApplicationUserSession userSession)
        {
            _userSession = userSession;
        }


        public async Task<IViewComponentResult> InvokeAsync(string workerId = "")
        {
            if (string.IsNullOrWhiteSpace(workerId)) workerId = _userSession.UserName;
            return View(new WorkerCallendarViewModel{WorkerId = workerId});
        }
    }

    public class WorkerCallendarViewModel
    {
        public string WorkerId { get; set; }
    }
}
