using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infra.DDD
{
    public class Period : ValueObject<Period>
    {
        private Period(DateTimeOffset start, DateTimeOffset end)
        {
            Start = start;
            End = end;
        }

        private Period(DateTimeOffset start, TimeSpan duration)
            : this(start, start.Add(duration))
        {
        }

        public DateTimeOffset Start { get; }

        public DateTimeOffset End { get; }

        public static Period Create(DateTimeOffset start, DateTimeOffset end)
        {
            return new Period(start, end);
        }

        public static Period Create(DateTimeOffset start, TimeSpan lenngth)
        {
            return new Period(start, lenngth);
        }

        public IEnumerable<DateTimeOffset> MissingDays(IEnumerable<DateTimeOffset> source)
        {
            var mine = Spaces(TimeSpan.FromDays(1)).Select(x => x.Date).Distinct();
            var yours = source.Select(x => x.Date).Distinct();
            return mine.Except(yours).Select(x => new DateTimeOffset(x));
        }

        public IEnumerable<DateTimeOffset> Days()
        {
            return Spaces(TimeSpan.FromDays(1));
        }

        private IEnumerable<DateTimeOffset> Spaces(TimeSpan granuality)
        {
            var stop = false;
            var current = Start;
            while (!stop)
            {
                if (current > End)
                {
                    stop = true;
                    current = End;
                }

                yield return current;
                current = current.Add(granuality);
            }
        }

        public int DurationInMinutes()
        {
            return (End - Start).Minutes;
        }

        public Period NewEnd(DateTimeOffset newEnd)
        {
            return new Period(Start, newEnd);
        }

        public Period NewDuration(TimeSpan newDuration)
        {
            return new Period(this.Start, newDuration);
        }

        public Period NewStart(DateTimeOffset newStart)
        {
            return new Period(newStart, this.End);
        }

        public static Period CreateOneDayRange(DateTimeOffset day)
        {
            return new Period(day, day.AddDays(1));
        }

        public static Period CreateOneWeekRange(DateTimeOffset startDay)
        {
            return new Period(startDay, startDay.AddDays(7));
        }

        public bool Overlaps(Period other)
        {
            return Start < other.End &&
                   End > other.Start;
        }
    }
}