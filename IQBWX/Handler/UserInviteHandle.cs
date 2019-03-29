using IQBCore.Common.Helper;
using IQBWX.DataBase;
using IQBWX.Models.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxPayAPI;

namespace IQBWX.Handler
{
    /// <summary>
    /// 处理各种渠道邀请码
    /// </summary>
    public class UserInviteHandle
    {

        public static IQBWX.Models.User.EUserInfo GetUserInfoFromWX(string openId)
        {
            JsApiPay jsApiPay = new JsApiPay();
            string accessToken = JsApiPay.GetAccessToken();
            string url_userInfo = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}",
            accessToken, openId);
          

            var wxUser = IQBCore.Common.Helper.HttpHelper.Get<WXUserInfo>(url_userInfo,true);
            IQBWX.Models.User.EUserInfo ui = new IQBWX.Models.User.EUserInfo(wxUser);
            return ui;
        }
        public static string ShuaDan(WXMessage msg)
        { 
            using (UserContent db = new UserContent())
            {
               var openId = msg.FromUserName;

               IQBWX.Models.User.EUserInfo ui = db.Get(openId);
                if (ui == null)
                {
                    try
                    {
                        ui = GetUserInfoFromWX(openId);
                        ui.ScanChannel = IQBCore.IQBWX.BaseEnum.UserScanChannel.SD;
                        db.UserInfo.Add(ui);
                        db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        NLogHelper.ErrorTxt("【UserInviteHandle】ShuaDan:" + ex.Message);
                    }
                    return $"{ui.nickname}，您好！感谢您的关注！";
                }
                else
                {
                   
                  
                    ui.ScanChannel = IQBCore.IQBWX.BaseEnum.UserScanChannel.SD;
                    db.SaveChanges();
                    return $"{ui.nickname}，欢迎您回来！";
                }
            }
        }
    }
}