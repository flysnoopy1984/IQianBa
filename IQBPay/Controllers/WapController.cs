using IQBPay.Core;
using IQBPay.DataBase;
using IQBPay.Models.QR;
using IQBPay.Models.System;
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

        public ActionResult Pay()
        {
            return View();
        }

        public ActionResult Auth_AR()
        {
            JsApiPay JsApiPay = new JsApiPay(this.HttpContext);
            JsApiPay.GetOpenidAndAccessToken(true);

            return Content(JsApiPay.openid);
           
        }

        public ActionResult Auth_Store(string Id)
        {
          
            EAliPayApplication app = null;
          
            app = BaseController.App;
            /*
            using (AliPayContent db = new AliPayContent())
            {
                qr = db.QR_GetById(QRId, Core.BaseEnum.QRType.StoreAuth);
                if (qr == null)
                    return Content("授权码不存在！");
                else if (qr.RecordStatus == Core.BaseEnum.RecordStatus.Blocked)
                    return Content("授权码已被使用");

                
                if (app == null)
                    return Content("平台配置错误，请联系站长");  
            }
          */
            return Redirect(app.AuthUrl_Store + "&Id=" + Id); 
            //return Content("OK");

        }
    }
}