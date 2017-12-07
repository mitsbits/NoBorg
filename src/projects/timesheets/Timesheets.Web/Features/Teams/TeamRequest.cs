using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Timesheets.Web.Auth;
using Timesheets.Web.Domain;
using Timesheets.Web.Domain.Infrastructure;
using Timesheets.Web.Domain.Services;
using Timesheets.Web.Features.Workers;
using Borg.Infra.DAL;

namespace Timesheets.Web.Features.Teams
{

    public class TeamViewModel
    {
        public string TeamId { get; set; }
        public string TimeZoneInfoId { get; set; }
        public IEnumerable<TeamMemberViewModel> TeamMembers { get; set; }
        public IReadOnlyDictionary<int, int> BankHolidays { get; set; }
    }

    public class TeamMemberViewModel
    {
        public string Email { get; set; }
        public bool Locked { get; set; }
        public string Display { get; set; }

    }

    public class TeamYearViewModel
    {
        public string TeamId { get; set; }
        public int Year { get; set; }

        public IEnumerable<BankHoliday> BankHolidays { get; set; }
    }

    public class TeamRequest : IRequest<QueryResult>
    {
        public TeamRequest(string teamId)
        {
            TeamId = teamId;
        }

        public string TeamId { get; }
    }
    public class TeamRequestHandler : IAsyncRequestHandler<TeamRequest, QueryResult>
    {
        private readonly ILogger _logger;
        private readonly TimesheetsDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITimeZoneProvider _timeZoneProvider;
        public TeamRequestHandler(ILoggerFactory loggerfactory, TimesheetsDbContext db, UserManager<ApplicationUser> userManager, ITimeZoneProvider timeZoneProvider)
        {
            _logger = loggerfactory.CreateLogger(typeof(WorkerRowsRequestHandler));
            _db = db;
            _userManager = userManager;
            _timeZoneProvider = timeZoneProvider;
        }
        public async Task<QueryResult> Handle(TeamRequest message)
        {
            try
            {
                var hit = await _db.Teams.Include(x => x.BankHolidays).AsNoTracking().SingleAsync(x => x.Id.Equals(message.TeamId));

                var model = new TeamViewModel()
                {
                    TeamId = hit.Id,
                    TimeZoneInfoId = hit.TimeZoneInfoId,
                    BankHolidays = hit.BankHolidays?.GroupBy(x => x.Date.Year).ToDictionary(x => x.Key, x => x.Count())
                };
                var teamMembersQuery =
                    from w in _db.Workers
                    join user in _db.AspUsers on w.Id equals user.Email
                    where w.TeamId.Equals(hit.Id, StringComparison.OrdinalIgnoreCase)
                    select new TeamMemberViewModel() { Email = user.Email, Locked = user.LockoutEnabled, Display = w.Name() };



                model.TeamMembers = await teamMembersQuery.ToListAsync();



                return QueryResult<TeamViewModel>.Success(model);
            }
            catch (Exception ex)
            {
                _logger.Error( ex, "Handler: @handler",  GetType());
                return QueryResult.Failure(ex.ToString());
            }
        }
    }

    public class TeamYearRequest : IRequest<QueryResult>
    {
        public TeamYearRequest(string teamId, int year)
        {
            TeamId = teamId;
            Year = year;
        }

        public string TeamId { get; }
        public int Year { get; }
    }



    public class TeamYearRequestHandler : IAsyncRequestHandler<TeamYearRequest, QueryResult>
    {
        private readonly ILogger _logger;
        private readonly TimesheetsDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITimeZoneProvider _timeZoneProvider;
        public TeamYearRequestHandler(ILoggerFactory loggerfactory, TimesheetsDbContext db, UserManager<ApplicationUser> userManager, ITimeZoneProvider timeZoneProvider)
        {
            _logger = loggerfactory.CreateLogger(typeof(WorkerRowsRequestHandler));
            _db = db;
            _userManager = userManager;
            _timeZoneProvider = timeZoneProvider;
        }
        public async Task<QueryResult> Handle(TeamYearRequest message)
        {
            try
            {
                var hit = await _db.BankHolidays.AsNoTracking()
                         .Where(x => x.TeamId.Equals(message.TeamId) && x.Date.Year == message.Year)
                            .OrderBy(x => x.Date)
                            .ToListAsync();
         
                var model = new TeamYearViewModel()
                {
                    TeamId = message.TeamId,
                    Year = message.Year,
                    BankHolidays = hit
                };
            
                return QueryResult<TeamYearViewModel>.Success(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Handler: @handler", GetType());
                return QueryResult.Failure(ex.ToString());
            }
        }
    }
}
