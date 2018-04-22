using System;

namespace WeatherVane.Utility
{
    public class DateTimeHelper
    {
        public static DateTime EPOCH = new DateTime(1970, 1, 1);

        public static DateTime FromEpoch(long timeFromEpoch)
        {
            return EPOCH.AddSeconds(timeFromEpoch);
        }
    }
}
