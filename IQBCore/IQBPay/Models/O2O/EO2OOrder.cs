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
    [Table("O2OOrder")]
    public class EO2OOrder
    {
        public EO2OOrder()
        {
            CreateDateTime = DateTime.Now;
            SettlementDateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(20)]
        public string O2ONo { get; set; }

        //   public long qrUserId { get; set; }
        [MaxLength(32)]
        public string AgentOpenId { get; set; }

        /// <summary>
        /// 费率
        /// </summary>
        public double FeeRate { get; set; }

        /// <summary>
        /// 用户手续费
        /// </summary>
        public double MarketRate { get; set; }

        /// <summary>
        /// 出库商UserId
        /// </summary>
        public int WHUserId { get; set; }

        [MaxLength(100)]
        public string WHAliPayAccount { get; set; }

        /// <summary>
        /// 出库商佣金
        /// </summary>
        public double WHRate { get; set; }

        public long RefOrderNo { get; set; }

        [MaxLength(20)]
        public string UserPhone { get; set; }

        [MaxLength(100)]
        public string UserAliPayAccount { get; set; }
     

        public int MallId { get; set; }

        public double MallFeeRate { get; set; }

        public int ItemId { get; set; }

        [MaxLength(50)]
        public string MallAccount { get; set; }

        [MaxLength(50)]
        public string MallPwd { get; set; }

        [MaxLength(20)]
        public string MallSMSVerify { get; set; }

        [MaxLength(255)]
        public string OrderImgUrl { get; set; }

        public double OrderAmount { get; set; }

        [MaxLength(200)]
        public string MallOrderNo { get; set; }

        [MaxLength(50)]
        public string RejectReason { get; set; }

        public int AddrId { get; set; }


        public O2OOrderStatus O2OOrderStatus { get; set; }

       

        public DateTime CreateDateTime { get; set; }

       
        /// <summary>
        /// 操作者
        /// </summary>
        public int SettlementUserId { get; set; }
        public DateTime SettlementDateTime { get; set; }
    }
}
