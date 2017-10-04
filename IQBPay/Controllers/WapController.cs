using IQBPay.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace IQBPay.Controllers
{
    public class WapController : BaseController
    {
        // GET: Wap
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pay()
        {
            return View();
        }

        public ActionResult Auth_AR()
        {
            JsApiPay JsApiPay = new JsApiPay(this.HttpContext);
            JsApiPay.GetOpenidAndAccessToken(true);

            return Content(JsApiPay.openid);
           
        }
    }
}