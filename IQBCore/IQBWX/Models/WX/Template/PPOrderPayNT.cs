using IQBCore.IQBWX.Models.WX.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBCore.IQBWX.Models.WX.Template
{
    public class PPOrderPayNT : Notification<PPOrderPayTemplate>
    {
        IQBCore.IQBPay.Models.Order.EOrderInfo _ppOrder;
        string _OpenId;
        public PPOrderPayNT(string accessToken,
                            string OpenId,
                            IQBCore.IQBPay.Models.Order.EOrderInfo ppOrder) : base(accessToken)
        {
            _OpenId = OpenId;
            _ppOrder = ppOrder;
        }
        protected override PPOrderPayTemplate SetData()
        {
            PPOrderPayTemplate data = new PPOrderPayTemplate();
            data = data.GenerateData(_OpenId, _ppOrder);
            return data;
        }
    }
}