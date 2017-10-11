using IQBCore.IQBPay.Models.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.AccountPayment
{
    [Table("Settlement")]
    public class ETransferAmount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [MaxLength(64)]
        public string TransferId { get; set; }

        public float TransferAmount { get; set; }

        public DateTime TransDate { get; set; }

        [MaxLength(64)]
        public string OrderId { get; set; }

        public long QRUserId { get; set; }

        [MaxLength(40)]
        public string AgentName { get; set; }

        public string Buyer_AliPayId { get; set; }

        public static ETransferAmount Init(string TransferId,EOrderInfo order)
        {
            ETransferAmount obj = new ETransferAmount();
            obj.TransferId = TransferId;
            obj.TransferAmount = order.RealTotalAmount;
            obj.TransDate = DateTime.Now;
            obj.OrderId = order.OrderNo;
            obj.QRUserId = order.QRUserId;
            obj.AgentName = order.AgentName;
            return obj;
        }
    }
}
