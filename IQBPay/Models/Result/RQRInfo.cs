using IQBPay.Models.QR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBPay.Models.Result
{
    [NotMapped]
    public class RQRInfo:EQRInfo
    {
        public string StoreName { get; set; }
    }
}