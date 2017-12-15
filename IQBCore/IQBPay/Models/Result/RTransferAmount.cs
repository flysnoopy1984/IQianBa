using IQBCore.IQBPay.Models.AccountPayment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    [NotMapped()]
    public class RTransferAmount: ETransferAmount
    {
        public float TotalAmountSum { get; set; }

        public string AliPayAccount { get; set; }

        public string TodayTransferAmt { get; set; }

        public string TotalTransferAmt { get; set; }
    }
}
