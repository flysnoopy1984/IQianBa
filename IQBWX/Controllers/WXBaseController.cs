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
using IQBCore.IQBWX.Const;
using IQBWX.DataBase.IQBPay;
using IQBCore.IQBPay.Models.Sys;
using IQBCore.Model;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Common.Helper;

namespace IQBWX.Controllers
{
    
    public abstract class WXBaseController : Controller
    {
        protected string CodeUrlFormat = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
        List<string> filterUrl;
        public IQBLog log;
        private static EGlobalConfig _GlobelConfig;

        public static bool RefreshSession { get; set; }

        

        public WXBaseController()
        {
            log = new IQBLog();
        }

        public ActionResult ErrorResult(string msg)
        {
            OutAPIResult result = new OutAPIResult();
            result.ErrorMsg = msg;
            result.IsSuccess = false;
            return Json(result);

        }

        public static EGlobalConfig GlobalConfig
        {
            get
            {
                if (_GlobelConfig == null)
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        _GlobelConfig = db.DBGlobalConfig.FirstOrDefault();
                        if (_GlobelConfig == null)
                        {
                            _GlobelConfig = new EGlobalConfig();
                            _GlobelConfig.Init();
                            db.DBGlobalConfig.Add(_GlobelConfig);
                            db.SaveChanges();
                        }
                    }
                }
                return _GlobelConfig;
            }
            set
            {
                _GlobelConfig = value;
            }
        }


        public string CheckPPUserRole(string openId)
        {
         

            using (AliPayContent db = new AliPayContent())
            {
                IQBCore.IQBPay.Models.User.EUserInfo ui = db.DBUserInfo.Where(u => u.OpenId == openId).FirstOrDefault();
                if (ui == null)
                {
                    return "网站已经关闭";
                }
                if(ui.UserRole < IQBCore.IQBPay.BaseEnum.UserRole.Agent)
                {
                    return "您没有权限";
                }
            }
            return "OK";
        }

        public string getAccessToken(Boolean isRefresh= false)
        {
            string accessToken = (string)Session[IQBWXConst.SessionToken];
           
            
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
            return string.Format(CodeUrlFormat, ConfigurationManager.AppSettings["WXAppId"],reUrl);

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
            object obj = Session[IQBWXConst.SessionUserId];
            int userId=0;
            if(obj ==null)
            {
                using (UserContent db = new UserContent())
                {
                    string openId = this.GetOpenId();
                    if(!string.IsNullOrEmpty(openId))
                    { 
                        userId = db.GetUserId(this.GetOpenId());
                        Session[IQBWXConst.SessionUserId] = userId;
                    }
                }
            }
            else
            {
                userId = Convert.ToInt32(Session[IQBWXConst.SessionUserId]);
            }
            return userId;
        }


        public UserSession UserSession
        {
            get
            {
                UserSession userSession = Session["UserSession"] as UserSession;
               
                if (userSession ==null || RefreshSession == true)
                {
                    RefreshSession = false;
                    userSession = new UserSession();
                    string openId = this.GetOpenId();
                   
                    using (AliPayContent db = new AliPayContent())
                    {
                        IQBCore.IQBPay.Models.User.EUserInfo ui =   db.DBUserInfo.Where(u => u.OpenId == openId).FirstOrDefault();
                        if (ui != null)
                        {
                            userSession.InitFromUser(ui);

                            Session["UserSession"] = userSession;
                        }
                    }
                }
                return userSession;
            }
            set { Session["UserSession"] = value; }
        }

        public void InitProfilePage()
        {
            ViewBag.Headimgurl = UserSession.Headimgurl;
            ViewBag.ShowName = UserSession.Name ;
            ViewBag.OpenId = UserSession.OpenId;
            ViewBag.UserRole = Convert.ToInt32(UserSession.UserRole);
           


            if (UserSession.UserRole == IQBCore.IQBPay.BaseEnum.UserRole.DiamondAgent)
            {
                ViewBag.UserRoleImg = "/Content/images/zs_blueBK.png";
            }
            else
            {
                ViewBag.UserRoleImg = "/Content/images/grzx_03.jpg";
            }

        }

        protected string GetOpenId(bool isTest = false,bool IsforOpenId = true)
        {
            bool isDev = Convert.ToBoolean(ConfigurationManager.AppSettings["DevMode"]);
          if(isDev) return "o3nwE0i_Z9mpbZ22KdOTWeALXaus";


            //Jacky
          //   if (isDev) return "o3nwE0qI_cOkirmh_qbGGG-5G6B0";
             //平台
         //   if (isDev) return "o3nwE0jrONff65oS-_W96ErKcaa0";

            string openId = (string)Session[IQBWXConst.SessionOpenId];
            if (string.IsNullOrEmpty(openId))
            {
                JsApiPay jsApiPay = new JsApiPay(this.HttpContext);
                jsApiPay.GetOpenidAndAccessToken(IsforOpenId);
                openId = jsApiPay.openid;
                Session[IQBWXConst.SessionOpenId] = openId;
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

            Session[IQBWXConst.SessionOpenId] = openId;
            Session[IQBWXConst.SessionToken] = acccess;

            WXParameter.AccessToke = acccess;
            WXParameter.OpenId = openId;
            return WXParameter;
        }

    }
}