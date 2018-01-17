using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    public class RQRHuge_HugeTrans
    {
       
        public string OpenId { get; set; }

        public string Amount { get; set; }

        public QRHugeStatus QRHugeStatus { get; set; }

        public QRHugeTransStatus QRHugeTransStatus { get; set; }

        public string CreateDate { get; set; }

    }
}
