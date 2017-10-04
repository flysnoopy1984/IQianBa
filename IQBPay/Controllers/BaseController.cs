using IQBPay.DataBase;
using IQBPay.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace IQBPay.Controllers
{
    public class BaseController : Controller
    {
        public static ESysConfig _ESysConfig;

        public static ESysConfig ESysConfig
        {
            get
            {
                if(_ESysConfig == null)
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        _ESysConfig = db.DBSysConfig.FirstOrDefault();
                       

                    }
                }
                return _ESysConfig;
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


    }
}
