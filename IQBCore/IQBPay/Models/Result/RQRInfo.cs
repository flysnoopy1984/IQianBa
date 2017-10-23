using IQBCore.IQBPay.Models.QR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    [NotMapped()]
    public class RQRInfo:EQRInfo
    {
        public string ParentName { get; set; }

        public string StoreName { get; set; }
    }
}
