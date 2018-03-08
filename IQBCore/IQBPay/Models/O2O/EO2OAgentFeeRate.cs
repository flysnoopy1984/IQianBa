using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("O2OAgentFeeRate")]
    public class EO2OAgentFeeRate
    {
        public EO2OAgentFeeRate()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(32)]
        public string OpenId { get; set; }

        ///// <summary>
        ///// 用户Id
        ///// </summary>
        //public int UserId { get; set; }



        //public long QrUserId { get; set; }

        /// <summary>
        /// 不同商城不同手续费
        /// </summary>
       
        public int MallId { get; set; }


        /// <summary>
        /// 费率(取Mall上的FeeRate,所有代理统一)
        /// </summary>
        public double DiffFeeRate { get; set; }

        /// <summary>
        /// 用户手续费
        /// </summary>
        public double MarketRate { get; set; } 
    }
}
