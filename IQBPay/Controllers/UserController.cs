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
using System.Configuration;
using IQBCore.Model;

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
                string OpenId = this.GetUserSession().OpenId;
                return Get(OpenId);
            }
            catch
            {
                return Redirect("/Main/Login?action=" + ExistAction.sessionlost.ToString());
            }
            
        }


        public ActionResult Get(string  OpenId)
        {
            string sql = @"select ui.Id,ui.Name,ui.UserStatus,ui.UserRole,ui.IsAutoTransfer,ui.CDate,ui.MDate,
                            ui.UserRole,ui.Headimgurl,ui.AliPayAccount,ui.QRInviteCode,ui.NeedFollowUp,
                            CONVERT(varchar(100), ui.RegisterDate, 111) as RegisterDate,
                           qruser.MarketRate,qruser.ID as qrUserId,QRUser.Rate,qruser.FilePath as QRFilePath,qruser.ParentCommissionRate,qruser.OrigQRFilePath,
                           qrUser.parentOpenId as ParentAgentOpenId,qrUser.ParentName as ParentAgent,
                           si.ID as StoreId,si.Name as StoreName,si.Rate as StoreRate
                           from userinfo as ui 
                           left join qrUser on qruser.OpenId = ui.OpenId 
                           left join StoreInfo as si on si.ID = qruser.ReceiveStoreId                  
                           where ui.openId = '{0}' and QRUser.IsCurrent ='true'
                        ";

            sql = string.Format(sql, OpenId);
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
                    EQRUser qrHuge = db.DBQRUser.Where(o => o.OpenId == OpenId && o.QRType == QRType.ARHuge).FirstOrDefault();
                    if(qrHuge ==null)
                        result.QRHuge = new EQRUser();
                    else
                    {
                        result.QRHuge = qrHuge;
                    }

                    result.StoreList = db.Database.SqlQuery<HashStore>("select Id,Name,Rate from storeinfo").ToList();
                    result.ParentAgentList = db.Database.SqlQuery<HashUser>("select OpenId,Name from userinfo where userRole = 3 or userRole=100").ToList();

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
        public ActionResult Query(UserRole UserRole = UserRole.All,string AgentName="",string ParentName="",int HasQRHuge= -1,UserStatus UserStatus= UserStatus.PPUser, int pageIndex = 0, int pageSize = IQBConstant.PageSize)
        {
            
            List<RUserInfo> result = new List<RUserInfo>();
            
            string sql = @"select ui.Id,ui.OpenId,ui.Name,ui.IsAutoTransfer,ui.CDate,ui.AliPayAccount,ui.UserStatus,ui.HasQRHuge,
                        qruser.Rate,qruser.ParentCommissionRate,qruser.parentOpenId as ParentAgentOpenId,qruser.ParentName as ParentAgent,QRUser.MarketRate,
                        si.ID as StoreId,si.Name as StoreName,si.Rate as StoreRate
                        from userinfo as ui 
                        left join qrUser on qruser.OpenId = ui.OpenId
                        left join StoreInfo as si on si.ID = qruser.ReceiveStoreId
                        where qrUser.IsCurrent = 'true'";

            if(!string.IsNullOrEmpty(AgentName))
            {
                sql += " and ui.name like '%"+AgentName+"%'";
            }
            if (!string.IsNullOrEmpty(ParentName))
            {
                sql += " and qruser.ParentName like '%" + ParentName + "%'";
            }
            if (HasQRHuge!=99)
            {
                sql += " and ui.HasQRHuge="+ HasQRHuge;
            }

            if (UserStatus !=  UserStatus.All)
            {
                sql += " and ui.UserStatus=" +  (int)UserStatus;
            }

            if (UserRole != UserRole.All)
            {
                sql += " and ui.UserRole=" + (int)UserRole;
            }

            sql +=" ORDER BY ui.CreateDate desc";

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

        [HttpPost]
        public ActionResult ChangeUserStatus()
        {
            string openId = Request["OpenId"];
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EUserInfo ui = db.DBUserInfo.Where(o => o.OpenId == openId).FirstOrDefault();
                    if (ui != null)
                    {
                        if (ui.UserStatus == UserStatus.JustRegister)
                            ui.UserStatus = UserStatus.PPUser;
                        else
                            ui.UserStatus = UserStatus.JustRegister;
                        db.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
       }

        [HttpPost]
        public ActionResult DeleteUserAgent(string openId)
        {
            OutAPIResult result = new OutAPIResult();
            result.IsSuccess = true;
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    var user = db.DBUserInfo.Where(o => o.OpenId == openId).FirstOrDefault();
                   
                    db.DBUserInfo.Remove(user);

                    var qrInfo = db.DBQRInfo.Where(o => o.OwnnerOpenId == openId).FirstOrDefault();
                    db.DBQRInfo.Remove(qrInfo);

                    var qrUserlist = db.DBQRUser.Where(o => o.OpenId == openId).ToList();
                    for(int i=0;i<qrUserlist.Count;i++)
                    {
                        db.DBQRUser.Remove(qrUserlist[i]);
                    }
                    db.SaveChanges();
                }
            }  
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
           

            return Json(result);
            
        }

        public ActionResult SaveUserAgent(InUserAgent InUA)
        {
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    float adjustRate = 0;
                    
                    EUserInfo ui = db.DBUserInfo.Where(o => o.OpenId == InUA.OpenId).FirstOrDefault();
                  
                    ui.IsAutoTransfer = InUA.IsAutoTransfer;
                    ui.AliPayAccount = InUA.AliPayAccount;
                    ui.UserStatus = InUA.UserStatus;
                    ui.UserRole = InUA.UserRole;
                    ui.parentOpenId = InUA.ParentOpenId;
                    ui.NeedFollowUp = InUA.NeedFollowUp;

                    //本人所有QRUser
                    EQRUser qrUser = db.DBQRUser.Where(o => o.OpenId == InUA.OpenId && o.QRType ==  QRType.AR).FirstOrDefault();

                    float Ratediff = InUA.MarketRate - InUA.Rate;

                    if (adjustRate == 0 && qrUser.IsCurrent)
                    {
                        adjustRate = InUA.Rate - qrUser.Rate;
                    }


                    qrUser.ParentName = InUA.ParentName;
                    qrUser.ParentOpenId = InUA.ParentOpenId;
                    qrUser.ParentCommissionRate = InUA.ParentCommissionRate;
                    qrUser.ReceiveStoreId = InUA.StoreId;
                    //qrUser.MarketRate = InUA.MarketRate;
                    qrUser.Rate = qrUser.MarketRate - Ratediff;

                    //本人邀请码QRInfo
                    EQRInfo qrInfo = db.DBQRInfo.Where(a => a.ID == ui.QRInviteCode).FirstOrDefault();
                    qrInfo.Rate = InUA.QRInfo_Rate;
                    qrInfo.ParentCommissionRate = InUA.QRInfo_ParentCommissionRate;
                    qrInfo.NeedFollowUp = InUA.NeedFollowUp;
                    qrInfo.MaxInviteCount = InUA.QRInfo_MaxInviteCount;

                    //Child
                    List<EUserInfo> cList = db.DBUserInfo.Where(o => o.parentOpenId == ui.OpenId && o.UserRole == UserRole.DiamondAgent).ToList();

                    for(int i=0;i< cList.Count;i++)
                    {
                        cList[i].NeedFollowUp = InUA.NeedFollowUp;
                    }



                    //if(adjustRate!=0)
                    //{
                    //    
                    //    EQRInfo qrInfo = db.DBQRInfo.Where(a => a.ID == ui.QRInviteCode).FirstOrDefault();
                    //    if (qrInfo == null)
                    //    {
                    //        throw new Exception("没有找到对应的邀请码");
                    //    }
                    //      qrInfo.Rate += adjustRate;
                    //   // qrInfo.ParentCommissionRate += adjustRate;
                    //    //下级联动
                    //    List<EQRUser> plist = db.DBQRUser.Where(o => o.ParentOpenId == InUA.OpenId).ToList();
                    //    for (int i = 0; i < plist.Count; i++)
                    //    {
                    //        EQRUser qrUser = plist[i];
                    //        qrUser.Rate += adjustRate;
                    //        //qrUser.ParentCommissionRate += adjustRate;
                    //    }
                    //}


                    db.SaveChanges();

                    //WX客户端
                    string url = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
                    url += "API/OutData/RefreshGlobelConfig";
                    HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, "", "application/x-www-form-urlencoded");
                

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
        [HttpPost]
        public ActionResult CreateOrUpdateQRHuge(string openId,float Rate,float marketRate)
        {
            OutAPIResult result = new OutAPIResult();
            try

            {
                using (AliPayContent db = new AliPayContent())
                {
                    EQRUser sQRUser = db.DBQRUser.Where(o => o.OpenId == openId && o.QRType == QRType.AR).First();
                    EQRUser bQRUser = db.DBQRUser.Where(o => o.OpenId == openId && o.QRType == QRType.ARHuge).FirstOrDefault();
                    if (bQRUser == null)
                    {
                        //大额码参数
                        bQRUser = EQRUser.CopyToQRUserForHuge(sQRUser);
                        bQRUser.Rate = Rate;
                        bQRUser.MarketRate = marketRate;
                        db.DBQRUser.Add(bQRUser);

                        //用户大额标记修改
                        EUserInfo ui = db.DBUserInfo.Where(u => u.OpenId == openId).First();
                        ui.HasQRHuge = true;


                        result.SuccessMsg = "开通权限";
                    }
                    else
                    {
                        bQRUser.Rate = Rate;
                        bQRUser.MarketRate = marketRate;
                       

                        result.SuccessMsg = "修改成功";

                        
                    }
                    db.SaveChanges();
                   


                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }


        public ActionResult Demo()
        {


            
           // string url = "http://ap.iqianba.cn/api/userapi/register/";
            string url = "http://localhost:24068/api/userapi/register/";
            string data = @"UserStatus=1&UserRole=1&Isadmin=false&name=平台服务客服&openId=o3nwE0m12mke3-VhWic-UAX7Oh_0&QRAuthId=66&Headimgurl=http://wx.qlogo.cn/mmopen/6bFpEa5VMGgc8Aoj6Ro2sTK4icFibQrEpLBQCiaGey6gWzBJnZZ44ic9JEUC2zxxRbx9bKC1COuXlwjZ8tMsC1wZtSHrW19qicl0N/0";
         
            string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
            if (res == "EXIST")
            {
                if (res.StartsWith("EXIST"))
                {
                    int a = 1;
                }
            }
           

            return Content(res);
           /*
            return RedirectToAction("WebEntry", "Main", new { openId = "aaa" });
            return View();
         */
        }
        [HttpPost]
        public ActionResult UserNoAccount()
        {
            string DateBefore = Request["DateBefore"];
            List<RUserInfo> result = new List<RUserInfo>();
            try
            {

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Json(result);
        }
        public ActionResult UserAdjustment()
        {
            return View();
        }

        #region 账户余额
       
        public ActionResult ReChargeUserAccountBalance()
        {
            UserSession us = this.GetUserSession();
            ViewBag.CurrentUserId = us.Id;
            ViewBag.IsAdmin = us.UserRole == UserRole.Administrator ? true : false;
            return View();

        }

        /// <summary>
        /// 获取所有出货商账户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetShipmentAccount()
        {
            NResult<RUserAccountBalance> result = new NResult<RUserAccountBalance>(); 
            try
            {
                UserSession us = GetUserSession();
                //没有登陆
                if(us.Id ==0 )
                {
                    result.IsSuccess = false;
                    result.IntMsg = -1;
                    return Json(result);
                }
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select b.Id,b.UserId,ui.OpenId,b.AliPayAccount,b.UserAccountType,ui.Name as UserName,
                    ROUND(b.O2OShipBalance, 2) as O2OShipBalance,o.O2OOnOrderAmount from UserAccountBalance as b
                    join  UserInfo as ui on b.UserId = ui.Id
                    left join 
                    (
                    select sum(o.OrderAmount) as O2OOnOrderAmount,o.WHUserId 
                    from O2OOrder as o 
                    where o.O2OOrderStatus <18
                    group by o.WHUserId
                    ) as o on o.WHUserId = ui.Id
                    where ui.O2OUserRole = {0}";
                    if(us.UserRole != UserRole.Administrator)
                    {
                        sql += " and b.UserId =" + us.Id;
                    }
                    sql = string.Format(sql, Convert.ToInt32(O2OUserRole.Shippment));

                    List<RUserAccountBalance> list = db.Database.SqlQuery<RUserAccountBalance>(sql).ToList();


                    result.resultList = list;
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
           
            return Json(result);
        }
        #endregion
    }
}