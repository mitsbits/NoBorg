using System;
using System.ComponentModel.DataAnnotations;
using Timesheets.Web.Domain.Infrastructure;
using Web.Domain;

namespace Timesheets.Web.Domain
{
    public class AnnualVacation : Entity<Guid>
    {
        protected AnnualVacation()
        {
            
        }

        public AnnualVacation(Guid workerId, int year, int days = 25):this()
        {
            Id = Guid.NewGuid();
            WorkerId = workerId;
            Year = year;
            Days = days;
        }
        [Required]
        public int Year { get; protected set; }
        [Required]
        public int Days { get; protected set; }
        [Required]
        public Guid WorkerId { get; protected set; }
        public virtual Worker Worker { get; protected set; }
    }
}
