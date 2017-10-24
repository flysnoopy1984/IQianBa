using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    [NotMapped()]
    public class ROrderInfo:EOrderInfo
    {
        public float TotalAmountSum { get; set; }

        public float RealTotalAmountSum { get; set; }
    }
}
