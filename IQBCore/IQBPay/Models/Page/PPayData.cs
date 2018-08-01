using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Page
{
    public class PPayData
    {
        public long qrId { get; set; }

        public string Name { get; set; }

        public float Rate { get; set; }

        public float MarketRate { get; set; }
    }
}
