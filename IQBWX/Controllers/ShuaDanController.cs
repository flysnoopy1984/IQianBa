using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.OutParameter;
using IQBWX.DataBase;
using IQBWX.Models.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace IQBWX.Controllers
{
    public class ShuaDanController : Controller
    {
        // GET: ShuaDan
        public ActionResult Index()
        {
          //  return RedirectToAction("Reg", "ShuaDan", new { OpenId = "o3nwE0qI_cOkirmh_qbGGG-5G6B0" });
            return WXLogin();
        }

        public ActionResult Reg()
        {
            //string OpenId = Request.QueryString["OpenId"];
            //if(!string.IsNullOrEmpty(OpenId))
            //{
            //    using (UserContent db = new UserContent())
            //    {
            //        IQBWX.Models.User.EUserInfo ui = db.UserInfo.Where(u => u.openid == OpenId).FirstOrDefault();
            //        if (ui != null)
            //        {
            //            if (!string.IsNullOrEmpty(ui.PhoneNumber))
            //                return RedirectToAction("ErrorPage", "ShuaDan", new { errormsg = "感谢！您已登记！" });
            //        }
            //        else
            //        {
            //            return RedirectToAction("ErrorPage", "ShuaDan", new { errormsg = "请重新扫码进入！" });

            //        }

            //    }
            //}
        
            return View();
        }

        public ActionResult ErrorPage()
        {
            string msg = Request.QueryString["errormsg"];
            ViewBag.ErrorMsg = msg;
            return View();
        }

        [HttpPost]
        public ActionResult GetUserPhone(string phone,string openId)
        {
          //  string phone = Request["Phone"];

            OutAPIResult result = new OutAPIResult();
            try
            {
                if (string.IsNullOrEmpty(phone))
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "手机号为空";
                    result.IntMsg = -1;
                    return Json(result);
                }
                if (string.IsNullOrEmpty(openId))
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "请重新扫码进入！";
                    result.IntMsg = -2;
                    return Json(result);
                }

                using (UserContent db = new UserContent())
                {
                    IQBWX.Models.User.EUserInfo ui = db.UserInfo.Where(u => u.openid == openId).FirstOrDefault();
                    if(ui == null)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "请重新扫码进入！";
                        result.IntMsg = -2;
                        return Json(result);
                    }
                    else
                    {
                        ui.PhoneNumber = phone;
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;

            }
            return Json(result);
        }

        public ActionResult WXLogin()
        {
            JsApiPay wxAPI = new JsApiPay();
            string openId = "";
        
            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                //获取code码，以获取openid和access_token
                string code = Request.QueryString["code"];
                //  NLogHelper.InfoTxt("[strat GetOpenidAndAccessTokenFromCode] Code:"+code);
                wxAPI.GetOpenidAndAccessTokenFromCode(code);
                //  NLogHelper.InfoTxt("[end GetOpenidAndAccessTokenFromCode]");
                openId = wxAPI.openid;
                NLogHelper.InfoTxt("WXLogin openId:" + openId);
                if (!string.IsNullOrEmpty(wxAPI.openid))
                {
                    using (UserContent db = new UserContent())
                    {
                        IQBWX.Models.User.EUserInfo ui = db.UserInfo.Where(u => u.openid == wxAPI.openid).FirstOrDefault();
                        if (ui == null)
                        {
                            string url_userInfo = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", wxAPI.access_token, wxAPI.openid);
                            try
                            {
                                WXUserInfo wxUser = IQBCore.Common.Helper.HttpHelper.Get<WXUserInfo>(url_userInfo, false);
                                if (!string.IsNullOrEmpty(wxUser.openid))
                                {
                                    openId = wxUser.openid;
                                    ui = new Models.User.EUserInfo(wxUser);
                                 
                                    // 
                                    ui.ScanChannel = IQBCore.IQBWX.BaseEnum.UserScanChannel.SD;
                                    db.UserInfo.Add(ui);

                                    db.SaveChanges();
                                    //  NLogHelper.InfoTxt("Save New User:" + ui.openid);
                                }
                                else
                                    return RedirectToAction("ErrorMessage", "Home", new { msg = "微信获取用户信息错误" });
                            }
                            catch (Exception ex)
                            {

                                  NLogHelper.ErrorTxt("【shuaDan】WXLogin:" + ex.Message);
                            }

                        }
                        else
                        {
                            openId = ui.openid;
                            ui.ScanChannel = IQBCore.IQBWX.BaseEnum.UserScanChannel.SD;
                            db.SaveChanges();
                        }

                    }
                }
             //   NLogHelper.InfoTxt("OpenId:" + openId);
                return RedirectToAction("Reg", "ShuaDan", new { OpenId = openId });

            }
            else
            {
                var redirect_uri = System.Web.HttpUtility.UrlEncode("http://wx.iqianba.cn/ShuaDan/Index", System.Text.Encoding.UTF8);

                WxPayData data = new WxPayData();
                data.SetValue("appid", WxPayConfig.APPID);
                data.SetValue("redirect_uri", redirect_uri);
                data.SetValue("response_type", "code");
                data.SetValue("scope", "snsapi_userinfo");
                //if (IsForOpenId)
                //    data.SetValue("scope", "snsapi_base");
                //else
                //    data.SetValue("scope", "snsapi_userinfo");

                data.SetValue("state", "1" + "#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();



                return Redirect(url);

            }
        }
    }
}