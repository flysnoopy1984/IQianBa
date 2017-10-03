using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBPay.Models.System
{
    [Table("SysConfig")]
    public class ESysConfig:BaseModel
    {
        [Key]
        [MaxLength(20)]
        public string ID { get; set; }

        
        public long DefaultQR_AR { get; set; }

        
        public int MaxUserStore { get; set; }

        public int MaxStoreQR { get; set; }

        /// <summary>
        /// 平台扣点
        /// </summary>
        public float PPRate { get; set; }


    }
}