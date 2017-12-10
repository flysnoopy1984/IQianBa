﻿using IQBPay.Core;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;
using LitJson;
using IQBCore.WxSDK;
using System.Security.Cryptography;
using System.Text;
using IQBCore.Common.Helper;

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

        [HttpPost]
        public ActionResult GetJSSDK(string AuthUrl)
        {
            string AccessToken = this.getAccessToken(true);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi",AccessToken);

            string result = HttpService.Get(url);
            //请求url以获取数据
            JsonData jd = JsonMapper.ToObject(result);

           string ticket =  (string)jd["ticket"];

            WXSign wxSign = new WXSign();
            wxSign.timestamp = WxPayApi.GenerateTimeStamp();
            wxSign.AppId = WxPayConfig.APPID;
            wxSign.nonceStr = WxPayApi.GenerateNonceStr();
            wxSign.jsapi_ticket = ticket;

            string sign = "jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}";
          
            sign = string.Format(sign, ticket, wxSign.nonceStr, wxSign.timestamp, AuthUrl);



            wxSign.signature = UtilityHelper.DoSHA1(sign, Encoding.Default).ToLower();

            return Json(wxSign);

          
        }

        public ActionResult UserVerification()
        {
            return View();
        }
    }
}