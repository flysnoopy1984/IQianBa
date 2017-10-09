using IQBCore.IQBPay.Models.QR;
using IQBWX.BLL.ExternalWeb;
using IQBWX.Common;
using IQBWX.DataBase.IQBPay;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;
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

        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult Pay(string Id)
        {
            ViewBag.QRUserId = Id;
            return View();
        }

        public ActionResult Auth_Store()
        {
            return View();
        }

    }
}