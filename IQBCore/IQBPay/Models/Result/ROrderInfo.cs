using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    [NotMapped()]
    public class ROrderInfo:EOrderInfo
    {
        public float TotalAmountSum { get; set; }

        /// <summary>
        /// 代理总计
        /// </summary>
        public float RealTotalAmountSum { get; set; }

        /// <summary>
        /// 代理今日订单
        /// </summary>
        public string AgentTodayOrderCount { get; set; }

        /// <summary>
        /// 代理今日收益
        /// </summary>
        public string AgentTodayIncome { get; set; }

        /// <summary>
        /// 代理总收益
        /// </summary>
        public string AgentTotalIncome { get; set; }

        /// <summary>
        /// 用户转账总计
        /// </summary>
        public float BuyerTransferSum { get; set; }

        /// <summary>
        /// 商家佣金
        /// </summary>
        public float StoreAmountSum { get; set; }

        /// <summary>
        /// 上级代理
        /// </summary>
        public float ParentAmountSum { get; set; }

        public float PPIncome { get; set; }


    }
}
