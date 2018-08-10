using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBWX.Models.InParameter;
using IQBCore.IQBWX.Models.OutParameter;
using IQBWX.DataBase;
using IQBWX.DataBase.IQBPay;
using IQBWX.Models.JsonData;
using IQBWX.Models.Results;
using IQBWX.Models.WX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using WxPayAPI;

namespace IQBWX.Controllers
{
    public class WXController : ApiController
    {
      //  IQBLog log = new IQBLog();
        [HttpGet]
        public string MessageHandler()
        {
            return "111";
        }
        [HttpGet]
        public string Test()
        {
            return "test";
        } 
                    
        public AccessToken getToken()
        {           
              string tokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
              tokenUrl = string.Format(tokenUrl, WxPayConfig.APPID, WxPayConfig.APPSECRET);
              AccessToken token = HttpHelper.Get<AccessToken>(tokenUrl);
                
              return token;         
        }

        /// <summary>
        /// 二维码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public WXQRResult getQR(String account, string access_token,string ssoToken=null,bool isTemp = true)
        {
            WXQRResult resObj = null;
            try
            { 

                //获取数据的地址（微信提供）
                String url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + access_token;

                //发送给微信服务器的数据

                String jsonStr = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\":{\"scene\": {\"scene_id\":" + account + "}}}";
                if (!string.IsNullOrEmpty(ssoToken))
                {
                    if (isTemp)
                    {
                        jsonStr = "{\"expire_seconds\": \"180\",\"action_name\": \"QR_STR_SCENE\", \"action_info\":{\"scene\": {\"scene_str\":\"" + ssoToken + "\"}}}";
                    }
                    else
                    {
                        jsonStr = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\":{\"scene\": {\"scene_str\":\"" + ssoToken + "\"}}}";
                    }
                }

             //   log.log("getQR"+jsonStr);

                //post请求得到返回数据（这里是封装过的，就是普通的java post请求）
                String response = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, jsonStr);

                resObj = JsonConvert.DeserializeObject<WXQRResult>(response);
            }
            catch(Exception ex)
            {
               // log.log("getQR Error" + ex.Message);
            }
            return resObj;
        }

        [HttpGet]
        public SSOQR CreateSSOQR(string appId)
        {
            string ssoToken = StringHelper.GetSSOToken();
            SSOQR ssrQR = new SSOQR();
            ssrQR.ssoToken = ssoToken;

            AccessToken token = this.getToken();
            WXQRResult resObj = this.getQR("", token.access_token, ssoToken);
            resObj.ticket = Uri.EscapeDataString(resObj.ticket);
            string fileName = ssoToken + ".jpg";
            string Picurl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + resObj.ticket + "";

            using (WXContent db = new WXContent())
            {
                ESSOLogin entity = new ESSOLogin();
                entity.AppId = appId;
                entity.LoginStatus = LoginStatus.QRCreated;
                entity.ssoToken = ssoToken;
                entity.IsValidate = false;
                entity.RequireTime = Convert.ToDouble(DateTime.Now.ToString("yyyyMMddhhmmss"));
                entity.CreatedDate = DateTime.Today;
                db.InserSSOToken(entity);
            }
            ssrQR.TargetUrl = Picurl;

            return ssrQR;
        }

        [HttpPost]
        public SSOQR CreateInviteQR([FromBody]InQR inQR)
        {
            AccessToken token = this.getToken();
            SSOQR ssrQR = new SSOQR();
            inQR.QRId = IQBConstant.WXQR_IQBPAY_PREFIX + inQR.QRId;
            WXQRResult resObj = this.getQR("", token.access_token, inQR.QRId, false);
            string Picurl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + resObj.ticket + "";
            ssrQR.TargetUrl = Picurl;
         
            
            return ssrQR;
        }


        public string downloadQR(WXQRResult qrObj,string saveFileName)
        {
            string Picurl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + qrObj.ticket + "";
            string address = HttpContext.Current.Server.MapPath("~/Content/QRImg/" + saveFileName);

            try
            {
                
                bool isExist = File.Exists(address);
                if (isExist) return address;
              
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Picurl);
                req.Proxy = null;
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:11.0) Gecko/20100101 Firefox/11.0";
                req.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.8,zh-hk;q=0.6,ja;q=0.4,zh;q=0.2");
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";              
              
