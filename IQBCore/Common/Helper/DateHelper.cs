using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Common.Helper
{
    public class DateHelper
    {
        public static bool IsOverTime(DateTime cmpDT,int mins)
        {


            TimeSpan ts1 = new TimeSpan(cmpDT.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();

            int sec = mins * 60;
            if (ts3.TotalSeconds > sec)
                return true;


            return false;
        }
    }
}
