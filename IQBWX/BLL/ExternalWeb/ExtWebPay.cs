using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using IQBWX.Common;
using System.Configuration;

namespace IQBWX.BLL.ExternalWeb
{
    public class ExtWebPay : BaseExternalWeb
    {
        public override string regeisterWebMember(EUserInfo ui)
        {
            string url = ConfigurationManager.AppSettings["Site_IQBPay_Register"];
            string data = "UserStatus=1&UserRole=1&Isadmin=false&name={0}&openId={1}";
            string name = ui.nickname;
            if (name == null) name = ui.UserName;
            if (name == null) name = "wx" + ui.UserId.ToString().PadLeft(7, '0');

            data = string.Format(data, "wx" + ui.UserId.ToString().PadLeft(7, '0'), name,ui.openid);

            string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
            return res;
           
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
                result.WXMessage += "您第一次访问爱钱吧-支付平台，但系统未注册成功，请+QQ:2551038207联系我们，非常抱歉！/n";
                result.WXMessage += res;
                result.Status = -1;
                return result;
            }
        }
    }
}