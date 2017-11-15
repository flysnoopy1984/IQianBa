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
    
    [Table("GlobalConfig")]
    public class EGlobalConfig:BaseModel
    {
        [Key]
        [MaxLength(20)]
        public string ID { get; set; }

       
        public PayWebStatus WebStatus { get; set; }


    }
}