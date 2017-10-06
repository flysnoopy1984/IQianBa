using IQBWX.Common;
using IQBWX.DataBase;
using IQBWX.Models.WX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;
using System.Web.Routing;
using IQBWX.WebCore;

namespace IQBWX.Controllers
{
    
    public abstract class WXBaseController : Controller
    {
        protected string CodeUrlFormat = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
        List<string> filterUrl;
        public WXBaseController()
        {
            
        }

        public string getAccessToken(Boolean isRefresh= false)
        {
            string accessToken = (string)Session[IQBConst.SessionToken];
           
            
            if (isRefresh || string.IsNullOrEmpty(accessToken))
            {
                accessToken =JsApiPay.GetAccessToken();
            }
            return accessToken;

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(filterUrl == null)
            {
                filterUrl = new List<string>();
                filterUrl.Add(Url.Action("ErrorMessage", "Home"));
                filterUrl.Add(Url.Action("Message", "Home"));
                filterUrl.Add(Url.Action("Menus", "Home"));
                filterUrl.Add(Url.Action("DBInit", "Home"));
            }
         
           string gUrl = Url.Action("ErrorMessage", "Home");
           Boolean WebMaintain = Convert.ToBoolean(ConfigurationManager.AppSettings["WebMaintain"]);
            if (WebMaintain)
            {
                if(!filterUrl.Contains(filterContext.HttpContext.Request.Url.LocalPath))                
                    filterContext.Result = RedirectToRoute(new { Controller = "Home", Action = "ErrorMessage",code="9999"});
            } 
            base.OnActionExecuting(filterContext);
        }


        protected string getCodeUrl(string reUrl)
        {
           
            Boolean WebMaintain = Convert.ToBoolean(ConfigurationManager.AppSettings["WebMaintain"]);
            return string.Format(CodeUrlFormat,
                                             ConfigurationManager.AppSettings["WXAppId"],
                                             reUrl);

        }

        protected Boolean CheckIsMember()
        {
            using (UserContent db = new UserContent())
            {
                return db.IsMember(this.GetOpenId());
            }
        }

        protected int GetUserId()
        {
            object obj = Session[IQBWX.Common.IQBConst.SessionUserId];
            int userId=0;
            if(obj ==null)
            {
                using (UserContent db = new UserContent())
                {
                    string openId = this.GetOpenId();
                    if(!string.IsNullOrEmpty(openId))
                    { 
                        userId = db.GetUserId(this.GetOpenId());
                        Session[IQBWX.Common.IQBConst.SessionUserId] = userId;
                    }
                }
            }
            else
            {
                userId = Convert.ToInt32(Session[IQBWX.Common.IQBConst.SessionUserId]);
            }
            return userId;
        }


        protected string GetOpenId(bool isTest = false,bool IsforOpenId = true)
        {
            if (isTest) return "orKUAw16WK0BmflDLiBYsR-Kh5bE";
            string openId = (string)Session[IQBConst.SessionOpenId];
            if (string.IsNullOrEmpty(openId))
            {
                JsApiPay jsApiPay = new JsApiPay(this.HttpContext);
                jsApiPay.GetOpenidAndAccessToken(IsforOpenId);
                openId = jsApiPay.openid;
                Session[IQBConst.SessionOpenId] = openId;
            }
            return openId;
        }

        protected WXParameter GetUserInfoAccessCode()
        {
            JsApiPay jsApiPay = new JsApiPay(this.HttpContext);
            WXParameter WXParameter = new WXParameter();
            string openId = "";
            string acccess = "";//(string)Session[IQBConst.SessionToken];

            jsApiPay.GetOpenidAndAccessToken(false);
            openId = jsApiPay.openid;
            acccess = jsApiPay.access_token;

            Session[IQBConst.SessionOpenId] = openId;
            Session[IQBConst.SessionToken] = acccess;

            WXParameter.AccessToke = acccess;
            WXParameter.OpenId = openId;
            return WXParameter;
        }

    }
}