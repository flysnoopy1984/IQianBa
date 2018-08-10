using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.FeeRate
{
    [Table("FeeAdjustLog")]
    public class EFeeAdjustLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(32)]
        public string OpenId { get; set; }

        public float fromFee { get; set; }

        public float toFee { get; set; }

        public QRReceiveType QRReveiveType { get;set;}

        /// <summary>
        /// 费率调整的条件，因为订单数量达标
        /// </summary>
        public int OrderNumCondition { get; set; }

        /// <summary>
        /// 费率调整的条件，因为订单金额达标
        /// </summary>
        public int OrderAmountCondition { get; set; }
    }
}
