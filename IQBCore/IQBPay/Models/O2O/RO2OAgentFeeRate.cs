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
    public class RO2OAgentFeeRate: EO2OAgentFeeRate
    {

        public string MallName { get; set; }

        public string ItemName { get; set; }

        public double FeeRate { get; set; }

        public bool IsLightReceive { get; set; }

        public double Amount { get; set; }

        public string PayMethodStr { get; set; }

        private PayMethod _PayMethod;

        public PayMethod PayMethod
        {
            get { return _PayMethod; }
            set
            {
                _PayMethod = value;
                PayMethodStr = IQBPayEnum.GetPayMethod(value);

            }
        }
    }
}
