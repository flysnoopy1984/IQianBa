using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.OutParameter
{
    public class OutAPI_QRHuge:OutAPIResult
    {
        public RQRHuge RQRHuge { get; set; }

        public int DiffSec { get; set; }
    }
}
