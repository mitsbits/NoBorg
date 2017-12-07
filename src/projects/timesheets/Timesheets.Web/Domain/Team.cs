using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Timesheets.Web.Domain.Infrastructure;
using Web.Domain;

namespace Timesheets.Web.Domain
{
    public class Team : Entity<string>
    {
        protected Team() { BankHolidays = new List<BankHoliday>(); }

        public Team(TeamCoutries teamCoutry, string timeZoneInfoId):this()
        {
            Id = teamCoutry.ToString();
            TimeZoneInfoId = timeZoneInfoId;
        }

        [StringLength(50)]
        public string TimeZoneInfoId { get; protected set; }

        public ICollection<BankHoliday> BankHolidays { get; protected set; }

    }
}
