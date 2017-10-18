using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    public class RUser_Order
    {
        public string OrderNo { get; set; }
        public string TransDateStr { get; set; }

        public float TotalAmount { get; set; }

        public float RealTotalAmount { get; set; }

        [NotMapped()]
        public int TotalCount;
    }
}
