using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WxPayAPI;

namespace IQBCore.Controllers
{
    public class BaseController:Controller
    {
        protected string GetOpenId(bool isTest = false)
        {
            if (isTest) return "orKUAw16WK0BmflDLiBYsR-Kh5bE";
            string openId = (string)Session["OpenId"];
            if (string.IsNullOrEmpty(openId))
            {
               /* JsApiPay jsApiPay = new JsApiPay(this.HttpContext);
                jsApiPay.GetOpenidAndAccessToken();
                openId = jsApiPay.openid;
                Session["OpenId"] = openId;
                */
            }
            return openId;
        }
    }
}
