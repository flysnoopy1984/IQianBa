using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.InParameter
{
    public class InQR
    {
        public string QRId { get; set; }
        public QRType QRType { get;set;}
    }
}
