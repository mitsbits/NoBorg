using Shouldly;
using System;
using Xunit;

namespace Borg.Infra.Tests.ExtensionMethods
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData("00:00:10", "2017/01/01 12:10:07", "2017/01/01 12:10:10")]
        [InlineData("00:15:00", "2017/01/01 12:10:00", "2017/01/01 12:15:00")]
        [InlineData("01:30:00", "2017/01/01 04:10:00", "2017/01/01 04:30:00")]
        [InlineData("06:00:00", "2017/01/01 09:15:00", "2017/01/01 12:00:00")]
        public void RoubdUp(string timespan, string source, string target)
        {
            TimeSpan tms = TimeSpan.Parse(timespan);
            DateTime s = DateTime.Parse(source);
            DateTime t = DateTime.Parse(target);
            t.ShouldBe(s.RoundUp(tms));
        }

        [Theory]
        [InlineData("00:00:10", "2017/01/01 12:15:16", "2017/01/01 12:15:10")]
        [InlineData("00:15:00", "2017/01/01 12:42:00", "2017/01/01 12:30:00")]
        [InlineData("01:30:00", "2017/01/01 04:10:00", "2017/01/01 03:00:00")]
        [InlineData("06:00:00", "2017/01/01 09:15:00", "2017/01/01 06:00:00")]
        public void RoubdDown(string timespan, string source, string target)
        {
            TimeSpan tms = TimeSpan.Parse(timespan);
            DateTime s = DateTime.Parse(source);
            DateTime t = DateTime.Parse(target);
            t.ShouldBe(s.RoundDown(tms));
        }

        [Theory]
        [InlineData("00:00:10", "2017/01/01 12:10:07", "2017/01/01 12:10:10")]
        [InlineData("00:15:00", "2017/01/01 12:10:00", "2017/01/01 12:15:00")]
        [InlineData("01:30:00", "2017/01/01 04:10:00", "2017/01/01 04:30:00")]
        [InlineData("06:00:00", "2017/01/01 09:15:00", "2017/01/01 12:00:00")]
        public void RoubdUpOffset(string timespan, string source, string target)
        {
            TimeSpan tms = TimeSpan.Parse(timespan);
            DateTimeOffset s = DateTimeOffset.Parse(source);
            DateTimeOffset t = DateTimeOffset.Parse(target);
            t.ShouldBe(s.RoundUp(tms));
        }

        [Theory]
        [InlineData("00:00:10", "2017/01/01 12:15:16", "2017/01/01 12:15:10")]
        [InlineData("00:15:00", "2017/01/01 12:42:00", "2017/01/01 12:30:00")]
        [InlineData("01:30:00", "2017/01/01 04:10:00", "2017/01/01 03:00:00")]
        [InlineData("06:00:00", "2017/01/01 09:15:00", "2017/01/01 06:00:00")]
        public void RoubdDownOffset(string timespan, string source, string target)
        {
            TimeSpan tms = TimeSpan.Parse(timespan);
            DateTimeOffset s = DateTimeOffset.Parse(source);
            DateTimeOffset t = DateTimeOffset.Parse(target);
            t.ShouldBe(s.RoundDown(tms));
        }
    }
}