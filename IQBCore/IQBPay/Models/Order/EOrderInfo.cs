using IQBCore.IQBPay.BaseEnum;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Order
{
    [Table("OrderInfo")]
    public class EOrderInfo
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [MaxLength(64)]
        public string OrderNo { get; set; }

        [MaxLength(64)]
        public string AliPayOrderNo { get; set; }

        public float AliPayTotalAmount { get; set; }
        public float AliPayReceiptAmount { get; set; }
        public float AliPayBuerPayAmount { get; set; }

        /// <summary>
        /// 非账户2088102122524333
        /// </summary>
        [MaxLength(16)]
        public string BuyerAliPayId { get; set; }

        [MaxLength(100)]
        public string BuyerAliPayLoginId { get; set; }

        [MaxLength(20)]
        public string BuyerMobilePhone { get; set; }

        [MaxLength(30)]
        public string SellerAliPayId { get; set; }


        [MaxLength(100)]

        public string SellerAliPayEmail { get; set; }

        [MaxLength(32)]
        public string AliPayTradeStatus { get; set; }

        [MaxLength(20)]
        /// <summary>
        /// 支付方式
        /// </summary>
        public string AliPayPayChannel { get; set; }

        [MaxLength(32)]
        public string AliPayAppId { get; set; }

        /// <summary>
        /// 订单总金额(平台记录)
        /// </summary>
        public float TotalAmount { get; set; }

        /// <summary>
        /// 代理实际收入
        /// </summary>

        public float RealTotalAmount { get; set; }
        /// <summary>
        /// 扣点金额
        /// </summary>
        public float RateAmount { get; set; }

        public float Rate { get; set; }

        public long QRUserId { get; set; }

        [MaxLength(32)]
        public string AgentOpenId { get; set; }

        [MaxLength(40)]
        public string AgentName { get; set; }

        public OrderStatus OrderStatus { get; set; }

        [NotMapped]
        [DefaultValue(0)]
        public int TotalCount { get; set; }


        public DateTime TransDate { get; set; }

    
        public string TransDateStr { get; set; }

        public DateTime AliPayTransDate { get; set; }

        public OrderType OrderType { get; set; }

       
        public Channel SellerChannel { get; set; }

        public float SellerRate { get; set; }

        /// <summary>
        /// 商户佣金
        /// </summary>
        public float SellerCommission { get; set; }

        [MaxLength(20)]
        public string SellerName { get; set; }

        public int SellerStoreId { get; set; }

        [MaxLength(255)]
        public string LogRemark { get; set; }

        [MaxLength(64)]
        public string TransferId { get; set; }

       
        public float TransferAmount { get; set; }

      
    }
}
