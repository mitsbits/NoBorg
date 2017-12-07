using System;

namespace Borg
{
    public static class DateTimeExtensions
    {
        public static DateTime RoundUp(this DateTime dt, TimeSpan timespan)
        {
            GuardTimespan(timespan);
            return new DateTime((dt.Ticks + timespan.Ticks - 1) / timespan.Ticks * timespan.Ticks);
        }

        public static DateTime RoundDown(this DateTime dt, TimeSpan timespan)
        {
            GuardTimespan(timespan);
            return new DateTime((dt.Subtract(timespan).Ticks + timespan.Ticks - 1) / timespan.Ticks * timespan.Ticks);
        }

        public static DateTimeOffset RoundUp(this DateTimeOffset dt, TimeSpan timespan)
        {
            GuardTimespan(timespan);
            return new DateTimeOffset(new DateTime((dt.Ticks + timespan.Ticks - 1) / timespan.Ticks * timespan.Ticks));
        }

        public static DateTimeOffset RoundDown(this DateTimeOffset dt, TimeSpan timespan)
        {
            GuardTimespan(timespan);
            return new DateTimeOffset(new DateTime((dt.Subtract(timespan).Ticks + timespan.Ticks - 1) / timespan.Ticks *
                                                   timespan.Ticks));
        }

        private static void GuardTimespan(TimeSpan timespan)
        {
            if (timespan == TimeSpan.Zero) throw new ArgumentNullException(nameof(timespan));
            if (Math.Abs(timespan.TotalSeconds) < 0.1) throw new ArgumentNullException(nameof(timespan));
        }
    }
}