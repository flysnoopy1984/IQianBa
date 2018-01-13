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
    [Table("QRHugeTrans")]
    public class EQRHugeTrans
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public long QRHugeId { get; set; }

        [MaxLength(100)]
        public string UserAliPayAccount { get; set; }

        public QRHugeTransStatus TransStatus { get; set; }

        public DateTime CreatedDate { get; set; }

       
        public float Amount { get; set; }

        [MaxLength(40)]
        public string AgentName { get; set; }

        public static EQRHugeTrans Init(EQRHuge qrHuge,string UserAliPayAccount)
        {
            EQRHugeTrans trans = new EQRHugeTrans
            {
                QRHugeId = qrHuge.ID,
                UserAliPayAccount = UserAliPayAccount,
                AgentName = qrHuge.AgentName,
                Amount = qrHuge.Amount,
                CreatedDate = DateTime.Now,
                TransStatus = QRHugeTransStatus.Open,
            };

            return trans;
        }
    }
}
