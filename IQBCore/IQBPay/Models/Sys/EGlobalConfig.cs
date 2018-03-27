using IQBCore.IQBPay.BaseEnum;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.Models.Sys
{
    
    [Table("GlobalConfig")]
    public class EGlobalConfig:BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

       
        public PayWebStatus WebStatus { get; set; }

        public QRHugeEntry QRHugeEntry { get; set; }

        public PayWebStatus O2OEntry { get; set; }

        /// <summary>
        /// 大额码申请后可被扫码次数
        /// </summary>
        public int QRHugeScanedNum { get; set; }

        /// <summary>
        /// 大额码多久失效
        /// </summary>
        public int QRHugeInValidMinutes { get; set; }

        public int QRHugeMax { get; set; }
        public int QRHugeMin { get; set; }
        public int QRMax { get; set; }
        public int QRMin { get; set; }


        [MaxLength(500)]
        public string Note { get; set; }

        public void Init()
        {
            MaxNumChildAgent = 40;
            WebStatus = PayWebStatus.Running;
            QRHugeEntry = QRHugeEntry.Running;
            QRHugeMin = 1999;
            QRHugeMax = 4999;
            QRMin = 20;
            QRMax = 7999;
            Note = "";

        }

        public Boolean AllowRegister { get; set; }

        /// <summary>
        /// 市场价花呗扣点率
        /// </summary>
        public float MarketRate { get; set; }

        /// <summary>
        /// 转账后微信通知
        /// </summary>
        public Boolean IsWXNotice_AgentTransfer { get; set; }

        /// <summary>
        /// 代理最大数量
        /// </summary>
        public int MaxNumChildAgent { get; set; }

     


           /*O2O begin */
        /// <summary>
        /// 根据出库商的费率，决定代理费率，两者差值
        /// </summary>
        public double AgentFeeBasedShipFee { get; set; } 
        /*O2O end */



    }
}