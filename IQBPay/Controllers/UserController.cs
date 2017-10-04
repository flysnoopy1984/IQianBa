using IQBCore.Common.Helper;
using IQBPay.Core;
using IQBPay.DataBase;
using IQBPay.Models.QR;
using IQBPay.Models.Result;
using IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            string OpenId = this.GetOpenId(true);

            return View();
        }

        public ActionResult Get()
        {
           
             Action< string > mylog = null;
            string OpenId = this.GetOpenId(true);
          
            RUserInfo result = null;
            using (AliPayContent db = new AliPayContent())
            {
                try
                {
      
                    EUserInfo ui = db.DBUserInfo.Where(u => u.OpenId == OpenId).FirstOrDefault();

                    if (ui == null)
                    {
                        result = new RUserInfo();
                        result.QueryResult = false;
                        return Json(result);
                    }
                    EQRUser qrUser = db.DBQRUser.Where(a => a.QRId == ui.QRDefaultId).FirstOrDefault();
                  
                    result = new RUserInfo();
                    result.InitFromChild(ui);
                    result.Rate = qrUser.Rate;
                    result.QRFilePath = qrUser.FilePath;
                    result.QueryResult = true;
                    return Json(result);
                }
                catch (Exception ex)
                {
                    mylog = db.Database.Log;
                    throw ex;
                }
           
            }
          
          
           
        }

       
        public ActionResult Demo()
        {
           
            EUserInfo ui = new EUserInfo();
            ui.Headimgurl = "http://ssdakdla";
            // string url = "http://ap.iqianba.cn/api/userapi/register/";
            string url = "http://localhost:24068/api/userapi/register/";
            string data = "UserStatus=1&UserRole=1&Isadmin=false&name={0}&openId={1}&Headimgurl=aaaaaaaaaaa";
            string name = "22";
           
            data = string.Format(data, "wx" + ui.Id.ToString().PadLeft(7, '0'), name, ui.OpenId);

            string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
            if(res.Contains("EXIST"))
            {
                int a = 1;
            }
            return Content(res);
         
        }
    }
}