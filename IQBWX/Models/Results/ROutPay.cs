using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Results
{
    public class ROutPay
    {
        public string openId { get; set; }
        public decimal Amount { get; set; }

        public int OutResult { get; set; }
        public string ResultRemark { get; set; }

    }
}