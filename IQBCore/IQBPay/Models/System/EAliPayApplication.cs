using IQBCore.IQBPay.BaseEnum;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.Models.System
{
    [Table("AliPayApplication")]
    public class EAliPayApplication:BasePageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(256)]
        public string ServerUrl { get; set; }

        [MaxLength(32)]
        public string AppId { get; set; }

        [MaxLength(100)]
        public string AppName { get; set; }

        
        public string Merchant_Private_Key { get; set; }

        public string Merchant_Public_key { get; set; }

        [MaxLength(10)]
        public string Version { get; set; }

        [MaxLength(10)]
        public string SignType { get; set; }

        [MaxLength(10)]
        public string Charset { get; set; }


        public RecordStatus RecordStatus { get; set; }


        [MaxLength(256)]
        /// <summary>
        /// 支付宝的Url
        /// </summary>
        public string AuthUrl_Store { get; set; }

        /// <summary>
        /// 设置当前应用APP
        /// </summary>
        public bool IsCurrent { get; set; }

    }
}