using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.InParameter
{
    public class InDoTransfer
    {
        /// <summary>
        /// 到款到哪个支付宝账户，song_fuwei@hotmai.com 
        /// </summary>
        public string AliPayAccount { get; set; }

        public string OpenId { get; set; }

        public float OrderAmount { get; set; }

        public float CommissionAmount { get; set; }

        public string accessToken { get; set; }



    }
}
