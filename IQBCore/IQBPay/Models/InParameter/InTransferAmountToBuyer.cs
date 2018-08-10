using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.InParameter
{
    public class InTransferAmountToBuyer
    {
        public string OrderNo{get;set;}
         
        public string AliAccount { get; set; }

        public string BuyerPhone { get; set; }
    }
}
