using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class WapController : Controller
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
    }
}