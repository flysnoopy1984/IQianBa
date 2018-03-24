using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    public class InO2OOrder
    {
        public string MallOrderNo { get; set; }
        public int BeforeDay { get; set; }

        public O2OOrderStatus O2OOrderStatus { get; set; }

        public bool IsSign { get; set; }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }


    }
}
