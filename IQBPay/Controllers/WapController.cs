using IQBPay.Core;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace IQBPay.Controllers
{
    public class WapController : BaseController
    {
        // GET: Wap
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyReceiveQR()
        {
            string FilePath = Request.QueryString["FilePath"];
            if (string.IsNullOrEmpty(FilePath))
                ViewBag.ImgSrc = "/Content/Images/noPic.jpg";
            else
            {
                ViewBag.ImgSrc = FilePath;
            }
            return View();
        }

        public ActionResult Auth_Store(string Id)
        {
          
            EAliPayApplication app = null;
          
            app = BaseController.App;
           
            return Redirect(app.AuthUrl_Store + "&Id=" + Id); 
            //return Content("OK");

        }
    }
}