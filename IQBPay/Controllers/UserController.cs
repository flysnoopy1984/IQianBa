using IQBCore.Common.Helper;
using IQBPay.Core;
using IQBPay.DataBase;
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
            return View();
        }

        public ActionResult Get()
        {
            string OpenId = this.GetOpenId(true);
            RUserInfo result = null;
            using (AliPayContent db = new AliPayContent())
            {
                EUserInfo ui =db.DBUserInfo.Where(u => u.OpenId == OpenId).FirstOrDefault();
                result = ui as RUserInfo;
                return Json(ui);
            }
           
        }

       
        public ActionResult Demo()
        {
            EUserInfo ui = new EUserInfo();
            ui.Headimgurl = "http://ssdakdla";
            string url = "http://localhost:24068/api/userapi/regiseter";
            string data = "UserStatus=1&UserRole=1&Isadmin=false&name={0}&openId={1}&Headimgurl=aaaaaaaaaaa";
            string name = "22";
           
            data = string.Format(data, "wx" + ui.Id.ToString().PadLeft(7, '0'), name, ui.OpenId);

            string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
            return Content(res);
          
        }
    }
}