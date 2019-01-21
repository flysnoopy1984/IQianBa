using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Common.Extension
{
    public static class DateTimeExtension
    {
        public static int GetSecInterval(this DateTime dTime)
        {
            TimeSpan nowtimespan = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan endtimespan = new TimeSpan(dTime.Ticks);
            TimeSpan timespan = nowtimespan.Subtract(endtimespan).Duration();

            return Convert.ToInt32(timespan.TotalSeconds);
        }
    }
}
