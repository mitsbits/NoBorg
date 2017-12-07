using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Timesheets.Web.Domain.Infrastructure;
using Web.Domain;

namespace Timesheets.Web.Domain
{
    public class Worker : Entity<string>
    {
        protected Worker()
        {
            WorkingDays = new List<WorkingDay>();
            AnnualVacations = new List<AnnualVacation>();
        }

        public Worker(string userName, TeamCoutries team) :this()
        {
            Id = userName;
            TeamId = team.ToString();
        }
        [Required] [MaxLength(32)]
        public string TeamId { get; protected set; }

        public void SetTeam(TeamCoutries team)
        {
            TeamId = team.ToString();
        }
        public virtual Team Team { get; protected set; }
        [MaxLength(512)]
        public string FirstName { get; set; }
        [MaxLength(512)]
        public string LastName { get; set; }

        public virtual ICollection<WorkingDay> WorkingDays { get; protected set; }
        public virtual ICollection<AnnualVacation> AnnualVacations { get; protected set; }

        public  string Name()
        {
            return $"{LastName}, {FirstName}";
        }

        public override string ToString()
        {
            return $"{Id} : {Name()}";
        }
    }
}
