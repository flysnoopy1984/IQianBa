using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OverView()
        {
            return View();
        }


        public ActionResult AgentOverView()
        {
            using (AliPayContent db = new AliPayContent())
            {

            }
            return View();
        }
    }
}