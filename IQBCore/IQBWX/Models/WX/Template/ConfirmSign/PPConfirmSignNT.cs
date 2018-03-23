using IQBCore.IQBPay.Models.O2O;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.ConfirmSign
{
    public class PPConfirmSignNT : Notification<PPConfirmSignTemplate>
    {
        RO2OOrder _O2OOrder;
        string _OpenId;
        public PPConfirmSignNT(string accessToken, RO2OOrder O2OOrder) : base(accessToken)
        {
            _O2OOrder = O2OOrder;
            _OpenId = O2OOrder.WHOpenId;
        }

        protected override PPConfirmSignTemplate SetData()
        {
            PPConfirmSignTemplate data = new PPConfirmSignTemplate();
            data = data.GenerateData(_OpenId, _O2OOrder);
            return data;
        }
    }
}
