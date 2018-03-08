using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Model
{
    public class O2OBuyerSession
    {

        public long BuyerId { get; set; }
        public string Phone { get; set; }

        public string AliPayAccount { get; set; }

        public long QRUserId { get; set; }

      //  public string O2ONo { get; set; }
    }
}
