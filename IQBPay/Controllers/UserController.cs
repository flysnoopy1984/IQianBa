using IQBCore.Common.Helper;
using IQBPay.Core;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.Common.Constant;

namespace IQBPay.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Info()
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
          
       
            using (AliPayContent db = new AliPayContent())
            {
                try
                {
                    EUserInfo result = db.DBUserInfo.Where(u => u.OpenId == OpenId).FirstOrDefault();

                    if (result == null)
                    {
                        result = new EUserInfo();
                        result.QueryResult = false;
                        return Json(result);
                    }
                   
                    if (result.QRUserDefaultId > 0)
                    {
                        EQRUser qrUser = db.DBQRUser.Where(a => a.ID == result.QRUserDefaultId).FirstOrDefault();
                        if (qrUser != null)
                        {
                            result.Rate = qrUser.Rate;
                            result.QRFilePath = qrUser.FilePath;

                        }
                    }
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

        [HttpPost]
        public ActionResult Query(UserRole role= UserRole.Agent, int pageIndex = 0, int pageSize = IQBConstant.PageSize)
        {
            List<EUserInfo> result = new List<EUserInfo>();
            IQueryable<EUserInfo> list = null;
            try
            {
                string openId = Convert.ToString(Session["OpenId"]);

                using (AliPayContent db = new AliPayContent())
                {
                    list = db.DBUserInfo.Where(u=>u.UserRole == role).OrderByDescending(i => i.CreateDate);
                    
                    int totalCount = list.Count();
                    if (pageIndex == 0)
                    {
                        result = list.Take(pageSize).ToList();

                        if (result.Count > 0)
                            result[0].TotalCount = totalCount;
                    }
                    else
                        result = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                }
            }
            catch (Exception ex)
            {
                Log.log("Store Query Error:" + ex.Message);
                throw ex;
            }
            return Json(result);
        }

        public ActionResult Save(EUserInfo ui)
        {
            try
            {
                using (AliPayContent db = new AliPayContent())
                {

                    ui.InitModify();

                    DbEntityEntry<EUserInfo> entry = db.Entry<EUserInfo>(ui);
                    entry.State = EntityState.Unchanged;


                    entry.Property(t => t.Name).IsModified = true;
                   // entry.Property(t => t.Rate).IsModified = true;
                   
                    entry.Property(t => t.AliPayAccount).IsModified = true;
                    entry.Property(t => t.UserStatus).IsModified = true;

                    entry.Property(t => t.MDate).IsModified = true;
                    entry.Property(t => t.MTime).IsModified = true;
                    entry.Property(t => t.ModifyDate).IsModified = true;

                    db.SaveChanges();


                }
            }
            catch (Exception ex)
            {

                return Content("Save Store Error" + ex.Message);
            }
            return Json("OK");
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