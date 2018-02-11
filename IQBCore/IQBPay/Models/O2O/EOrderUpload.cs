using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    public class EOrderUpload
    {
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 真实商城订单号
        /// </summary>
        public string RealOrderId { get; set; }

        /// <summary>
        /// 截图地址
        /// </summary>
        public string FilePath { get; set; }
    }
}
