using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.PaySuccessTellAdmin
{
    public class PaySuccessTellAdminNT : Notification<PaySuccessTellAdminTemplate>
    {
        IQBCore.IQBPay.Models.Order.EOrderInfo _ppOrder;
        public string _toOpenId;
        public PaySuccessTellAdminNT(string accessToken,
                                     string toOpenId,
                                     IQBCore.IQBPay.Models.Order.EOrderInfo ppOrder) : base(accessToken)
        {
            _toOpenId = toOpenId;
            _ppOrder = ppOrder;
        }

        protected override PaySuccessTellAdminTemplate SetData()
        {
            PaySuccessTellAdminTemplate data = new PaySuccessTellAdminTemplate();
            data = data.GenerateData(_toOpenId, _ppOrder);
            return data;
        }
    }
}
