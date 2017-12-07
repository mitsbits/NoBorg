using System;
using System.Collections.Generic;
using System.Linq;

namespace Timesheets.Web.Domain.Services
{
    public class TimeZoneProvider : ITimeZoneProvider
    {
        private static readonly IDictionary<string, TimeZoneInfo> _timeZoneInfos;

        static TimeZoneProvider()
        {
            _timeZoneInfos = TimeZoneInfo.GetSystemTimeZones().ToDictionary(x => x.Id, x => x);
        }

        public TimeZoneProvider() { }
        public IDictionary<string, TimeZoneInfo> TimeZones => _timeZoneInfos;
    }
}
