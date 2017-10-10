using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Order
{
    [Table("Settlement")]
    public class ESettlement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public DateTime TransDate { get; set; }

        public long OrderId { get; set; }

        public float OrderAmount { get; set; }

        public long QRUserId { get; set; }

        public long AgentName { get; set; }

        public string Buyer_AliPayId { get; set; }
    }
}
