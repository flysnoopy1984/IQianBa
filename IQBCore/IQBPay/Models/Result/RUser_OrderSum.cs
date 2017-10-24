using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    public class RUser_OrderSum
    {
        public string AgentOpenId { get; set; }
        public string AgentName { get; set; }

        
        public float RemainAmount { get; set; }

        public float CommissionAmount { get; set; }

        public string AliPayAccount { get; set; }

        [NotMapped()]
        public int TotalCount;
    }
}
