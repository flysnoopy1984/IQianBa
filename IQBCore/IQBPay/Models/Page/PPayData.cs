using IQBCore.IQBPay.BaseEnum;
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

        public QRReceiveType QRType { get; set; }

        /// <summary>
        /// 传递给成功页面，为了确保是系统本身发起的
        /// </summary>
        public string PageSign { get; set; }

        public float MarketRate { get; set; }

        public string BuyerPhone { get; set; }
    }
}
