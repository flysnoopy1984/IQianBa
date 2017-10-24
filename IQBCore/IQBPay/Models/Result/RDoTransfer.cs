using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    public class RDoTransfer
    {
        /// <summary>
        /// 我的余额
        /// </summary>
        public float MyRemainAmount { get; set; }

        public float MyOrderTotalAmount { get; set; }

        public float MyAgentOrderTotalAmount { get; set; }
    }
}
