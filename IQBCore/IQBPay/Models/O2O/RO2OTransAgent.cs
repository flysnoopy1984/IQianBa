using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [NotMapped()]
    public class RO2OTransAgent: EO2OTransAgent
    {
        public string TransDescription
        {
            get
            {
                switch(this.TransactionType)
                {
                    case BaseEnum.TransactionType.Agent_Order_Comm:
                        return "订单佣金";
                    case BaseEnum.TransactionType.GetCash:
                        return "提现操作";
                    case BaseEnum.TransactionType.Parent_Comm:
                        return "二级佣金";
                    case BaseEnum.TransactionType.L3_Comm:
                        return "三级佣金";
                    default:
                        return "";

                }
            }
        }

        public string TransDateStr
        {
            get
            {
                return this.TransDateTime.ToString("yyyy-MM-dd");
            }
        }

    }
}
