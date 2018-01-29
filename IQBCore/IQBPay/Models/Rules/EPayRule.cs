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
        /// 用户每单服务费
        /// </summary>
        public double User_ServerFee_Q { get; set; }

        public double User_ServerFee_HQ { get; set; }
    }
}
