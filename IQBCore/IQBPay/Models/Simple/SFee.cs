using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Simple
{
    public class SFee
    {
        public float OrigFeeRate { get; set; }

        public QRReceiveType QRType { get;set;}

        public float AdjustedFeeRate { get; set; }
    }
}
