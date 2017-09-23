using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            string WXurl =  ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
            ViewData["WXUrl"] = WXurl;
            return View();
        }
    }
}