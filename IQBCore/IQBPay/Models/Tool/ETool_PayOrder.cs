using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Tool
{
    [Table("Tool_PayOrder")]
    public class ETool_PayOrder
    {
        public string OrderNo { get; set; }

        public string QRFilePath { get; set; }


        public string Amount { get; set; }
    }
}
