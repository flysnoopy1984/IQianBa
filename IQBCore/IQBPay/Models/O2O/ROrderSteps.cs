using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    public class ROrderSteps
    {
        /// <summary>
        /// 0成功，-1没有订单，-2没有手机号
        /// </summary>
        public int QueryResult { get; set; }
        public string OrderId { get; set; }

        public List<RO2OStep> SList { get; set; }
    }
}
