using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.InParameter
{
    public class InBuyerTrans
    {
        public string BuyerPhone { get; set; }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }
    }
}
