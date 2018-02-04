using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Report
{
    public class RPAgentPerformance
    {
        public int Id { get; set; }
        public string OpneId { get; set; }
        public string AgentName { get; set; }

        public string ParentOpenId { get; set; }

        public string ParentName { get; set; }

        public string RegisterDate { get; set; }

        public int? OrderComplatedNum { get; set; }
        public double? OrderTotalAmount { get; set; }

        public int? OrderUnPaidNum { get; set; }
      
    }
}
