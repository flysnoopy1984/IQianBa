﻿using IQBCore.IQBPay.BaseEnum;
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

        [MaxLength(500)]
        public string Note { get; set; }

        public void Init()
        {
            MaxNumChildAgent = 40;
            WebStatus = PayWebStatus.Running;
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


    }
}