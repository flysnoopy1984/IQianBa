using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.InParameter
{
    public class InAgentOverview
    {
        public string AgentName { get; set; }
        public string ParentName { get; set; }
        public string BeforeDay { get; set; }
        public  int pageIndex { get; set; }

        public int pageSize { get; set; }
    }
}

