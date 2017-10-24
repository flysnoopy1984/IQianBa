using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.AccountPayment
{
    [Table("TransferAmount")]
    public class ETransferAmount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [MaxLength(64)]
        public string TransferId { get; set; }

        public float TransferAmount { get; set; }

        public DateTime TransDate { get; set; }

        public string TransDateStr { get; set; }

        [MaxLength(64)]
        public string OrderNo { get; set; }

        public long QRUserId { get; set; }

        [MaxLength(100)]
        public string AgentAliPayAccount { get; set; }

        [MaxLength(32)]
        public string AgentOpenId { get; set; }

        [MaxLength(40)]
        public string AgentName { get; set; }

        [MaxLength(16)]
        public string Buyer_AliPayId { get; set; }

        [MaxLength(100)]
        public string Buyer_AliPayLoginId { get; set; }

        public Boolean IsAutoTransfer { get; set; }

        [MaxLength(40)]
        public string Operator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TransferId"></param>
        /// <param name="TargetAccount">转到哪个账户</param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static ETransferAmount Init(string TransferId,EUserInfo AgentUser,EOrderInfo order)
        {
            ETransferAmount obj = ETransferAmount.Init(TransferId, AgentUser.OpenId, AgentUser.AliPayAccount, order);
            return obj;
        }

        public static ETransferAmount Init(string TransferId, string OpenId,string AliPayAccount,EOrderInfo order)
        {
            ETransferAmount obj = new ETransferAmount();
            obj.TransferId = TransferId;
            obj.TransferAmount = order.RealTotalAmount;
            obj.TransDate = DateTime.Now;
            obj.TransDateStr = obj.TransDate.ToString("yyyy-MM-dd HH:mm");
            obj.OrderNo = order.OrderNo;
            obj.QRUserId = order.QRUserId;
            obj.AgentName = order.AgentName;
            obj.AgentOpenId = OpenId;
            obj.AgentAliPayAccount = AliPayAccount;
            return obj;
        }

        [NotMapped]
        [DefaultValue(0)]
        public int TotalCount { get; set; }
    }
}
