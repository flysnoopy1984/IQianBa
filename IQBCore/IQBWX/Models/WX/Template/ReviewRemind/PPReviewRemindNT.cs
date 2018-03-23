using IQBCore.IQBPay.Models.O2O;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.ReviewRemind
{
    public class PPReviewRemindNT : Notification<PPReviewRemindTemplate>
    {
        RO2OOrder _O2OOrder;
        string _OpenId;
        public PPReviewRemindNT(string accessToken,string toUserOpenId, RO2OOrder O2OOrder) : base(accessToken)
        {
            _O2OOrder = O2OOrder;
            _OpenId = toUserOpenId;
        }

        protected override PPReviewRemindTemplate SetData()
        {
            PPReviewRemindTemplate data = new PPReviewRemindTemplate();
            data = data.GenerateData(_OpenId, _O2OOrder);
            return data;
        }
    }
}
