using System;
using System.ComponentModel.DataAnnotations;
using Timesheets.Web.Domain.Infrastructure;
using Web.Domain;

namespace Timesheets.Web.Domain
{
    public class BankHoliday :Entity<Guid>
    {
        protected BankHoliday()
        {
            
        }

        public BankHoliday(string teamId, DateTimeOffset date, string description = ""):this()
        {
            Id = Guid.NewGuid();
            TeamId = teamId;
            Date = date;
            Description = description;
        }
        [Required]
        public DateTimeOffset Date { get;protected set; }
        [StringLength(1024)]
        public string Description { get; protected set; }
        [Required][MaxLength(32)]
        public string TeamId { get; protected set; }

        public virtual Team Team { get; protected set; }
    }
}
