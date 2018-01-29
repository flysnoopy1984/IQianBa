using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Rules
{
    public class EBenefitRule
    {
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 费率
        /// </summary>
        public double FeeRate { get; set; }

        /// <summary>
        /// 返佣金率
        /// </summary>
        public double CommRate  { get; set; }

        /// <summary>
        /// 3级返佣金率
        /// </summary>
        public double L3CommAmtRate { get; set; }
    }
}
