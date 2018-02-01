using IQBCore.IQBPay.BaseEnum;
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

        public float AgentAmount { get; set; }

        public float ParentCommissionAmount { get; set; }
        public float L3CommissionAmount { get; set; }

        public DateTime TransDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        [NotMapped()]
        public int TotalCount;

        public float TotalAmountSum { get; set; }

        /// <summary>
        /// 代理总计
        /// </summary>
        public float RealTotalAmountSum { get; set; }

        /// <summary>
        /// 代理今日订单
        /// </summary>
        public string AgentTodayOrderAmount { get; set; }

        /// <summary>
        /// 代理今日收益
        /// </summary>
        public string AgentTodayIncome { get; set; }

        /// <summary>
        /// 代理总收益
        /// </summary>
        public string AgentTotalIncome { get; set; }
    }
}
