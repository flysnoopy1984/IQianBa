using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Order
{
    [Table("OrderInfo")]
    public class EOrderInfo:BasePageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string OrderNo { get; set; }

        public string AliPayOrderNo { get; set; }

        public string BuyerAliPayAccount { get; set; }
       
        public string SellerAliPayAccount { get; set; }
        public string SellerAliPayEmail { get; set; }

        public string AliPayTradeStatus { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string AliPayPayChannel { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public float TotalAmount { get; set; }

        /// <summary>
        /// 收款码
        /// </summary>
        public long QRId { get; set; }

        public float RealTotalAmount { get; set; }

        /// <summary>
        /// 扣点金额
        /// </summary>
        public float RateAmount { get; set; }



      
        public float Rate { get; set; }




    }
}
