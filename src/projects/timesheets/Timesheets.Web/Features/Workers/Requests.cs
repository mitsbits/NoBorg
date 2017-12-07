using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Borg.Infra.Collections;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Timesheets.Web.Auth;
using Timesheets.Web.Domain;
using Timesheets.Web.Domain.Infrastructure;
using Timesheets.Web.Domain.Services;
using Web.Features.Workers;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;

namespace Timesheets.Web.Features.Workers
{
    #region WorkerRows
    public class WorkerRowViewModel
    {
        public string Email { get; set; }

        public bool LockoutEnabled { get; set; }

        public IEnumerable<Roles> Roles { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Team { get; set; }

        public DateTimeOffset? LastEntry { get; set; }

        public string Name()
        {
            return $"{LastName}, {FirstName}";
        }
    }
    public class WorkerRowsRequest : IRequest<QueryResult>
    {
        public WorkerRowsRequest(int page = 1, int rows = 10, string searchterm = "")
        {
            Page = page;
            Rows = rows;
            SearchTerm = searchterm;
        }

        public int Page { get; }
        public int Rows { get; }
        public string SearchTerm { get; }
    }


    public class WorkerRowsRequestHandler : IAsyncRequestHandler<WorkerRowsRequest, QueryResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<TimesheetsDbContext> _uow;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITimeZoneProvider _timeZoneProvider;
        public WorkerRowsRequestHandler(ILoggerFactory loggerfactory, IUnitOfWork<TimesheetsDbContext> uow, UserManager<ApplicationUser> userManager, ITimeZoneProvider timeZoneProvider)
        {
            _logger = loggerfactory.CreateLogger(typeof(WorkerRowsRequestHandler));
            _uow = uow;
            _userManager = userManager;
            _timeZoneProvider = timeZoneProvider;
        }
        public async Task<QueryResult> Handle(WorkerRowsRequest message)
        {
            try
            {
                var search = message.SearchTerm.Trim();
                Expression<Func<Worker, bool>> whereClause =
                    (x) => x.Id.Contains(search) || x.FirstName.Contains(search) || x.LastName.Contains(search);
                if (string.IsNullOrWhiteSpace(search)) whereClause = (x) => true;


                var db = _uow.Context;
                var count = await db.Workers.CountAsync(whereClause);



                var query = from w in db.Workers.AsNoTracking().Where(whereClause)
                            join a in db.AspUsers.AsNoTracking() on w.Id equals a.Email
                            select new { A = a, W = w };



                var hits = await
                    query
                        .OrderBy(x => x.W.LastName)
                        .ThenBy(x => x.W.FirstName)
                        .ThenBy(x => x.W.Id)
                        .Skip((message.Page - 1) * message.Rows)
                        .Take(message.Rows)
                        .Select(x => new WorkerRowViewModel
                        {
                            Email = x.W.Id,
                            LockoutEnabled = x.A.LockoutEnabled,
                            FirstName = x.W.FirstName,
                            LastName = x.W.LastName,
                            Team = x.W.TeamId,
                        })
                        .ToArrayAsync();

                var emails = hits.Select(x => x.Email).Distinct().ToArray();

                var rolesQuery = from w in db.AspUsers
                                 join ur in db.AspUserRoles on w.Id equals ur.UserId
                                 join r in db.AspRoles on ur.RoleId equals r.Id
                                 where emails.Contains(w.Email)
                                 select new { w.Email, Role = r.Name };

                var roleHits = await rolesQuery.AsNoTracking().ToListAsync();

                //var lastEntries = await
                //    _db.WorkingDays.Where(x => emails.Contains(x.WorkerId))
                //        .AsNoTracking()
                //        .GroupBy(x => x.WorkerId)
                //        .AsNoTracking()
                //        .ToDictionaryAsync(x => x.Key, x => x.Max(y => y.Date));

                foreach (var workerRowViewModel in hits)
                {
                    workerRowViewModel.Roles =
                        roleHits.Where(x => x.Email == workerRowViewModel.Email)
                            .Select(x => (Roles)Enum.Parse(typeof(Roles), x.Role))
                            .ToArray();

                    //workerRowViewModel.LastEntry = lastEntries.ContainsKey(workerRowViewModel.Email)
                    //    ? lastEntries[workerRowViewModel.Email]
                    //    : default(DateTimeOffset?);
                }

                return QueryResult<IPagedResult<WorkerRowViewModel>>.Success(new PagedResult<WorkerRowViewModel>(hits, message.Page, message.Rows, count));

            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Handler: @handler", typeof(WorkerRequestHandler));
                return QueryResult.Failure(ex.ToString());
            }
        }
    }
    #endregion

    #region WorkingDays

    public class AssigmentDTO
    {
        public string taxonomy { get; set; }
        public double span { get; set; }
    }
    public class WorkingDayViewModel
    {
        public string WorkerId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Team { get; set; }

