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
            return "OK";
        }

        public override RExternalWebResult WXInfo(EUserInfo ui, WXMessage msg)
        {
            string res = regeisterWebMember(ui).Trim().ToUpper();
            RExternalWebResult result = new RExternalWebResult();

            if (res == "OK")
            {
                result.WXMessage += string.Format("亲爱的{0}，您已成功加入爱钱吧-支付平台！", ui.nickname);
                result.Status = 1;
                return result;
            }
            else if (res == "EXIST")
            {
                result.WXMessage += string.Format("欢迎回来,亲爱的{0}!", ui.nickname);
                result.Status = 2;
                return result;
            }
            else
            {
                result.WXMessage += "您第一次访问爱钱吧-支付平台，但系统未注册成功，请+QQ:2551038207联系我们，非常抱歉！";
                result.Status = -1;
                return result;
            }
        }
    }
}