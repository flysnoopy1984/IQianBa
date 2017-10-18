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

        [NotMapped()]
        public int TotalCount;
    }
}
