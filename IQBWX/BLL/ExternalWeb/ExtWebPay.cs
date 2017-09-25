using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;

namespace IQBWX.BLL.ExternalWeb
{
    public class ExtWebPay : BaseExternalWeb
    {
        public override string regeisterWebMember(EUserInfo ui)
        {
            throw new NotImplementedException();
        }

        public override RExternalWebResult WXInfo(EUserInfo ui, WXMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}