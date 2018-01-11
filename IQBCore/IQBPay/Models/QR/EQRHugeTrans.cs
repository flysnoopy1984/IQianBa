using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.QR
{
    [Table("QRHugeTrans")]
    public class EQRHugeTrans
    {
        public long ID { get; set; }

        public long QRId { get; set; }

        public string UserAccount { get; set; }

        public QRHugeTransStatus TransStatus { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
