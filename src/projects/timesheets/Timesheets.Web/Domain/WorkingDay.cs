using Borg.Infra.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Timesheets.Web.Domain.Infrastructure;
using System.Collections;

namespace Timesheets.Web.Domain
{
    public class WorkingDay : Entity<Guid>
    {
        protected WorkingDay()
        {
            Assignments = new List<Assignment>();
        }

        public WorkingDay(string workerId, DateTimeOffset date) : this()
        {
            Id = Guid.NewGuid();
            WorkerId = workerId;
            Date = date;
        }
        [Required]
        public string WorkerId { get; protected set; }
        [Required]
        public DateTimeOffset Date { get; protected set; }

        public ICollection<Assignment> Assignments { get; protected set; }

        public double Span()
        {
            return Assignments.Sum(x => x.Span);
        }

        public bool SingleAssigmentDay()
        {
            return Assignments.Count() == 1;
        }

        public DayOfWeek DayOfWeek()
        {
            return Date.DayOfWeek;
        }
    }

    public class WorkerPeriod : ValueObject<WorkerPeriod>, IEnumerable<WorkingDay>
    {
        private readonly List<WorkingDay> _data;

        protected Period _period;


        protected WorkerPeriod(IEnumerable<WorkingDay> data)
        {
            _data = (data == null) ? new List<WorkingDay>() : new List<WorkingDay>(data);
            Pad();
        }

        private void Pad()
        {
            _period = Period.Create(_data.Min(x => x.Date), _data.Max(x => x.Date));
            var missingDays = _period.MissingDays(_data.Select(x => x.Date));
            if (!missingDays.Any()) return;
            var usr = _data.OrderByDescending(x => x.Date).First().WorkerId;
            foreach(var md in missingDays)
            {
                _data.Add(new WorkingDay(usr, md));
            }
        }

        public IEnumerator<WorkingDay> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}