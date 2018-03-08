using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    /// <summary>
    /// O2O中除了代理的佣金结算
    /// </summary>
   [Table("O2ORoleCharge")]
    public class EO2ORoleCharge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 可能是商户，可能是出库商，或之后扩充
        /// </summary>
        public O2OUserRole O2OUserRole { get; set; }

        /// <summary>
        /// 佣金费
        /// </summary>
        public double ChargeFee { get; set; }
    }
}
