using IQBCore.IQBPay.BaseEnum;
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

        public string UserName { get; set; }

        public string AliPayAccount { get; set; }

       public bool IsUserOrder { get; set; }


       
       public bool IsAdmin
        {
            get {
                return Phone == "13482710060";
            }
        }

      //  public string O2ONo { get; set; }
    }
}