        public DateTimeOffset Date { get; set; }

        public string DisplayName => Name();
        public string Name()=> $"{LastName}, {FirstName}";

        public AssigmentDTO[] Assigments { get; set; }
    }
    public class WorkingDayRequest : IRequest<QueryResult>
    {
        public WorkingDayRequest(string workerId, DateTimeOffset date)
        {
            WorkerId = workerId;
            Date = date;
        }

        public string WorkerId { get; }
        public DateTimeOffset Date { get; }
    }


    public class WorkingDaysRequestHandler : IAsyncRequestHandler<WorkingDayRequest, QueryResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<TimesheetsDbContext> _uow;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITimeZoneProvider _timeZoneProvider;
        public WorkingDaysRequestHandler(ILoggerFactory loggerfactory, IUnitOfWork<TimesheetsDbContext> uow, UserManager<ApplicationUser> userManager, ITimeZoneProvider timeZoneProvider)
        {
            _logger = loggerfactory.CreateLogger(typeof(WorkingDaysRequestHandler));
            _uow = uow;
            _userManager = userManager;
            _timeZoneProvider = timeZoneProvider;
        }
        public async Task<QueryResult> Handle(WorkingDayRequest message)
        {
            try
            {

                var worker = await _uow.QueryRepo<Worker>().Get(x => x.Id.Equals(message.WorkerId));
                var user = await _userManager.FindByEmailAsync(message.WorkerId);
                var day = await _uow.QueryRepo<WorkingDay>()
                    .Get(x => x.WorkerId.Equals(message.WorkerId) && x.Date.Date.Equals(message.Date.Date),CancellationToken.None, workingDay => workingDay.Assignments) ??
                          new WorkingDay(worker.Id, message.Date);
                var model = new WorkingDayViewModel()
                {
                    WorkerId = worker.Id,
                    Date = day.Date,
                    Assigments = day.Assignments.Select(x=> new AssigmentDTO(){span = x.Span, taxonomy = x.TaxonomyId.ToString()}).ToArray(),
                    FirstName = worker.FirstName,
                    LastName = worker.LastName,
                    Team = worker.TeamId
                };
                return QueryResult<WorkingDayViewModel>.Success(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Handler: @handler", GetType());
                return QueryResult.Failure(ex.ToString());
            }
        }
    }
    #endregion


    #region Worker
    public class WorkerRequest : IRequest<QueryResult>
    {
        public WorkerRequest(string email)
        {
            Email = email.Trim().ToUpper();
        }
        public string Email { get; }
    }

    public class WorkerRequestHandler : IAsyncRequestHandler<WorkerRequest, QueryResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<TimesheetsDbContext> _uow;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITimeZoneProvider _timeZoneProvider;
        public WorkerRequestHandler(ILoggerFactory loggerfactory, IUnitOfWork<TimesheetsDbContext> uow, UserManager<ApplicationUser> userManager, ITimeZoneProvider timeZoneProvider)
        {
            _logger = loggerfactory.CreateLogger(typeof(WorkerRequestHandler));
            _uow = uow;
            _userManager = userManager;
            _timeZoneProvider = timeZoneProvider;
        }
        public async Task<QueryResult> Handle(WorkerRequest message)
        {
            try
            {
                var worker = await _userManager.FindByEmailAsync(message.Email);
                var roles = await _userManager.GetRolesAsync(worker);

                var repo = _uow.QueryRepo<Worker>();

                //var workerEntity = await _db.Workers.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(message.Email));
                var workerEntity = await repo.Get(x => x.Id.Equals(message.Email));


                TeamCoutries country;
                Enum.TryParse(workerEntity.TeamId, out country);
                var model = new WorkerViewModel()
                {
                    Email = message.Email,
                    Roles = roles.ToArray(),
                    Locked = worker.LockoutEnabled,
                    FirstName = workerEntity.FirstName,
                    LaststName = workerEntity.LastName,
                    TeamId = workerEntity.TeamId
                };
                var timeZone = _timeZoneProvider.TeamTimeZoneInfo(country);
                if (worker.LockoutEnabled && worker.LockoutEnd.HasValue)
                {
                    model.LockoutEnd = TimeZoneInfo.ConvertTime(worker.LockoutEnd.Value.DateTime, TimeZoneInfo.Utc, timeZone);
                }
                else
                {
                    model.LockoutEnd = TimeZoneInfo.ConvertTime(DateTime.UtcNow.AddDays(42), TimeZoneInfo.Utc, timeZone);
                }
                return QueryResult<WorkerViewModel>.Success(model);


            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Handler: @handler", typeof(WorkerRequestHandler));
                return QueryResult.Failure(ex.ToString());
            }
        }
    }
    #endregion
}
