using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using IQBWX.Common;
using IQBWX.Models.Results;

namespace IQBWX.BLL.ExternalWeb
{
    public class ExtWebBook : BaseExternalWeb
    {
        public override string regeisterWebMember(EUserInfo ui)
        {
            string url = "http://book.iqianba.cn/member/wxRegNew.php";
            string data = "wxRegNew=1&userId={0}&uname={1}&sex={2}&openId={3}&faceurl={4}";
            string name = ui.nickname;
            if (name == null) name = ui.UserName;
            if (name == null) name = "wx" + ui.UserId.ToString().PadLeft(7, '0');

            data = string.Format(data, "wx" + ui.UserId.ToString().PadLeft(7, '0'), name, ui.sex, ui.openid, ui.headimgurl);

            string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
            return res;
        }

        public override RExternalWebResult WXInfo(EUserInfo ui, WXMessage msg)
        {
            string res = regeisterWebMember(ui).Trim().ToUpper();
            RExternalWebResult result = new RExternalWebResult();
            

            if (res == "OK")
            {
                result.WXMessage+= string.Format("亲爱的{0}，已为您注册为爱钱吧会员！", ui.nickname);
                result.Status = 1;
                return result;
            }
            else if (res == "EXIST")
            {
                result.WXMessage += string.Format("您账号已经和微信绑定，欢迎回来，{0}！", ui.nickname);
                result.Status = 2;
                return result;
            }
            else
            {
                result.WXMessage += "您第一次访问爱钱吧-书站，但系统未注册成功，请+QQ:2551038207联系我们，非常抱歉！";
                result.Status = -1;
                return result;
            }
            
        }
    }
}