using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.O2O;
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

        [MaxLength(20)]
        public string TransDateStr { get; set; }

        [MaxLength(64)]
        public string OrderNo { get; set; }

        public long QRUserId { get; set; }

        //[MaxLength(100)]
        //public string AgentAliPayAccount { get; set; }

        [MaxLength(32)]
        public string AgentOpenId { get; set; }

        [MaxLength(40)]
        public string AgentName { get; set; }

        //[MaxLength(16)]
        //public string Buyer_AliPayId { get; set; }

        //[MaxLength(100)]
        //public string Buyer_AliPayLoginId { get; set; }

        //public Boolean IsAutoTransfer { get; set; }

        //[MaxLength(40)]
        //public string Operator { get; set; }

        public TransferTarget TransferTarget { get; set; }

        [MaxLength(100)]
        public string TargetAccount { get; set; }

        public TransferStatus TransferStatus { get; set; }

        [MaxLength(255)]
        public string Log { get; set; }

        [MaxLength(64)]
        public string AliPayOrderId { get; set; }


        public static ETransferAmount Init(TransferTarget target, string TransferId, float TransferAmount, string AliPayAccount, EOrderInfo order, EUserInfo ui = null)
        {
            ETransferAmount obj = new ETransferAmount();
            obj.TransferId = TransferId;
            obj.TransferAmount = TransferAmount;
            obj.TransDate = DateTime.Now;
            obj.TransDateStr = obj.TransDate.ToString("yyyy-MM-dd HH:mm");
            obj.OrderNo = order.OrderNo;
            obj.QRUserId = order.QRUserId;
            obj.TransferStatus = TransferStatus.Open;
            obj.TargetAccount = AliPayAccount;
            obj.Log = "";
            if (ui != null)
            {
                if (target == TransferTarget.Agent || target == TransferTarget.ParentAgent)
                {
                    obj.AgentOpenId = ui.OpenId;
                    obj.AgentName = ui.Name;
                }
            }
           
            obj.TransferTarget = target;
            return obj;
        }

        [NotMapped]
        [DefaultValue(0)]
        public int TotalCount { get; set; }

        [MaxLength(20)]
        public string O2ONo { get; set; }

        public void O2OInitForShipment(EO2OOrder O2OOrder)
        {
            this.O2OInitForUser(O2OOrder);
            this.TargetAccount = O2OOrder.WHAliPayAccount;
            this.TransferTarget = TransferTarget.O2OWareHouse;
          //  this
        }

        public void O2OInitForAgent(EO2OOrder O2OOrder, EUserInfo ui)
        {
           
            this.AgentOpenId = ui.OpenId;
            this.AgentName = ui.Name;
            this.O2ONo = O2OOrder.O2ONo;
            this.TargetAccount = ui.AliPayAccount;
            this.TransDate = DateTime.Now;
            this.TransDateStr = DateTime.Now.ToShortDateString();
            this.TransferStatus = TransferStatus.Open;
            this.TransferTarget = TransferTarget.Agent;
           
        }



        public void O2OInitForUser(EO2OOrder O2OOrder)
        {
            this.O2ONo = O2OOrder.O2ONo;
            this.TargetAccount = O2OOrder.UserAliPayAccount;
            this.TransDate = DateTime.Now;
            this.TransDateStr = DateTime.Now.ToShortDateString();
            this.TransferStatus = TransferStatus.Open;
            this.TransferTarget = TransferTarget.User;
        }
    }
}
