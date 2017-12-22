using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;

using System.Configuration;
using IQBCore.Common.Helper;

namespace IQBWX.BLL.ExternalWeb
{
    public class ExtWebPay : BaseExternalWeb
    {
        IQBLog log = new IQBLog();
        public ExtWebPay()
        {

        }
       
        public override string regeisterWebMember(EUserInfo ui,long QRAuthId = 0)
        {
           
            try
            {
                string url = ConfigurationManager.AppSettings["Site_IQBPay_Register"];
                string data = "UserStatus=1&UserRole=1&Isadmin=false&name={0}&openId={1}&QRAuthId={3}&Headimgurl={2}";
                string name = ui.nickname;
                if (name == null) name = ui.UserName;
                if (name == null) name = "wx" + ui.UserId.ToString().PadLeft(7, '0');
               
                data = string.Format(data, name, ui.openid, ui.headimgurl, QRAuthId);
            //    log.log("regeisterWebMember Data: " + data);
                string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
                return res;
            }
            catch(Exception ex)
            {
                log.log("Regeister Pay Web Error" + ex.Message);
            }
            return null;
           
        }

        public override RExternalWebResult WXInfo(EUserInfo ui, WXMessage msg)
        {
            string res = regeisterWebMember(ui).Trim().ToUpper();
            log.log("ExtWebPay WXInfo:" + res);
            RExternalWebResult result = new RExternalWebResult();

            if (res.Contains("OK"))
            {
                result.WXMessage += string.Format("亲爱的{0}，欢迎您成功加入玉杰服务平台！", ui.nickname);
                result.Status = 1;
                return result;
            }
            else if (res.Contains("EXIST"))
            {
                result.WXMessage += string.Format("欢迎回来,亲爱的{0}!", ui.nickname);
                result.Status = 2;
                return result;
            }
            else
            {
                //result.WXMessage += "系统已关闭，请联系公众平台！/n";
                result.WXMessage += res;
                result.Status = -1;
                return result;
            }
        }
    }
}