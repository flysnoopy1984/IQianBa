using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class TestController : BaseController
    {
        
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (IsTestMode)
            {
                return Redirect("/Main/WebEntry?openId=o3nwE0qI_cOkirmh_qbGGG-5G6B0");
            }

            return RedirectToAction("Login", "Main");
        }
    }
}