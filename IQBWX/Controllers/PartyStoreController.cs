using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBWX.Controllers
{
    public class PartyStoreController : Controller
    {
        // GET: PartyStore
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tmall()
        {
            return View();

        }
    }
}