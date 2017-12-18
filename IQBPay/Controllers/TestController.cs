using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Com.Alipay.Model;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Sys;
using IQBCore.IQBPay.Models.Tool;
using IQBPay.DataBase;
using Newtonsoft.Json;
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
            using (AliPayContent db = new AliPayContent())
            {
                EQRUser qrUser = db.DBQRUser.Where(o => o.OpenId == "o3nwE0qI_cOkirmh_qbGGG-5G6B0").FirstOrDefault();
                string url = "http://localhost:24068/api/userapi/CreateAgentQR_AR";
                string data = string.Format(@"ID={0}", qrUser.ID);
                string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
                OutAPIResult result = JsonConvert.DeserializeObject<OutAPIResult>(res);
            }
            //string url = "http://localhost:24068/api/userapi/register/";
            //string data = @"UserStatus=1&UserRole=1&Isadmin=false&name=平台服务客服&openId=o3nwE0jrONff65oS-_W96ErKcaa0&QRAuthId=10";
            //string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");

            return View();
        }
    }
}