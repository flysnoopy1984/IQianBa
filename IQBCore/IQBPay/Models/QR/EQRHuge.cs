using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.QR
{
    [Table("QRHuge")]
    public class EQRHuge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public DateTime CreateDate { get; set; }

        [MaxLength(32)]
        public string OpenId { get; set; }

        [MaxLength(40)]
        public string AgentName { get; set; }

        public float Amount { get; set; }

        [MaxLength(256)]
        public string QRUrl { get; set; }

        [MaxLength(128)]
        public string FilePath { get; set; }

        public QRHugeStatus QRHugeStatus { get;set;}

        /// <summary>
        /// 被支付次数
        /// </summary>
        public int PayCount { get; set; }


    }
}
