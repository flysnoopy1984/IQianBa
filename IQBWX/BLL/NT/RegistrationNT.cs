using IQBCore.IQBWX.Models.WX.Template;
using IQBWX.Models.User;
using IQBWX.Models.WX.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.BLL.NT
{
    public class RegistrationNT : Notification<RegTemplate>
    {
        private EUserInfo _UserInfo;
        public RegistrationNT(EUserInfo ui,string accessToken) 
            : base(accessToken) {
            _UserInfo = ui;
        }

        protected override RegTemplate SetData()
        {
            RegTemplate data = new RegTemplate();
            data = data.GenerateData(_UserInfo.ParentOpenId, _UserInfo.nickname, _UserInfo.SubscribeDateTime);
            return data;
        }
    }
}