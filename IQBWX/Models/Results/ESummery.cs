using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Results
{
    public class ESummery
    {
    }

    public class UserSummery
    {
        public int DayMemberAdded { get; set; }
        public int DayUserSub { get; set; }

        public int TotalMember { get; set; }
        public int TotalUser { get; set; }
    }

    public class OrderSummery
    {
        /// <summary>
        /// 套餐1
        /// </summary>
        public int O1DayAdded { get; set; }

        /// <summary>
        /// 套餐2
        /// </summary>
        public int O2DayAdded { get; set; }

        public int TotalO1 { get; set; }
        public int TotalO2 { get; set; }

    }
}