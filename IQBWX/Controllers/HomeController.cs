using IQBWX.BLL;
using IQBWX.Common;
using IQBWX.DataBase;
using IQBWX.Models.JsonData;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using IQBWX.Models.WX.Template;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WxPayAPI;

namespace IQBWX.Controllers
{
    public class HomeController : WXBaseController
    {

        public string ErrorMsg;
       
        // GET: Home
        public ActionResult Index()
        {
           return RedirectToRoute(new { Controller = "Home", Action = "ErrorMessage" });
            //JsApiPay jsApiPay = new JsApiPay(HttpContext);
            //IQBLog log = new IQBLog();
            //try
            //{
            //    jsApiPay.GetOpenidAndAccessToken();

            //    string url_userInfo = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", jsApiPay.access_token, jsApiPay.openid);

            //    WXUserInfo wxUser = HttpHelper.Get<WXUserInfo>(url_userInfo);             

            //    return View(wxUser);
            //}
            //catch(Exception ex)
            //{
            //    log.log(@"Home\Index error:" + ex.Message);

            //}

            return View();

        }      
        public ActionResult Message()
        {
            IQBLog log = new IQBLog();
          
            try
            {
                string echostr = Request.QueryString["echostr"];
               
                if (string.IsNullOrEmpty(echostr))
                {
                   
                    StreamReader reader = new StreamReader(Request.InputStream);
                    string strXml = reader.ReadToEnd();
                    log.log("Message strXml:" + strXml);
                    if (string.IsNullOrEmpty(strXml))
                        return View();
               

                    WXMessage wxMsg = new WXMessage();
                    wxMsg.LoadXml(strXml); 
                    MenuEvents menuEvent= new MenuEvents();
                    switch (wxMsg.Event)
                    {
                        case "view":                      
                            break;
                        case "click":
                            menuEvent.ClickHandler(wxMsg);
                            break;
                        case "scan":
                            //如果是扫描登录
                            if (!menuEvent.WXScanLogin(wxMsg,this))             
                            menuEvent.ScanHandler(wxMsg);
                            break;
                        case "subscribe":
                            if (!menuEvent.WXScanLogin(wxMsg,this))
                             
                            menuEvent.SubscribeHandler(wxMsg,this);
                            break;

                        default:
                        break;
                    }
                    log.log("Message: " + menuEvent. ResponseXml);
                    if (menuEvent.ResponseXml != null)
                        return Content(menuEvent.ResponseXml);

                    return View();

                }
                else
                    return Content(echostr);
            }
            catch(Exception ex)
            {
              
                log.log("Message Error:"+ ex.Message);
                log.log("Message Error:" + ex.StackTrace);
            }
            return View();

        }

        public ActionResult ErrorMessage()
        {
            string code = Request.QueryString["code"];
            jsonError data=null;
            if (!string.IsNullOrEmpty(code))
            {
                Errorcode ec = (Errorcode)Enum.Parse(typeof(Errorcode), code);
                if(ec == Errorcode.NormalErrorNoButton)
                {
                    data = new jsonError();
                    data.errorCode = Convert.ToInt32(ec);
                    data.errorMsg = Request.QueryString["ErrorMsg"];

                }
                else
                { 
                    data = jsonError.GetErrorObj(ec);
                }
                return View(data);                       
                
            }
            if(data ==null)
            {
                data = new jsonError();
            }
            return View(data);

        }
        public ActionResult Menus(string actionId)
        {
            return View();
        }

        public ActionResult WXLoginWAP()
        {
            IQBLog log = new IQBLog();
          
            string entry = Request.QueryString["entry"]; 
            string openId = this.GetOpenId(true);
            log.log("WXLoginWAP entry:" + entry);
            log.log("WXLoginWAP openId:" + openId);
            if (entry=="1")
                return Redirect("http://book.iqianba.cn/member/wxlogin.php?iswaptouch=1&openId="+openId);
            else if(entry=="2")
                return Redirect("http://book.iqianba.cn/member/wxlogin.php?iswap=1&openId=" + openId);
            else
                return RedirectToRoute(new { Controller = "Home", action = "ErrorMessage", code = Errorcode.OpenIdNotFound });
            

        }


        public ActionResult IndexWxLogin()
        {
          
            return View();
        }


        public ActionResult DBInit(string id)
        {
            if (id == "1")
            {
                using (UserContent db = new UserContent())
                {
                    db.Get("111");
                    db.GetMemberInfoByOpenId("111");
                }
                  
            }
        
           return View();
        }
    }
}