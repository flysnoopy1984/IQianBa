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
        public int ID { get; set; }
        public float Rate { get; set; }

        public string AliPayAccount { get; set; }

        public bool IsAutoTransfer { get; set; }

        public string ParentOpenId { get; set; }

        public float ParentCommissionRate { get; set; }

        public int StoreId { get; set; }

        public UserStatus UserStatus { get; set; }

        public int QrUserId { get; set; }


    }

}
