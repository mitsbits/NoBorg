using System;

namespace Borg
{
    public static class NumericExtensions
    {
        public static string ToOrdinal(this int num)
        {
            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num.ToString("#,###0") + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num.ToString("#,###0") + "st";

                case 2:
                    return num.ToString("#,###0") + "nd";

                case 3:
                    return num.ToString("#,###0") + "rd";

                default:
                    return num.ToString("#,###0") + "th";
            }
        }

        public static string SizeDisplay(this long byteCount, string format = "{0:0.##} {1}", int decimalsToShow = 1)
        {
            if (string.IsNullOrWhiteSpace(format)) format = "{0:0.##} {1}";
            if (decimalsToShow < 0) decimalsToShow = 1;
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0) return string.Format(format, 0, suf[0]);
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), decimalsToShow);
            return string.Format(format, Math.Sign(byteCount) * num, suf[place]);
        }

        public static string SizeDisplay(this int byteCount, string format = "{0:0.##} {1}", int decimalsToShow = 1)
        {
            return ((long)byteCount).SizeDisplay(format, decimalsToShow);
        }

        public static int RoundUp(this int i, int round = 10)
        {
            return (int)((long)i).RoundUp(round);
        }

        public static long RoundUp(this long i, int round = 10)
        {
            return i.RoundDown(round) + round;
        }

        public static int RoundOff(this int i, int round = 10)
        {
            return (int)((long)i).RoundOff(round);
        }

        public static int RoundDown(this int i, int round = 10)
        {
            return (int)((long)i).RoundDown(round);
        }

        public static long RoundOff(this long i, int round = 10)
        {
            if (round <= 0) throw new ArgumentOutOfRangeException(nameof(round));
            return (long)Math.Round(i / (double)round) * round;
        }

        public static long RoundDown(this long i, int round = 10)
        {
            if (round <= 0) throw new ArgumentOutOfRangeException(nameof(round));
            var factor = Math.Truncate(i / (double)round);
            var result = (long)factor * round;
            return result;
        }
    }
}