using IQBCore.IQBPay.Models.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.InviteCode
{
    /// <summary>
    /// 原来用作邀请码名额，现用于费率变更
    /// </summary>
    public class PPInviteCodeNT : Notification<PPInviteCodeTemplate>
    {
        private List<SFee> _FeeList;
        private string _OpenId;
        public PPInviteCodeNT(string accessToken, List<SFee> feeList, string opneId) : base(accessToken)
        {
            _FeeList = feeList;
             _OpenId = opneId;
        }

        protected override PPInviteCodeTemplate SetData()
        {
            PPInviteCodeTemplate data = new PPInviteCodeTemplate();
            data = data.GenerateData(_OpenId, _FeeList);
            return data;
        }
    }
}
