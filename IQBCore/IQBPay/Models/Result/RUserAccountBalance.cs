using IQBCore.IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    [NotMapped]
    public class RUserAccountBalance: EUserAccountBalance
    {
        public string UserName { get; set; }

        public double? O2OOnOrderAmount { get; set; }
    }
}
