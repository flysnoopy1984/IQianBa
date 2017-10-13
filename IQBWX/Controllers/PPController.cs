using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBWX.Models.WX.Template;
using IQBWX.BLL.ExternalWeb;
using IQBWX.BLL.NT;
using IQBWX.Common;
using IQBWX.DataBase.IQBPay;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using IQBWX.Models.WX.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace IQBWX.Controllers
{
    public class PPController : WXBaseController
    {
        private IQBLog _Log;

        public PPController()
        {
            _Log = new IQBLog();
        }


       
        public ActionResult YunLong()
        {
            return View();
        }

        public ActionResult Demo(string Id)
        {
           
            if (Id == "1")
            {
               
            }
             return View();
        }

        public ActionResult Pay(string Id)
        {
            ViewBag.QRUserId = Id;
            using (AliPayContent db = new AliPayContent())
            {
                IQBCore.IQBPay.Models.Order.EOrderInfo _ppOrder = db.DBOrderInfo.FirstOrDefault();
            }
            return View();
        }

        public ActionResult Auth_Store(string Rate)
        {
            ViewBag.Rate = Rate;
           
            return View();
        }
        public ActionResult AliPayAccount()
        {
            return View();
        }
        public ActionResult OrderList()
        {
            return View();
        }
        public ActionResult Settlement()
        {
            string accessToken = this.getAccessToken(true);
            IQBCore.IQBPay.Models.Order.EOrderInfo _ppOrder;
            using (AliPayContent db = new AliPayContent())
            {
                _ppOrder = db.DBOrderInfo.FirstOrDefault();
            }
            PPOrderPayNT notice = new PPOrderPayNT(accessToken, "orKUAw16WK0BmflDLiBYsR-Kh5bE", _ppOrder);
            return Content(notice.Push());
           
        }

    }
}