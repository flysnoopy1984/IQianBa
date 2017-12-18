using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBWX.Controllers
{
    public class ValidController : Controller
    {
        // GET: Valid
        public ActionResult Index()
        {
            return View();
        }

        public JqueryValid CheckNum8_12()
        {
            JqueryValid result = new JqueryValid();

            return result;
        }
    }
}