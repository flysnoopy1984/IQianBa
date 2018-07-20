using IQBCore.IQBPay.BaseEnum;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.Models.Store
{
    [Table("StoreInfo")]
    public class EStoreInfo : BasePageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 谁的邀请码，店的主就是谁的，和实际店主无关。
        /// </summary>
        [MaxLength(32)]
        public string OwnnerOpenId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [DefaultValue("")]
        [MaxLength(100)]
        public string Remark { get; set; }

        public long QRId { get; set; }

        public float Rate { get; set; }

      
        /// <summary>
        /// 可能被禁用
        /// </summary>
        public RecordStatus RecordStatus { get; set; }

        [MaxLength(28)]
        /// <summary>
        /// 支付宝的商户Id 2088xxx
        /// </summary>
        public string AliPayAccount { get; set; }

        [MaxLength(40)]
        public string AliPayAuthToke { get; set; }

        [MaxLength(32)]
        public string AliPayAuthAppId { get; set; }


        public Channel Channel { get; set; }

        [DefaultValue("")]
        [MaxLength(20)]
        public string OpenTime { get; set; }

        [DefaultValue("")]
        [MaxLength(20)]
        public string CloseTime { get; set; }

        public string FromIQBAPP { get; set; }

        /// <summary>
        /// 是否收款账户
        /// </summary>
        public bool IsReceiveAccount { get; set; }

        /// <summary>
        /// 非风控金额
        /// </summary>

        public float MaxLimitAmount{get;set;}

        /// <summary>
        /// 风控金额
        /// </summary>
        public float MinLimitAmount { get; set; }

        public float RemainAmount { get; set; }

        /// <summary>
        /// 每天的营业额度(限额)
        /// </summary>
        public float DayIncome { get; set; }

        public StoreAuthStatus StoreAuthStatus { get; set; }


        public StoreType StoreType { get; set; }

        [MaxLength(255)]
        public string Log { get; set; }

        [MaxLength(50)]
        public string Provider { get; set; }

        [MaxLength(100)]
        /// <summary>
        /// 中间商的账户
        /// </summary>
        public string MidCommAccount { get; set; }

        /// <summary>
        /// 中间商返利
        /// </summary>
        public float MidCommRate { get; set; }



    }
}