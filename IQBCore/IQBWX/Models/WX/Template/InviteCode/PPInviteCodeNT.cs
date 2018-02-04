using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.InviteCode
{
    public class PPInviteCodeNT : Notification<PPInviteCodeTemplate>
    {
        private int _OrigNum, _CurNum;
        private string _OpenId;
        public PPInviteCodeNT(string accessToken,int origNum,int curNum,string opneId) : base(accessToken)
        {
            _CurNum = curNum;
            _OrigNum = origNum;
            _OpenId = opneId;
        }

        protected override PPInviteCodeTemplate SetData()
        {
            PPInviteCodeTemplate data = new PPInviteCodeTemplate();
            data = data.GenerateData(_OpenId, _OrigNum,_CurNum);
            return data;
        }
    }
}
