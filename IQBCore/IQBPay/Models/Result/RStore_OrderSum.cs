using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    public class RStore_OrderSum
    {
        public string StoreName { get; set; }

        public float DayIncome { get; set; }

        public float Rate { get; set; }

        public float TotalAmount { get; set; }
        [NotMapped()]
        public int TotalCount;
    }
}
