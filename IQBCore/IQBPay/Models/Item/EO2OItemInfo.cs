using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Item
{
    public class EO2OItemInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MallId { get; set; }

        public string RealAddress { get; set; }

        public string ImgUrl { get; set; }

        public float Amount { get; set; }

        public int Qty { get; set; }

        public int O2OItemRuleId { get; set; }
    }
}
