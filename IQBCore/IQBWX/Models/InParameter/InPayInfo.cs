using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.InParameter
{
    public class InPayInfo
    {
        public string ItemDes { get; set; }

        public float PayAmount { get; set; }

        /// <summary>
        /// 买家的OpenId
        /// </summary>
        public string OpenId { get; set; }

        public long QrId { get; set; }

        public string BuyerPhone { get; set; }
    }
}
