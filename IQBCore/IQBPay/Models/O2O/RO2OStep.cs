using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [NotMapped()]
    public class RO2OStep: EO2OStep
    {
        public double OrderAmount { get; set; }

        public string O2ORuleCode { get; set; }

        public O2OOrderStatus O2OOrderStatus { get; set; }

        public string RejectReason { get; set; }

        public string O2ONo { get; set; }

        public int RSeq { get; set; }

        public bool NeedSMS { get; set; }
    }
}
