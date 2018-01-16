using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Common.Helper
{
    public class DateHelper
    {
        public static bool IsOverTime(DateTime cmpDT,int sec)
        {
            TimeSpan ts1 = new TimeSpan(cmpDT.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();            
            if (ts3.TotalSeconds > sec)
                return true;
            return false;
        }

        /// <summary>
        /// dt2 - dt1的差额
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static int GetDiffSec(DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt2.Ticks);
            TimeSpan ts2 = new TimeSpan(dt1.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            return Convert.ToInt32(ts3.TotalSeconds);
        }
    }
}
