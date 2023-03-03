using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Converters
{
    public class TimeConverter
    {
        /// <summary>
        /// Converts DateTime to Unix Timestamp in seconds
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns>long</returns>
        public long ConvertDateTimeToUnix(DateTime datetime)
        {
            DateTime dt = datetime;
            long unixTimestampInSeconds = new DateTimeOffset(dt).ToUnixTimeSeconds();

            return unixTimestampInSeconds; 
        }

        /// <summary>
        /// Converts string unix time to DateTime
        /// </summary>
        /// <param name="uDateTime"></param>
        /// <returns>DateTime</returns>
        public DateTime ConvertUnixToDateTime(int uDateTime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(uDateTime).ToLocalTime();
            return dtDateTime;
        }
        /// <summary>
        /// Rounds up to nearest hour 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>DateTime nearest hour</returns>
        public DateTime RoundUpDateTime(DateTime dateTime)
        {
            var timeSpan = TimeSpan.FromMinutes(60);
            return new DateTime((dateTime.Ticks + timeSpan.Ticks - 1) / timeSpan.Ticks * timeSpan.Ticks, dateTime.Kind);
        }

    }
}
