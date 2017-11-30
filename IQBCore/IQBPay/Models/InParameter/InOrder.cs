using IQBCore.IQBPay.BaseEnum;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.InParameter
{
    public class InOrder: BaseInPage
    {
        public OrderStatus OrderStatus { get; set; }

        public string AgentName { get; set; }

        public ConditionDataType DataType { get; set; }

        public string AgentOpenId { get; set; }

        public OrderType OrderType { get; set; }

        public string StoreId { get; set; }

        public string OrderNo { get; set; }

        public string AliPayOrderNo { get; set; }

        public InOrder()
        {
            DataType = ConditionDataType.Today;
            OrderStatus = OrderStatus.ALL;
            OrderType = OrderType.Normal;
        }
    }
}
