using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace IQBWX.Controllers
{
    public class PPController : WXBaseController
    {
        // GET: PP
        public ActionResult Auth_AR()
        {
            JsApiPay JsApiPay = new JsApiPay(this.HttpContext);
            JsApiPay.GetOpenidAndAccessToken(true);

            return Content(JsApiPay.openid);
        }

      
    }
}