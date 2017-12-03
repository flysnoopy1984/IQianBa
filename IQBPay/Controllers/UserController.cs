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
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.InParameter;

namespace IQBPay.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [IQBPayAuthorize_Admin]
        public ActionResult List()
        {
            return View();
        }

        [IQBPayAuthorize]
        public ActionResult Info()
        {
            return View();
        }

        [IQBPayAuthorize]
        public new ActionResult Profile()
        {

            return View();
        }
        public ActionResult GetProfile()
        {
            try
            {
                int Id = this.GetUserSession().Id;
                return Get(Id);
            }
            catch
            {
                return Redirect("/Main/Login?action=" + ExistAction.sessionlost.ToString());
            }
            
        }


        public ActionResult Get(int Id)
        {
            string sql = @"select ui.Id,ui.Name,ui.UserStatus,ui.UserRole,ui.IsAutoTransfer,ui.CDate,ui.MDate,ui.UserRole,ui.Headimgurl,ui.AliPayAccount,
                           qruser.MarketRate,qruser.ID as qrUserId,QRUser.Rate,qruser.FilePath as QRFilePath,qruser.ParentCommissionRate,
                           qrUser.parentOpenId as ParentAgentOpenId,qrUser.ParentName as ParentAgent,
                           si.ID as StoreId,si.Name as StoreName,si.Rate as StoreRate
                           from userinfo as ui 
                           left join qrUser on qruser.ID = ui.QRUserDefaultId  
                           left join StoreInfo as si on si.ID = qruser.ReceiveStoreId                  
                           where ui.Id = {0}
                        ";

            sql = string.Format(sql, Id);
           // base.Log.log(sql);

            using (AliPayContent db = new AliPayContent())
            {
                try
                {
                    RUserInfo result = db.Database.SqlQuery<RUserInfo>(sql).FirstOrDefault();
                    

                    if (result == null)
                    {
                        result = new RUserInfo();
                        result.QueryResult = false;
                        return Json(result);
                    }
                    result.StoreList = db.Database.SqlQuery<HashStore>("select Id,Name,Rate from storeinfo where storeinfo.IsReceiveAccount = 'false'").ToList();
                    result.ParentAgentList = db.Database.SqlQuery<HashUser>("select OpenId,Name from userinfo").ToList();

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
            List<RUserInfo> result = new List<RUserInfo>();

            string sql = @"select ui.Id,ui.Name,ui.IsAutoTransfer,ui.CDate,ui.AliPayAccount,ui.UserStatus,qruser.Rate,qruser.ParentCommissionRate,
	                    qruser.parentOpenId as ParentAgentOpenId,qruser.ParentName as ParentAgent,
	                    si.ID as StoreId,si.Name as StoreName,si.Rate as StoreRate
                        from userinfo as ui 
                        left join qrUser on qruser.ID = ui.QRUserDefaultId
		                left join StoreInfo as si on si.ID = qruser.ReceiveStoreId
                        ORDER BY ui.CreateDate desc";

           // IQueryable<EUserInfo> list = null;
            try
            {

                using (AliPayContent db = new AliPayContent())
                {

                    var list = db.Database.SqlQuery<RUserInfo>(sql);
                 
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

      
        public ActionResult QueryKeyValue()
        {
            List<HashUser> result = null;

            using (AliPayContent db = new AliPayContent())
            {
                result = db.Database.SqlQuery<HashUser>("select OpenId,Name from userinfo").ToList();
                if (result == null)
                    result = new List<HashUser>();
            }

            return Json(result);
        }

        public ActionResult SaveUserAgent(InUserAgent InUA)
        {
            try
            {
                using (AliPayContent db = new AliPayContent())
                {

                    EUserInfo ui = new EUserInfo();
                    ui.Id = InUA.ID;
                    ui.IsAutoTransfer = InUA.IsAutoTransfer;
                    ui.AliPayAccount = InUA.AliPayAccount;
                    ui.UserStatus = InUA.UserStatus;
                    ui.UserRole = InUA.UserRole;

                    DbEntityEntry<EUserInfo> entry = db.Entry<EUserInfo>(ui);
                    entry.State = EntityState.Unchanged;
                    entry.Property(t => t.AliPayAccount).IsModified = true;
                    entry.Property(t => t.IsAutoTransfer).IsModified = true;
                    entry.Property(t => t.UserStatus).IsModified = true;
                    entry.Property(t => t.UserRole).IsModified = true;

                    EQRUser qrUser = new EQRUser();
                    qrUser.ID = InUA.QrUserId;
                    qrUser.ParentName = InUA.ParentName;
                    qrUser.ParentOpenId  = InUA.ParentOpenId;
                    qrUser.ParentCommissionRate  = InUA.ParentCommissionRate;
                    qrUser.ReceiveStoreId = InUA.StoreId;
                    qrUser.MarketRate = InUA.MarketRate;
                    qrUser.Rate = InUA.Rate;

                    DbEntityEntry<EQRUser> qrEntry = db.Entry<EQRUser>(qrUser);
                    qrEntry.State = EntityState.Unchanged;
                    qrEntry.Property(t => t.ParentName).IsModified = true;
                    qrEntry.Property(t => t.ParentOpenId).IsModified = true;
                    qrEntry.Property(t => t.ParentCommissionRate).IsModified = true;
                    qrEntry.Property(t => t.ReceiveStoreId).IsModified = true;
                    qrEntry.Property(t => t.MarketRate).IsModified = true;
                    qrEntry.Property(t => t.Rate).IsModified = true;


                    db.SaveChanges();


                }
            }
            catch (Exception ex)
            {

                return Content("Save Store Error" + ex.Message);
            }
            return Json("OK");
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
                    entry.Property(t => t.IsAutoTransfer).IsModified = true;
                   
                    entry.Property(t => t.AliPayAccount).IsModified = true;
                    entry.Property(t => t.UserStatus).IsModified = true;
                    entry.Property(t => t.UserRole).IsModified = true;
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


            /*
           // string url = "http://ap.iqianba.cn/api/userapi/register/";
            string url = "http://localhost:24068/api/userapi/register/";
            string data = @"UserStatus=1&UserRole=1&Isadmin=false&name=平台服务客服&openId=o3nwE0jrONff65oS-_W96ErKcaa0&QRAuthId=10&Headimgurl=http://wx.qlogo.cn/mmopen/6bFpEa5VMGgc8Aoj6Ro2sTK4icFibQrEpLBQCiaGey6gWzBJnZZ44ic9JEUC2zxxRbx9bKC1COuXlwjZ8tMsC1wZtSHrW19qicl0N/0";
            //""
            string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
            if (res == "EXIST")
            {
                if (res.StartsWith("EXIST"))
                {
                    int a = 1;
                }
            }
           

            return Content(res);
            */
            return RedirectToAction("WebEntry", "Main", new { openId = "aaa" });
            return View();
         
        }
    }
}