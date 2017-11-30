using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class TestController : Controller
    {
        
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return Redirect("/Main/WebEntry?openId=o3nwE0qI_cOkirmh_qbGGG-5G6B0");
        }
    }
}