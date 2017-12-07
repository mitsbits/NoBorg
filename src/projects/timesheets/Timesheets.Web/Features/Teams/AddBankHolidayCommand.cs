using System;
using System.Linq;
using System.Threading.Tasks;
using Borg;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Timesheets.Web.Domain;
using Timesheets.Web.Domain.Infrastructure;
using Borg.Infra.DAL;
using Borg.Platform.EF.Contracts;

namespace Timesheets.Web.Features.Teams
{
    public class AddBankHolidayCommand : IRequest<CommandResult>
    {
        public AddBankHolidayCommand(string teamId, int year, DateTime date, string description)
        {
            TeamId = teamId;
            Year = year;
            Date = date;
            Description = description;
        }

        public string TeamId { get; }
        public int Year { get;  }
        public string Description { get;  }
        public DateTime Date { get; }

    }


    public class AddBankHolidayCommandHandler : IAsyncRequestHandler<AddBankHolidayCommand, CommandResult>
    {
        private readonly ILogger _logger;
        //private readonly TimesheetsDbContext _db;
        private readonly IUnitOfWork< TimesheetsDbContext> _uow;

        public AddBankHolidayCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<TimesheetsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(typeof(AddBankHolidayCommandHandler));
            _uow = uow;

        }

        public async Task<CommandResult> Handle(AddBankHolidayCommand message)
        {
            try
            {
                var repo = _uow.ReadWriteRepo<BankHoliday>();
                var dt = new DateTimeOffset(message.Year, message.Date.Month, message.Date.Day, 0,0,0, TimeSpan.Zero);
                //var hit = await _db.BankHolidays.Where(x => x.Date.Equals(dt) && x.TeamId.Equals(message.TeamId)).SingleOrDefaultAsync();
                var hit = await repo.Get(x => x.Date.Equals(dt) && x.TeamId.Equals(message.TeamId));
                if (hit != null)
                {
                    await repo.Delete(hit);
                }
                hit = new BankHoliday(message.TeamId, dt, message.Description);
                //await _db.BankHolidays.AddAsync(hit);
                //await _db.SaveChangesAsync();
                await repo.Create(hit);
                await _uow.Save();
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.Error( ex, "Error registering new user: @exception", ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }


    public class RemoveBankHolidayCommand : IRequest<CommandResult>
    {
        public RemoveBankHolidayCommand(string teamId,  DateTime date)
        {
            TeamId = teamId;
        
            Date = date;
     
        }

        public string TeamId { get; }
 
  
        public DateTime Date { get; }

    }


    public class RemoveBankHolidayCommandHandler : IAsyncRequestHandler<RemoveBankHolidayCommand, CommandResult>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork< TimesheetsDbContext> _uow;


        public RemoveBankHolidayCommandHandler(ILoggerFactory loggerFactory, IUnitOfWork<TimesheetsDbContext> uow)
        {
            _logger = loggerFactory.CreateLogger(typeof(AddBankHolidayCommandHandler));
            _uow = uow;

        }

        public async Task<CommandResult> Handle(RemoveBankHolidayCommand message)
        {
            try
            {
                var clock = message.Date.ToUniversalTime();
                var repo = _uow.ReadWriteRepo<BankHoliday>();
                var hit = await repo.Get(x => x.Date.Equals(clock) && x.TeamId.Equals(message.TeamId));
                if (hit != null)
                {
                    await repo.Delete(hit);
                }

                await _uow.Save();
                return CommandResult.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error registering new user: @exception", ex);
                return CommandResult.Failure(ex.ToString());
            }
        }
    }
}
