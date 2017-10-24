using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.AccountPayment
{
    public class EPreTransfer
    {
        public string NoSeq { get; set; }

        public string OrderId { get; set; }

        public float TransferAmount { get; set; }

        
    }
}
