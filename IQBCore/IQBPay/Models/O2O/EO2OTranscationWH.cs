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
    [Table("O2OTranscationWH")]
    public class EO2OTranscationWH
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public TransferTarget TransferTarget { get; set; }


        public int MallId { get; set; }

        public int ItemId { get; set; }

        [MaxLength(20)]
        public string O2ONo { get; set; }

        [MaxLength(200)]
        public string MallOrderNo { get; set; }

        public double FeeRate { get; set; }

        [MaxLength(100)]
        public string ReceiveAccount { get; set; }

        public double TransferAmount { get; set;}

        public DateTime TransDateTime { get; set; }

        [MaxLength(32)]
        public string OpenId { get; set; }
    }
}
