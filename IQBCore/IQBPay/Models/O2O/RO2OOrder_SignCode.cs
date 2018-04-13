using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [NotMapped()]
    public class RO2OOrder_SignCode: EO2OOrder_SignCode
    {
        public bool HasSignCode { get; set; }
    }
}
