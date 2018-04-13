using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("O2OBuyer")]
    public class EO2OBuyer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(50)]
        public string BuyerName { get; set; }

        /// <summary>
        /// 用户对应的代理
        /// </summary>
        [MaxLength(32)]
        public string AgentOpenId { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(100)]
        public string ReceiveAccount { get; set; }

        public int BuyCount { get; set; }

        public DateTime LastBuyerDate { get; set; }

        public EO2OBuyer()
        {
            LastBuyerDate = DateTime.Now;
           
        }

    }
}
