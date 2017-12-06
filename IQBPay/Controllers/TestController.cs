using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Com.Alipay.Model;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.System;
using IQBCore.IQBPay.Models.Tool;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class TestController : BaseController
    {
        
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (IsTestMode)
            {
                return Redirect("/Main/WebEntry?openId=o3nwE0qI_cOkirmh_qbGGG-5G6B0");
            }

            return RedirectToAction("Login", "Main");
        }


        public ActionResult Tool_RPay()
        {


            return View();
        }

        [HttpPost]
        public ActionResult GetRPayQR()
        {
            List<ETool_QR> result = null;

            using (AliPayContent db = new AliPayContent())
            {
                result = db.DBTool_QR.ToList();
                if (result == null)
                    result = new List<ETool_QR>();
            }

            return Json(result);

        }

        [HttpPost]
        public ActionResult CreateRPayQR(string Amount,string couponAmt,string inputAmt)
        {
            AliPayManager payMag = new AliPayManager();
            string sellerId;
            string Res = "";
            ETool_QR qr = new ETool_QR();
            qr.CouponAmt = Convert.ToSingle(couponAmt);
            qr.InputAmt = Convert.ToSingle(inputAmt);
            qr.OrderAmt = Convert.ToSingle(Amount);

            using (AliPayContent db = new AliPayContent())
            {
                sellerId = db.DBStoreInfo.Where(s => s.IsReceiveAccount == true).FirstOrDefault().AliPayAccount;

                ResultEnum status;
                Res = payMag.PayF2F_ForR(BaseController.App, sellerId, Amount, qr,out status);
                if (status == ResultEnum.SUCCESS)
                {
                   
                  
                    qr.FilePath = Res;
                    db.DBTool_QR.Add(qr);
                    db.SaveChanges();
                }
            }
            return Json(qr);
        }

        public ActionResult JF()
        {
            EAliPayApplication app = BaseController.App;

            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
                      app.Merchant_Private_Key, "json", app.Version, app.SignType, app.Merchant_Public_key, "UTF-8", false);


            AlipayEbppBillAddRequest request = new AlipayEbppBillAddRequest();
            request.MerchantOrderNo = StringHelper.GenerateOrderNo();
            request.OrderType = "JF";
            request.SubOrderType = "ELECTRIC";
            request.ChargeInst = "BJCEB";
            request.BillKey = "3388102012376451";
            request.OwnerName = "织绫";
            request.PayAmount = "23.45";
            request.ServiceAmount = "8";
            request.BillDate = "201203";
            request.Mobile = "15987838584";
            request.TrafficLocation = "浙江,杭徽高速";
            request.TrafficRegulations = "窜红灯";
            request.BankBillNo = "20130916";
            //request.ExtendField = "{"key1":"value1","key2":"value2","key3":"value3","key4":"value4"}";
            AlipayEbppBillAddResponse response = alipayClient.Execute(request);

            return Content(response.Body);
        }
    }
}