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

        public DateTimeOffset Start { get; }

        public DateTimeOffset End { get; }

        public static Period Create(DateTimeOffset start, DateTimeOffset end)
        {
            return new Period(start, end);
        }

        public static Period Create(DateTimeOffset start, TimeSpan lenngth)
        {
            return new Period(start, start.Add(lenngth));
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
    }
}