using IQBCore.Common.Helper;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using IQBCore.IQBPay.BaseEnum;

namespace IQBPay.Controllers
{
    public class BaseController : Controller
    {
        public static ESysConfig _ESysConfig;
        public static EAliPayApplication _App;
        public IQBLog _Log;

        public IQBLog Log
        {
            get
            {
                if (_Log == null)
                    _Log = new IQBLog();
                return _Log;
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

      

        public BaseController()
        {
            
        }

        protected void SetOpenId(string openId)
        {
            Session["OpenId"] = openId;
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

        public string UserHeaderImg
        {
            get
            {
                return (string)Session["UserHeaderImg"];
            }
            set
            {
                Session["UserHeaderImg"] = value;
            }

        }

        public UserRole UserRole
        {
            get {
                return (UserRole)Enum.Parse(typeof(UserRole),Convert.ToString(Session["UserRole"]));
            }
            set
            {
                Session["UserRole"] = value;
            }
        }


    }
}
