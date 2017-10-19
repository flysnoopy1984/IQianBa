using IQBCore.IQBPay.BaseEnum;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.InParameter
{
    public class InTransfer: BaseInPage
    {
        public string AgentName { get; set; }

        public string AgentOpenId { get; set; }

        public ConditionDataType DataType { get; set; }

        public InTransfer()
        {
            DataType = ConditionDataType.Today;
         
        }
    }
}
