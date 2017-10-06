using IQBCore.IQBPay.Models.QR;
using IQBWX.BLL.ExternalWeb;
using IQBWX.Common;
using IQBWX.DataBase.IQBPay;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace IQBWX.Controllers
{
    public class PPController : WXBaseController
    {
        private IQBLog _Log;

        public PPController()
        {
            _Log = new IQBLog();
        }


        /// <summary>
        /// act = 1 开始授权
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        // 用户授权
        public ActionResult Auth_AR(string Id, string act)
        {
            long qrId = 0;
            EQRInfo qr = null;
            // return View();

            string OpenId = this.GetOpenId(true, true);

            if (string.IsNullOrEmpty(Id) || !long.TryParse(Id, out qrId))
            {
                _Log.log("Auth_AR 没有 Id");
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.NormalErrorNoButton, ErrorMsg = "【传入的Id值不正确】无法授权，请联系平台" });

            }

            using (AliPayContent db = new AliPayContent())
            {
                qr = db.QR_GetById(qrId, IQBCore.IQBPay.BaseEnum.QRType.ARAuth);
            }
            if (qr == null)
            {
                _Log.log("Auth No QR");
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.NormalErrorNoButton, ErrorMsg = "【授权码不存在】无法授权，请联系平台！" });

            }
            ViewBag.Rate = qr.Rate;
            ViewBag.AuthUrl = "/PP/Auth_AR?Id="+qr.ID+"&act=1";

            if (act == "1")
            {
              
                JsApiPay jsApiPay = new JsApiPay(this.HttpContext);
                jsApiPay.GetOpenidAndAccessToken(false);

                string url_userInfo = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", jsApiPay.access_token, jsApiPay.openid);

                WXUserInfo wxUser = HttpHelper.Get<WXUserInfo>(url_userInfo, true);
               

                ExtWebPay exWeb = new ExtWebPay();
                EUserInfo ui = new EUserInfo();
                ui.openid = wxUser.openid;
                ui.nickname = wxUser.nickname;
                ui.headimgurl = wxUser.headimgurl;
                string result = exWeb.regeisterWebMember(ui);
                if (result.Contains("OK") || result.Contains("EXIST"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.NormalErrorNoButton, ErrorMsg = result });
                }
            }

            return View();
            
        }

        public ActionResult YunLong()
        {
            return View();
        }

        public ActionResult Demo()
        {
            return View();
        }

    }
}