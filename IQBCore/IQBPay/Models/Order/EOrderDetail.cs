using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Order
{
    [Table("OrderDetail")]
    public class EOrderDetail
    {

        public string OrderNo { get; set; }

        /// <summary>
        /// 支付渠道 花呗还是余额
        /// </summary>
        public string AliPayChannel { get; set; }


    }
}
