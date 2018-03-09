using IQBCore.Common.Helper;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.Store;
using WxPayAPI;
using IQBCore.Model;
using System.Configuration;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.User;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.Common.Constant;

namespace IQBPay.Controllers
{
    public class BaseController : Controller
    {
    
        private static EAliPayApplication _App;
        private static EAliPayApplication _SubApp;

        private static EStoreInfo _SubAccount;
        private static EGlobalConfig _GlobelConfig;
        private IQBLog _Log;


       
        public IQBLog Log
        {
            get
            {
                if (_Log == null)
                    _Log = new IQBLog();
                return _Log;
            }
        }

        public static void CleanApp()
        {
            _App = null;
            _SubApp = null;
        }

       

        public static void CleanSubAccount()
        {
            _App = null;
        }

        public static EStoreInfo SubAccount
        {
            get
            {
                if(_SubAccount == null)
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        _SubAccount = db.DBStoreInfo.Where(a => a.IsReceiveAccount == true).FirstOrDefault();
                        if (_SubAccount == null)
                        {
                            throw new Exception("没有设置收款账户");
                        }
                    }  
                }
                return _SubAccount;
            }
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


        public static EAliPayApplication App
        {
            get
            {
                if (_App == null)
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        _App = db.DBAliPayApp.Where(a => a.IsCurrent == true).FirstOrDefault();
                        if(_App == null)
                        {
                            throw new Exception("没有当前应用");
                        }
                    }
                }
                return _App;
            }
        }

        /// <summary>
        /// 转账的App应用
        /// </summary>
        public static EAliPayApplication SubApp
        {
            get
            {
                if (_SubApp == null)
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        _SubApp = db.DBAliPayApp.Where(a => a.IsSubAccount == true).FirstOrDefault();
                        if (_SubApp == null)
                        {
                            throw new Exception("没有Sub应用");
                        }
                    }
                }
                return _SubApp;
            }
        }



        public BaseController()
        {
            
        }
        #region O2O

        public string GetBuyerPhone()
        {
            string buyerPhone = null;
            O2OBuyerSession buyerSession = Session["BuyerSession"] as O2OBuyerSession;
            if (buyerSession == null)
            {
                buyerPhone = CookieHelper.getCookie(IQBConstant.ck_O2OBuyerPhone);
                return buyerPhone;

            }
            else
                return buyerSession.Phone;

        }

        public string GetCurrentOrder(string BuyerPhone, AliPayContent db=null)
        {
            var sql = @"select top 1 o.O2ONo from O2OOrder as o
	                where o.UserPhone = '{0}'
	                order by o.CreateDateTime desc";

            sql = string.Format(sql, BuyerPhone);
            string OrderNo = null;
            if(db == null)
            {
                db = new AliPayContent();
              
            }
            OrderNo = db.Database.SqlQuery<string>(sql).FirstOrDefault();
            db.Dispose();


            return OrderNo;


        }
        #endregion


        public  void SetBuyerCookie(EO2OBuyer buyer)
        {
            CookieHelper.setCookie(IQBConstant.ck_O2OBuyerPhone, buyer.Phone);
            CookieHelper.setCookie(IQBConstant.ck_O2OReceiveAccount, buyer.ReceiveAccount);
        }
        public void SetBuyerSession(EO2OBuyer buyer)
        {
            O2OBuyerSession buyerSession = new O2OBuyerSession();
            buyerSession.AliPayAccount = buyer.ReceiveAccount;
            buyerSession.Phone = buyer.Phone;
            buyerSession.BuyerId = buyer.Id;
            Session["BuyerSession"] = buyerSession;
        }
       
        protected void SetUserSession(string openId)
        {
            using (AliPayContent db = new AliPayContent())
            {
                string sql = string.Format("select Id,OpenId,UserRole,Headimgurl,Name from userInfo where OpenId ='{0}'", openId);
                UserSession userSession = db.Database.SqlQuery<UserSession>(sql).FirstOrDefault();
                Session["UserSession"] = userSession;
            }
        }

        protected void SetUserSession(EUserInfo ui)
        {
            UserSession userSession = new UserSession();
            userSession.InitFromUser(ui);
            Session["UserSession"] = userSession;
        }

        protected UserSession GetUserSession()
        {
            UserSession userSession = Session["UserSession"] as UserSession;
            if (userSession == null)
            {
                userSession = new UserSession();
                userSession.UserRole = UserRole.NormalUser;
                userSession.O2OUserRole = O2OUserRole.User;
            }
            return userSession;
        }

        public void ExitSession()
        {
            Session["UserSession"] = null;
        }



        protected string GetOpenId(bool isTest = false)
        {
            if (isTest) return "orKUAw16WK0BmflDLiBYsR-Kh5bE";
            string openId = (string)Session["OpenId"];
            if (string.IsNullOrEmpty(openId))
            {

            }
            return openId;
        }

        //public string UserHeaderImg
        //{
        //    get
        //    {
        //        return (string)Session["UserHeaderImg"];
        //    }
        //    set
        //    {
        //        Session["UserHeaderImg"] = value;
        //    }

        //}

        //public UserRole UserRole
        //{
        //    get {
        //        return (UserRole)Enum.Parse(typeof(UserRole),Convert.ToString(Session["UserRole"]));
        //    }
        //    set
        //    {
        //        Session["UserRole"] = value;
        //    }
        //}

        public string getAccessToken(Boolean isRefresh = false)
        {
            string accessToken = (string)Session["AccessToken"];


            if (isRefresh || string.IsNullOrEmpty(accessToken))
            {
                accessToken = JsApiPay.GetAccessToken();
            }
            return accessToken;

        }

        public ActionResult MenuList()
        {
            UserSession userSession = GetUserSession();
            
            ViewBag.UserRole = Convert.ToInt32(userSession.UserRole);
            ViewBag.O2OUserRole = Convert.ToInt32(userSession.O2OUserRole);
            return PartialView("MenuList");
        }

        public Boolean IsTestMode
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["TestModel"]);
            }
        }
        public ActionResult ErrorResult(string msg)
        {
            OutAPIResult result = new OutAPIResult();
            result.ErrorMsg = msg;
            result.IsSuccess = false;
            return Json(result);

        }

    }
}
