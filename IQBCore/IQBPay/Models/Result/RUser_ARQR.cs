using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    public class RUser_ARQR
    {
        public long qrUserId { get; set; }

        public int ID { get; set; }
        public string HeadImgUrl { get; set; }
        public string UserName { get; set; }

        public float Rate { get; set; }

        public float ParentCommissionRate { get; set; }

        public UserStatus UserStatus { get; set; }

        public bool IsCurrent { get; set; }

        public string ParentOpenId { get; set; }

        public float MarketRate { get; set; }

        public int TotalMember { get; set; }

        public float TotalAmount { get; set; }
    }
}
