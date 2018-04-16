using IQBCore.IQBPay.Models.O2O;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.ReviewResult
{
    public class PPReviewResultNT : Notification<PPReviewResultTemplate>
    {
        RO2OOrder _O2OOrder;
        string _OpenId;
        
        public PPReviewResultNT(string accessToken, RO2OOrder O2OOrder) : base(accessToken)
        {
            _O2OOrder = O2OOrder;
            _OpenId = O2OOrder.AgentOpenId;
        }

        protected override PPReviewResultTemplate SetData()
        {
            PPReviewResultTemplate data = new PPReviewResultTemplate();
            data = data.GenerateData(_OpenId, _O2OOrder);
            return data;
        }
    }
}
