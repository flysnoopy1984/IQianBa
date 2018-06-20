using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    /// <summary>
    /// 用户余额表
    /// </summary>
    public class EUserBalance
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public double Balance { get; set; }

        public string CurrencyCode { get; set; }
    }
}
