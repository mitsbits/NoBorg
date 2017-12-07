using Borg.Infra.DDD;
using System;
using System.ComponentModel.DataAnnotations;
using Timesheets.Web.Domain.Infrastructure;
using Web.Domain;

namespace Timesheets.Web.Domain
{
    public class Assignment : Entity<Guid>
    {
        protected Assignment() { }

        public Assignment(Guid workingDayId, Guid taxonomyId, double span = 8) : this()
        {
            Id = Guid.NewGuid();
            WorkingDayId = workingDayId;
            TaxonomyId = taxonomyId;
            Span = span;
        }
        [Required]
        public Guid TaxonomyId { get; protected set; }
        [Required]
        public double Span { get; protected set; }
        [Required]
        public Guid WorkingDayId { get; protected set; }
        public virtual WorkingDay WorkingDay { get; protected set; }
    }
}