using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.QR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Page
{
    public class PPaySelection
    {
        //public string SmallQRUserId { get; set; }

        //public string CCQRUserId { get; set; }

        public long QRUserId { get; set; }

        public QRReceiveType QRType { get; set; }

        public string OpenId { get; set; }
    }
}
