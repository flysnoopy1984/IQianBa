using IQBCore.Common.Helper;
using IQBPay.Core;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.User;
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
            UserHeaderImg = "http://wx.qlogo.cn/mmopen/v6XbW38nFORmMzMtm1VjI3WYE7onmicI6UheCgyKJZwPFWTRXSTZqVROYkdllKNGzF82uicVp1ZLPGM9dGKe0KbgE0NVPicWWg7/0";
            return View();
        }

        public ActionResult Get()
        {
           
           
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
                    EQRUser qrUser = db.DBQRUser.Where(a => a.ID == ui.QRDefaultId).FirstOrDefault();
                  
                    result = new RUserInfo();
                    result.InitFromChild(ui);
                    result.Rate = qrUser.Rate;
                    result.QRFilePath = qrUser.FilePath;
                    result.QueryResult = true;
                    return Json(result);
                }
                catch (Exception ex)
                {
                    Log.log("User Get Error:" + ex.Message);
                    throw ex;
                }
           
            }
          
          
           
        }

       
        public ActionResult Demo()
        {
           
            EUserInfo ui = new EUserInfo();
            ui.Headimgurl = "http://ssdakdla";
            ui.QRAuthId = 13;

           // string url = "http://ap.iqianba.cn/api/userapi/register/";
            string url = "http://localhost:24068/api/userapi/register/";
            string data = @"UserStatus=1&UserRole=1&Isadmin=false&name=Jacky&openId=orKUAw16WK0BmflDLiBYsR-Kh5bE&Headimgurl=http://wx.qlogo.cn/mmopen/v6XbW38nFORmMzMtm1VjI3WYE7onmicI6UheCgyKJZwPFWTRXSTZqVROYkdllKNGzF82uicVp1ZLPGM9dGKe0KbgE0NVPicWWg7/0";
            string name = "22";
           
            data = string.Format(data, name, "orKUAw16WK0BmflDLiBYsR-Kh5bE");
          
            string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
            if(res.Contains("EXIST"))
            {
                int a = 1;
            }
            return Content(res);
         
        }
    }
}