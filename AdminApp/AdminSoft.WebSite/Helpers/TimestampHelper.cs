using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSoft.WebSite.Helpers
{
    public static class TimestampHelper
    {
        public static double ToTimeStamp(this DateTime toTimeStamp)
        {
            DateTime tInit = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime toUTC = TimeZoneInfo.ConvertTimeToUtc(toTimeStamp);
            double toStamp = (toUTC - tInit).TotalSeconds;

            return Math.Round(toStamp, 0);
        }
        public static DateTime ToDateTime(this int toDateTime)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            date = date.AddSeconds(toDateTime).ToLocalTime();

            return date;
        }
    }
}