using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Json
{
    public class InOrderReview
    {
        public string RejectReason { get; set; }
        public string MallOrderNo { get; set; }

        public string O2ONo { get; set; }

        public double OrderAmount { get; set; }

        public bool IsApprove { get; set; }
    }
}
