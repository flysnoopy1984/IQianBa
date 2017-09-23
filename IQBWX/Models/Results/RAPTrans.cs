using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Results
{
    public class RAPTrans
    {
        public int TransId { get; set; }
        public DateTime TransDateTime { get; set; }
        public decimal Amount { get; set; }
        public int TotalCount { get; set; }
    }
}