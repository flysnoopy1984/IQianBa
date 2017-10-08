using IQBCore.IQBPay.Models.Order;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            using (AliPayContent db = new AliPayContent())
            {
                EOrderInfo order = db.DBOrder.FirstOrDefault();

                return View();
            }
              
        }
    }
}