                Image img = Image.FromStream(req.GetResponse().GetResponseStream());
                img.Save(address);
                string bkAdree = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QRBKPath"]);
                Bitmap bkImg = new Bitmap(bkAdree);
                Bitmap finImg = ImgHelper.ImageWatermark(bkImg, img);
                finImg.Save(HttpContext.Current.Server.MapPath("~/Content/QRImg/bk_" + saveFileName));
                finImg.Dispose();
                img.Dispose();

                
            }
            catch(Exception ex)
            {
                throw ex;
               // IQBLog log = new IQBLog();
                //log.log("downloadQR Error:" + ex.Message);
                //log.log("downloadQR InnerError:" + ex.InnerException.Message);
               
            }
            return address;
        }

        [HttpPost]
        public string CreateMenu()
        {

            IQBLog log = new IQBLog();
            string responseResult = "";       

            try
            {
                string access_token = this.getToken().access_token;
                string posturl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + access_token;
                string menuStr = "";// " 菜单结构";
                using (StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"Content\json\menus.json")) 
                {
                    menuStr = sr.ReadToEnd();
                }        
                responseResult = HttpHelper.RequestUrlSendMsg(posturl, HttpHelper.HttpMethod.Post, menuStr);              
            }
            catch(Exception ex)
            {
                log.log("exception error:" + ex.Message);
            }
           return responseResult;           

        }

        [HttpGet]
        public RSSOResult WaitingScan(string ssoToken,string appId)
        {
            DateTime beginTime = DateTime.Now;
            DateTime endTime = beginTime;
            TimeSpan ts;
            ESSOLogin sso = null;
            bool IsLogin = false;
            RSSOResult result =new RSSOResult();
            result.ErrorMsg = "";

            while (!IsLogin)
            {
                
                using (WXContent db = new WXContent())
                {
                  

                    sso = db.FindSSOForScaned(ssoToken);
                    if(sso!=null)
                    {
                        //if (!WXBaseController.GlobalConfig.AllowRegister)
                        //{
                        //    result.ErrorMsg = "close";
                        //    result.ReturnUrl = ConfigurationManager.AppSettings["Site_IQBPay"];
                        //    return result;
                        //}

                        sso.IsValidate = false;
                        sso.LoginStatus = LoginStatus.Login;
                        db.SaveChanges();
                        
                        result.AppId = appId;
                        result.OpenId = sso.OpenId;
                        result.ssoToken = ssoToken;
                        if(appId == "pp")
                            result.ReturnUrl = ConfigurationManager.AppSettings["Site_IQBPay_WebEntry"];
                        else if(appId =="o2o")
                        {
                            result.ReturnUrl = ConfigurationManager.AppSettings["Site_IQBPay"]+ "O2OWap/Index?aoId=" + result.OpenId;
                        }
                        else
                            result.ReturnUrl = ConfigurationManager.AppSettings["Site_IQBBook"];

                        IsLogin = true;
                        break;
                    }
                }

                Thread.Sleep(500);
                endTime = DateTime.Now;
                ts = endTime - beginTime;
                if(ts.Seconds>120)
                {
                    result.ErrorMsg = "timeout";
                }
            }
            if (IsLogin)
                return result;
            return null;


        }


       
        /// <summary>
        /// 新版微信支付（微信公众好内支付）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public WxPayOrder Pay(InPayInfo payInfo)
        {
          //  WxPayOrder result = new WxPayOrder();

            WxPayOrder wxOrder = null;
            JsApiPay jsApiPay = new JsApiPay();

            try
            {
                string openId = this.Login();
               
                if(!string.IsNullOrEmpty(openId))
                {
                    NLogHelper.InfoTxt("Pay openId:" + openId);

                    string notifyUrl = ConfigurationManager.AppSettings["Site_WX"] + "api/wx/PayNotify";

                    string OrderNo = WxPayApi.GenerateOutTradeNo();
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult_YJ(payInfo.ItemDes, notifyUrl, OrderNo);
                    WxPayData wxJsApiParam = jsApiPay.GetJsApiParameters2();

                    wxOrder = new WxPayOrder()
                    {
                        appId = wxJsApiParam.GetValue("appId").ToString(),
                        nonceStr = wxJsApiParam.GetValue("nonceStr").ToString(),
                        package = wxJsApiParam.GetValue("package").ToString(),
                        paySign = wxJsApiParam.GetValue("paySign").ToString(),
                        signType = "MD5",
                        timeStamp = wxJsApiParam.GetValue("timeStamp").ToString(),
                    };
                }
               
            }
            catch
            {
                wxOrder = new WxPayOrder()
                {
                    appId = "",
                    nonceStr = "",
                    package = "",
                    paySign = "",
                    timeStamp = "",
                    signType = "MD5",

                };
            }

            return wxOrder;
        }

        public void PayNotify()
        {
            NLogHelper.InfoTxt("==============WXPayNotify=================");
        }

        public string Login()
        {
            JsApiPay wxAPI = new JsApiPay();
            string openId = "";
            string code = HttpContext.Current.Request.QueryString["code"];
            NLogHelper.InfoTxt("Login Code:" + code);
            if (!string.IsNullOrEmpty(code))
            {
                //获取code码，以获取openid和access_token
            //    code = Request.QueryString["code"];
                //  NLogHelper.InfoTxt("[strat GetOpenidAndAccessTokenFromCode] Code:"+code);
                wxAPI.GetOpenidAndAccessTokenFromCode(code);
                //  NLogHelper.InfoTxt("[end GetOpenidAndAccessTokenFromCode]");
                openId = wxAPI.openid;

                return openId;
                // NLogHelper.InfoTxt("RedirectToAction:" + rtnAction);
               // return HttpContext.Current.RedirectToAction(rtnAction, rtnController, new { OpenId = openId });

            }
            else
            {
                try

                {
                    var redirect_uri = System.Web.HttpUtility.UrlEncode("http://wx.iqianba.cn/api/wx/Login", System.Text.Encoding.UTF8);

                    WxPayData data = new WxPayData();
                    data.SetValue("appid", WxPayConfig_YJ.APPID);
                    data.SetValue("redirect_uri", redirect_uri);
                    data.SetValue("response_type", "code");
                    data.SetValue("scope", "snsapi_userinfo");

                    data.SetValue("state", "1" + "#wechat_redirect");
                    string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();

                    NLogHelper.InfoTxt("WX/Login:" + url);

                    HttpContext.Current.Response.Redirect(url, true);
                }
                catch
                {

                }
               

            }
            return "";
        }
    }
}
