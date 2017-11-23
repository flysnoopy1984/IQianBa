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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

       
        public PayWebStatus WebStatus { get; set; }

        [MaxLength(500)]
        public string Note { get; set; }

        public void Init()
        {
            WebStatus = PayWebStatus.Running;
            Note = "";
        }

        public Boolean AllowRegister { get; set; }
    }
}