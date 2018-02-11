using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("O2ORule")]
    public class EO2ORule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DefaultValue("")]
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(50)]
        [DefaultValue("")]
        public string Name { get; set; }

        public bool NeedMallAccount { get; set; }

        public bool NeedMallSMSVerify { get; set; }

        /// <summary>
        /// 是否秒回款
        /// </summary>
        public bool IsMoneyImmediate { get; set; }

        public bool IsGeneralPayQR { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public void InitFromUpdate(EO2ORule updateObj)
        {
            this.Name = updateObj.Name;
            this.Code = updateObj.Code;
            this.IsGeneralPayQR = updateObj.IsGeneralPayQR;
            this.IsMoneyImmediate = updateObj.IsMoneyImmediate;
            this.NeedMallAccount = updateObj.NeedMallAccount;
            this.NeedMallSMSVerify = updateObj.NeedMallSMSVerify;
            this.RecordStatus = updateObj.RecordStatus;
            
        }

    }
}
