using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBWX.Models.InParameter;
using IQBCore.IQBWX.Models.OutParameter;
using IQBWX.DataBase;
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
        IQBLog log = new IQBLog();
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
                log.log("getQR Error" + ex.Message);
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
            ssrQR.QRImgUrl = Picurl;

            return ssrQR;
        }

        [HttpPost]
        public SSOQR CreatePayQRAuth([FromBody]InQR inQR)
        {
            AccessToken token = this.getToken();
            SSOQR ssrQR = new SSOQR();

            inQR.QRId = IQBConstant.WXQR_IQBPAY_PREFIX + inQR.QRId;

            WXQRResult resObj = this.getQR("", token.access_token, inQR.QRId, false);
         
            string Picurl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + resObj.ticket + "";
            ssrQR.QRImgUrl = Picurl;
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
                IQBLog log = new IQBLog();
                log.log("downloadQR Error:" + ex.Message);
                log.log("downloadQR InnerError:" + ex.InnerException.Message);
               
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
    }
}
