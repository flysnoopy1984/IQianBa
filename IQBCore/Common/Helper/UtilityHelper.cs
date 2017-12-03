using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Common.Helper
{
    public class UtilityHelper
    {
        #region 获取 本周、本月、本季度、本年 的开始时间或结束时间
        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime GetTimeStartByType(string TimeType, DateTime now)
        {
            now = DateTime.Parse(now.ToString("yyyy-MM-dd"));
            switch (TimeType)
            {
                case "Week":
                    int weeknow = Convert.ToInt32(now.DayOfWeek);

                    //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
                    weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
                    int daydiff = (-1) * weeknow;

                    //本周第一天  
                    string FirstDay = now.AddDays(daydiff).ToString("yyyy-MM-dd");
                    return Convert.ToDateTime(FirstDay);

                
                case "Month":
                    return now.AddDays(-now.Day + 1);
                case "Season":
                    var time = now.AddMonths(0 - ((now.Month - 1) % 3));
                    return time.AddDays(-time.Day + 1);
                case "Year":
                    return now.AddDays(-now.DayOfYear + 1);
                default:
                    return now;
            }
        }

        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime GetTimeEndByType(string TimeType, DateTime now)
        {
            now = DateTime.Parse(now.ToString("yyyy-MM-dd"));
            DateTime result;
            switch (TimeType)
            {
                case "Week":
                    //星期天为最后一天  
                    int weeknow = Convert.ToInt32(now.DayOfWeek);
                    weeknow = (weeknow == 0 ? 7 : weeknow);
                    int daydiff = (7 - weeknow);

                    //本周最后一天  
                    string LastDay = now.AddDays(daydiff).ToString("yyyy-MM-dd");
                    result = Convert.ToDateTime(LastDay);
                    break;
                
                case "Month":
                    result  =  now.AddMonths(1).AddDays(-now.AddMonths(1).Day + 1).AddDays(-1);
                    break;
                case "Season":
                    var time = now.AddMonths((3 - ((now.Month - 1) % 3) - 1));
                    result = time.AddMonths(1).AddDays(-time.AddMonths(1).Day + 1).AddDays(-1);
                    break;
                case "Year":
                    var time2 = now.AddYears(1);
                    result= time2.AddDays(-time2.DayOfYear);
                    break;
                default:
                    result = now;
                    break;

            }
            return result.AddDays(1);
        }
        #endregion
    }
}
