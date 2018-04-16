using IQBCore.IQBPay.Models.O2O;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.SettleToUser
{
    public class SettleToUserNT : Notification<SettleToUserTemplate>
    {
        EO2OOrder _O2OOrder;
        string _OpenId;

        public SettleToUserNT(string accessToken, string toUserOpenId,EO2OOrder o2oOrder) : base(accessToken)
        {
            _OpenId = toUserOpenId;
            _O2OOrder = o2oOrder;
        }

        protected override SettleToUserTemplate SetData()
        {
            SettleToUserTemplate data = new SettleToUserTemplate();
            data = data.GenerateData(_OpenId, _O2OOrder);
            return data;
        }
    }
}
