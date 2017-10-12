using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    [NotMapped()]
    public class ROrder_Transfer
    {
        public EOrderInfo Order { get; set; }
        
        public ETransferAmount Transfer { get; set; }

        public List<EOrderInfo> OrderList { get; set; }

        public List<ETransferAmount> TransferList { get; set; }

        /// <summary>
        /// 1 OK -1 没有数据 -2 没有主数据
        /// </summary>
        public int Result { get; set; }
    }
}
