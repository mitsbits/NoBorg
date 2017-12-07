using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Domain;

namespace Timesheets.Web.Domain.Services
{
    public interface ITimeZoneProvider
    {
        IDictionary<string, TimeZoneInfo> TimeZones { get; }
    }

    public static class TimeZoneProviderExtensions
    {
        public static IEnumerable<SelectListItem> TimeZoneOptions(this ITimeZoneProvider provider)
        {
            return provider.TimeZones.Keys.Select(
                x => new SelectListItem { Value = x, Text = provider.TimeZones[x].DisplayName });
        }

        public static TimeZoneInfo TeamTimeZoneInfo(this ITimeZoneProvider provider, TeamCoutries teamCoutry)
        {
            TimeZoneInfo result;

            switch (teamCoutry)
            {
                case TeamCoutries.UK:
                    result = provider.TimeZones["GMT Standard Time"];
                    break;
                case TeamCoutries.Australia:
                    result = provider.TimeZones["AUS Eastern Standard Time"];
                    break;
                case TeamCoutries.Greece:
                    result = provider.TimeZones["GTB Standard Time"];
                    break;
                default:
                    throw new NotSupportedException(nameof(teamCoutry));
            }
            return result;
        }
    }
}
