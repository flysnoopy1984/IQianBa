using IQBCore.IQBPay.Models.Order;
using IQBWX.DataBase.IQBPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBWX.Controllers
{
    public class PPAdminController : Controller
    {
        // GET: PPAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReviewPaySuccess()
        {
            string OrderNo = Request.QueryString["OrderNo"];
            EOrderInfo order = null;
            if (!string.IsNullOrEmpty(OrderNo))
            {
                using (AliPayContent db = new AliPayContent())
                {
                    order =  db.DBOrder.Where(a => a.OrderNo == OrderNo).FirstOrDefault();
                   
                }
            }
            if (order == null) order = new EOrderInfo();

           
            return View(order);
        }
    }
}