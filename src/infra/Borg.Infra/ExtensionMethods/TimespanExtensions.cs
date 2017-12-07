using System;
using System.Threading;

namespace Borg
{
    public static class TimespanExtensions
    {
        public static CancellationToken ToCancellationToken(this TimeSpan timeout)
        {
            if (timeout == TimeSpan.Zero)
                return new CancellationToken(true);

            return timeout.Ticks > 0 ? new CancellationTokenSource(timeout).Token : default(CancellationToken);
        }

        public static TimeSpan Min(this TimeSpan source, TimeSpan other)
        {
            return source.Ticks > other.Ticks ? other : source;
        }
    }
}