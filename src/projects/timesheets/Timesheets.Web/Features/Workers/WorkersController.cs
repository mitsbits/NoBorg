using Borg.Infra.Collections;
using Borg.Infra.DAL;
using Borg.Infra.DDD;
using Borg.MVC.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Timesheets.Web.Infrastructure;
using Web.Features.Workers;

namespace Timesheets.Web.Features.Workers
{
    [Route("Workers")]
    public class WorkersController : FrameworkController
    {
        private readonly IMediator _dispatcher;

        public WorkersController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory)
        {
            _dispatcher = dispatcher;
        }

        [Route("{searchterm?}")]
        [HttpGet]
        public async Task<IActionResult> Index([FromServices]IApplicationUserSession userSession, string searchterm = "", int p = 1)
        {
            userSession.TryContextualize(this);
            if (!userSession.IsAdminOrManager())
            {
                return RedirectToAction("Worker", new { email = userSession.UserName });
            }
            var searchMessage = string.IsNullOrWhiteSpace(searchterm) ? string.Empty : $"Search for: {searchterm}";
            SetPageTitle("Workers", searchMessage);
            var result = await _dispatcher.Send(new WorkerRowsRequest(p, userSession.RowsPerPage(), searchterm ?? string.Empty));
            if (result.Outcome == TransactionOutcome.Success)
            {
                var model = (result as QueryResult<IPagedResult<WorkerRowViewModel>>).Payload;
                return View(model);
            }

            //if (result.Outcome == QueryOutcome.NotFound)
            //{
            //    return View(new PagedResult<WorkerRowViewModel>(new WorkerRowViewModel[0], p, userSession.RowsPerPage(), 0));
            //}

            //TODO: log error
            return View(new PagedResult<WorkerRowViewModel>(new WorkerRowViewModel[0], p, userSession.RowsPerPage(), 0));
        }

        [Route("{email}")]
        [HttpGet]
        public async Task<IActionResult> Worker([FromServices]IApplicationUserSession userSession, string email)
        {
            if (!userSession.IsAdminOrManager())
            {
                if (userSession.UserName.Trim().ToUpper() != email.Trim().ToUpper())
                {
                    return RedirectToAction("Worker", new { email = userSession.UserName });
                }
            }
            var result = await _dispatcher.Send(new WorkerRequest(email));
            if (result.Succeded)
            {
                var model = (result as QueryResult<WorkerViewModel>).Payload;
                SetPageTitle($"{model.FirstName} {model.LaststName}", $"{model.TeamId} - {model.Email.ToLower()}");
                return View(model);
            }
            SetPageTitle($"{email} - not found");
            return View("NotFound", result);
        }

        [Route("ToggleLockOut")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ToggleLockOut(WorkerViewModel model, string redirecturl)
        {
            var result = await _dispatcher.Send(new ToggleLockOutCommand(model.Email, model.LockoutEnd));
            return Redirect(redirecturl);
        }

        [Route("SetRoles")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SetRoles(WorkerRolesViewModel model, string redirecturl)
        {
            var result = await _dispatcher.Send(new RolesCommand(model.Email, model.RoleOptions.Where(x => x.Selected).Select(x => x.Key).ToArray()));
            return Redirect(redirecturl);
        }

        [Route("SetNameAndTeam")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SetNameAndTeam(WorkerNameAndTeamViewModel model, string redirecturl)
        {
            var result = await _dispatcher.Send(new NameAndTeamCommand(model));
            return Redirect(redirecturl);
        }

        [Route("jfeed/v1/callendar/")]
        [HttpPost]
        public async Task<IActionResult> Callendar(string workerId, DateTimeOffset start, DateTimeOffset end)
        {
            var period = Period.Create(start, end); //TODO: utilize period
            var bucket = new List<dynamic>();
            foreach (var day in period.Days())
            {
                bucket.Add(new { id = Guid.NewGuid().ToString(), title = "borg", start = day, allDay = true });
            }
            return Ok(bucket.ToArray());
        }

        [Route("jfeed/v1/WorkingDay/")]
        [HttpPost]
        public async  Task< IActionResult> WorkingDay(string workerId, DateTimeOffset date)
        {

            var result = await _dispatcher.Send(new WorkingDayRequest(workerId, date));
            if (result.Succeded)
            {
                var model = (result as QueryResult<WorkingDayViewModel>)?.Payload;
                var bucket = new[]
                {
                    new AssigmentDTO {taxonomy = "cccc", span = 4},
                    new AssigmentDTO {taxonomy = "dddd", span = 3},
                    new AssigmentDTO {taxonomy = "ffff", span = 2}
                };
                model.Assigments = bucket;
                return Ok(model);
            }
            
            return StatusCode(500, result);


        }

        [Route("jfeed/v1/UpdateWorkingDay/")]
        [HttpPost]
        public IActionResult UpdateWorkingDay(string workerId, DateTimeOffset date, AssigmentDTO[] assingments)
        {

            return Ok();
        }



    }
}