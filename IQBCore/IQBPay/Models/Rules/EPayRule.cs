using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Rules
{
    public class EPayRule
    {
        /// <summary>
        /// 首单费率
        /// </summary>
        public double Agent_FOFeeRate { get; set; }

        /// <summary>
        /// 代理邀请码开通所需订单金额
        /// </summary>
        public double Agent_InviteFee { get; set; }

        /// <summary>
        /// 大额码开通所需订单金额
        /// </summary>
        public double Agent_QRHugeFee { get; set; }

        /// <summary>
        /// 每单用户服务费
        /// </summary>
        public double User_ServerFee_Q { get; set; }

        public double User_ServerFee_HQ { get; set; }
    }
}
