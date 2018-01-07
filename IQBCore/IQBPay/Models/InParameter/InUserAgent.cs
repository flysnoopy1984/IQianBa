using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.InParameter
{
    public class InUserAgent
    {
        public string OpenId { get; set; }
        public float Rate { get; set; }

        public string AliPayAccount { get; set; }

        public bool IsAutoTransfer { get; set; }

        public string ParentOpenId { get; set; }

        public string ParentName { get; set; }

        public float ParentCommissionRate { get; set; }

        public int StoreId { get; set; }

        public UserStatus UserStatus { get; set; }

        public UserRole UserRole { get; set; }

        public int QrUserId { get; set; }

        public float MarketRate { get; set; }


        public long QRInfoId { get; set; }

        public float QRInfo_Rate { get; set; }

        public float QRInfo_ParentCommissionRate { get; set; }

        public bool NeedFollowUp { get; set; }




    }

}
