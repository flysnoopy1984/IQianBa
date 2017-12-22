using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBWX.Models.OutParameter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBWX.Controllers
{
    public class TestController : Controller
    {
        
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestQR(string Id)
        {
            //using (AliPayContent db = new AliPayContent())
            //{
            //    EQRInfo qr = db.DBQRInfo.Where(a => a.ID == 66).FirstOrDefault();
            //    string url = "http://wx.iqianba.cn/api/wx/CreateInviteQR";
            //    string data = string.Format("QRId={0}&QRType={1}", qr.ID, qr.Type);
            //    string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
            //    SSOQR obj = JsonConvert.DeserializeObject<SSOQR>(res);
                return View();
            //}
        }
    }
}