using System;
using System.Threading.Tasks;
using Borg.Infra;
using Borg.Infra.DTO;
using Borg.MVC.Extensions;
using Borg.MVC.Services.UserSession;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timesheets.Web.Domain.Infrastructure;
using Timesheets.Web.Infrastructure;
using Borg.Infra.DAL;

namespace Timesheets.Web.Features.Teams
{
    [Route("Teams")]
    public class TeamsController : FrameworkController
    {
        private readonly IMediator _dispatcher;

        public TeamsController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory)
        {
            _dispatcher = dispatcher;
        }
        [Route("{team}")]
        public async Task<IActionResult> Teams(string team)
        {
            var result = await _dispatcher.Send(new TeamRequest(team));
            //if (result.Outcome == QueryOutcome.NotFound) return View("NotFound");
            if (result.Succeded)
            {
                SetPageTitle($"Team: {team}");
                return View((result as QueryResult<TeamViewModel>).Payload);
            }

            return View("Error");


        }

        [Route("{team}/Year/{year:int?}")]
        public async Task<IActionResult> Year(string team, int? year)
        {
            var result = await _dispatcher.Send(new TeamYearRequest(team, year ?? DateTime.Now.Year));
           // if (result.Outcome == QueryOutcome.NotFound) return View("NotFound");

            if (result.Succeded)
            {
                SetPageTitle($"Team: {team} - {year ?? DateTime.Now.Year}");
                return View((result as QueryResult<TeamYearViewModel>).Payload);
            }
            return View("Error");
        }

        [Route("AddBankHoliday")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddBankHoliday([FromServices] IContextAwareUserSession session, string team,DateTime date, int year, string description, string redirecturl)
        {
            var result = await _dispatcher.Send(new AddBankHolidayCommand(team, year,date, description));
          
            if (result.Succeded)
            {
                session.TryContextualize(this);
                session.Push(new ServerResponse(ResponseStatus.Info, $"{team} : {year}", $"Added {date:dd/MM}"));               
            }
            return Redirect(redirecturl);
        }
        [Route("RemoveBankHoliday")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RemoveBankHoliday([FromServices] IContextAwareUserSession session, string team, DateTime date, string redirecturl)
        {
            var result = await _dispatcher.Send(new RemoveBankHolidayCommand(team,  date));

            if (result.Succeded)
            {
                session.TryContextualize(this);
                session.Push(new ServerResponse(ResponseStatus.Info, $"{team} : {date.Year}", $"Deleted {date:dd/MM}"));
            }
            return Redirect(redirecturl);
        }
    }
}