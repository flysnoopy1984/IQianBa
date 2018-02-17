using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    public class InO2OTrans
    {
        public string MallOrderNo { get; set; }

        public int BeforeDay { get; set; }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }
    }
}
