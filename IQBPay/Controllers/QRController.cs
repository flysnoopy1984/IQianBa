using IQBPay.Core;
using IQBCore.IQBPay.BaseEnum;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Common.Constant;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBWX.Models.WX.Template.InviteCode;
using IQBCore.IQBPay.Models.User;
using IQBCore.IQBPay.Models.O2O;
using System.Data.Entity.Migrations;

namespace IQBPay.Controllers
{
    public class QRController : BaseController
    {
        // GET: QR
        public ActionResult Index()
        {
            return View();
        }

        #region ARList
        [IQBPayAuthorize_Admin]
        public ActionResult ARList()
        {
          
            return View();
        }

        [HttpPost]
        public ActionResult Query(QRType QRType, string Name = "", UserRole UserRole = UserRole.All,int pageIndex = 0, int pageSize = IQBConstant.PageSize)
        {
            List<RQRInfo> result = new List<RQRInfo>();
         
            try
            {
               //string openId = this.GetOpenId(true);
                
                using (AliPayContent db = new AliPayContent())
                {
                    var list =
                    (
                        from qr in db.DBQRInfo
                        join st in db.DBStoreInfo on qr.ReceiveStoreId equals st.ID into joinedQRST 
                        from st in joinedQRST.DefaultIfEmpty()
                        join ui in db.DBUserInfo on qr.OwnnerOpenId equals ui.OpenId into joinedUI
                        from ui in joinedUI.DefaultIfEmpty()
                        join pi in db.DBUserInfo.DefaultIfEmpty() on qr.ParentOpenId  equals pi.OpenId into joinQRUser
                        from pi in joinQRUser.DefaultIfEmpty()
                        where qr.Type== QRType
                        select new RQRInfo
                        {
                            ID = qr.ID,
                            FilePath = qr.FilePath,
                            Name = qr.Name,
                            Rate = qr.Rate,
                            ParentName =pi.Name,
                            ParentCommissionRate = qr.ParentCommissionRate,
                            StoreName = st.Name,
                            Remark = qr.Remark,
                            RecordStatus = qr.RecordStatus,
                            APPId = qr.APPId,
                            CreateDate = qr.CreateDate,
                            UserRole = ui.UserRole,
                            MaxInviteCount = qr.MaxInviteCount,
                            OwnnerOpenId = qr.OwnnerOpenId,
                            NeedFollowUp = qr.NeedFollowUp,
                            

                        }
                    );
                    if(!string.IsNullOrEmpty(Name))
                    {
                        list = list.Where(a => a.Name.Contains(Name));
                    }
                    if(UserRole!= UserRole.All)
                    {
                        
                        list = list.Where(a => a.UserRole == UserRole);
                    }
                    list =list.OrderByDescending(i => i.CreateDate);
                  
                    //  db.DBQRInfo.Where(i => i.Type == QRType).OrderByDescending(i => i.CreateDate);
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

                using (AliPayContent db = new AliPayContent())
                {
                    if (UserRole == UserRole.DiamondAgent)
                    {
                        foreach (RQRInfo r in result)
                        {
                            string sql = string.Format("select count(*) from UserInfo where parentOpenId = '{0}' and UserStatus=1", r.OwnnerOpenId);
                            r.CurrentInvitedNum = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.log("QR Query Error:" + ex.Message);
                return Content(ex.Message);
            }
            return Json(result);
        }

        #endregion

        #region ARInfo
        [IQBPayAuthorize_Admin]
        public ActionResult ARInfo()
        {
            return View();
        }

        public ActionResult GetByType(QRType qrType,string openId)
        {

            try
            {
                EQRUser result = null;
                using (AliPayContent db = new AliPayContent())
                {
                  
                    result = db.DBQRUser.Where(a => a.QRType == qrType && a.OpenId == openId).FirstOrDefault();
                    if (result == null)
                        result = new EQRUser();
                           
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult O2OCreateOrUpdate(string openId, float Rate, float marketRate)
        {
            OutAPIResult result = new OutAPIResult();
            try

            {
                using (AliPayContent db = new AliPayContent())
                {
                    EQRUser sQRUser = db.DBQRUser.Where(o => o.OpenId == openId && o.QRType == QRType.AR).First();
                    EQRUser bQRUser = db.DBQRUser.Where(o => o.OpenId == openId && o.QRType == QRType.O2O).FirstOrDefault();
                    EUserInfo ui = db.DBUserInfo.Where(u => u.OpenId == openId).First();
                    if (bQRUser == null)
                    {
                        //O2O参数
                        bQRUser = EQRUser.CopyToQRUserForO2O(sQRUser);
                        bQRUser.Rate = Rate;
                        bQRUser.MarketRate = marketRate;
                        db.DBQRUser.AddOrUpdate(bQRUser);

                        //ui.HasQRO2O = true;
                        ui.O2OUserRole = O2OUserRole.Agent;
                        db.SaveChanges();

                        bQRUser = QRManager.CreateO2OEntryQR(bQRUser);
                        db.Entry(bQRUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();


                        result.SuccessMsg = "开通权限";
                    }
                    else
                    {
                        bQRUser.Rate = Rate;
                        bQRUser.MarketRate = marketRate;

                        result.SuccessMsg = "修改成功";
                       
                    }
                    //O2O代理费率配置表
                    List<EO2OMall> mallList = db.DBO2OMall.ToList();
                    foreach (EO2OMall mall in mallList)
                    {
                        EO2OAgentFeeRate rate = new EO2OAgentFeeRate
                        {
                            MallId = mall.Id,
                            MarketRate = 8,
                            DiffFeeRate = 0,
                            OpenId = bQRUser.OpenId,
                            //QrUserId = bQRUser.ID,
                            // UserId = ui.Id
                        };
                        EO2OAgentFeeRate dbRate =  db.DBO2OAgentFeeRate.Where(a => a.OpenId == rate.OpenId && a.MallId == rate.MallId).FirstOrDefault();
                        if (dbRate == null)
                            db.DBO2OAgentFeeRate.Add(rate);
                        dbRate = rate;
                            
                    }
                    db.SaveChanges();




                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }

        public ActionResult Get(long Id,QRType qrType)
        {
            
            try
            {
                EQRInfo result = null;
                using (AliPayContent db = new AliPayContent())
                {
                    if (Id == -1)
                    {
                        result = new EQRInfo();
                        result.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Normal;
                        
                    }
                    else
                    {
                        result = db.DBQRInfo.Where(a => a.ID == Id).FirstOrDefault();
                    }
                    if (qrType == QRType.ARAuth)
                    {
                        string sql = string.Format("select count(*) from UserInfo where parentOpenId = '{0}' and UserStatus = 1", result.OwnnerOpenId);
                        result.CurrentInvitedNum = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                        result.HashStoreList = db.Database.SqlQuery<HashStore>("select Id,Name from storeinfo").ToList();
                        result.HashUserList = db.Database.SqlQuery<HashUser>("select OpenId,Name from userinfo where UserRole = 3 or userRole=100").ToList();
                    }
                }
                return Json(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 给Master用
        /// </summary>
        /// <param name="qr"></param>
        /// <returns></returns>
        public ActionResult SaveAR(EQRInfo qr)
        {
           
            try
            {
                int origNum, curNum;
                qr.OwnnerOpenId = base.GetUserSession().OpenId;
                qr.RunResult = "OK";
                using (AliPayContent db = new AliPayContent())
                {
                    if(qr.ID>0)
                    {
                        EQRInfo updateQR = db.DBQRInfo.Where(o => o.ID == qr.ID).FirstOrDefault();
                        origNum = updateQR.MaxInviteCount;
                        curNum = qr.MaxInviteCount;
                        updateQR.Name = qr.Name;
                        updateQR.Rate = qr.Rate;
                        updateQR.ReceiveStoreId = qr.ReceiveStoreId;
                        updateQR.Remark = qr.Remark;
                        updateQR.ParentOpenId = qr.ParentOpenId;
                        updateQR.ParentCommissionRate = qr.ParentCommissionRate;
                        updateQR.RecordStatus = qr.RecordStatus;
                        updateQR.NeedVerification = qr.NeedVerification;
                        updateQR.MaxInviteCount = qr.MaxInviteCount;
                        updateQR.NeedFollowUp = qr.NeedFollowUp;
                      
                        updateQR.InitModify();

                        //DbEntityEntry<EQRInfo> entry = db.Entry<EQRInfo>(qr);
                        //entry.State = EntityState.Unchanged;

                        //entry.Property(t => t.Name).IsModified = true;
                        //entry.Property(t => t.Rate).IsModified = true;
                        //entry.Property(t => t.ReceiveStoreId).IsModified = true;
                        //entry.Property(t => t.Remark).IsModified = true;
                        //entry.Property(t => t.ParentOpenId).IsModified = true;
                        //entry.Property(t => t.ParentCommissionRate).IsModified = true;
                        //entry.Property(t => t.RecordStatus).IsModified = true;
                        //entry.Property(t => t.NeedVerification).IsModified = true;
                        //entry.Property(t => t.MaxInviteCount).IsModified = true;
                        //entry.Property(t => t.MDate).IsModified = true;
                        //entry.Property(t => t.MTime).IsModified = true;
                        //entry.Property(t => t.ModifyDate).IsModified = true;
                        //entry.Property(t => t.NeedFollowUp).IsModified = true;
                        db.SaveChanges();

                        if(origNum != 0 && curNum>origNum)
                        {
                            try
                            {
                                string accessToken = this.getAccessToken(true);
                                PPInviteCodeNT notice = new PPInviteCodeNT(accessToken, origNum, curNum, updateQR.OwnnerOpenId);
                                notice.Push();
                            }
                            catch(Exception ex)
                            {
                                Log.log(ex.Message);
                            }
                        }
                    }
                    else
                    {

                        qr.OwnnerOpenId = this.GetUserSession().OpenId;
                        qr.Channel = Channel.PP;
                        qr.Type = QRType.ARAuth;
                       
                        db.DBQRInfo.Add(qr);
                        db.SaveChanges();      
                                         
                        qr = QRManager.CreateMasterUrlById(qr, System.Web.HttpContext.Current);
                        db.Entry(qr).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                    }

                }
            }
            catch (Exception ex)
            {
                qr.RunResult = "Save QR Error:"+ex.Message;
                Log.log("QR Save Error:" + ex.Message);
                return Content("Save Store Error" + ex.Message);
            }
            return Json(qr);
        }

        #endregion

        #region ARDefaultInfo
        [IQBPayAuthorize_Admin]
        public ActionResult ARDefaultInfo()
        {
            return View();
        }

        public ActionResult GetDefault()
        {
            EQRInfo result = null;

            using (AliPayContent db = new AliPayContent())
            {
                result = db.DBQRInfo.Where(a => a.Channel == Channel.PPAuto).FirstOrDefault();
                if (result==null) result = new EQRInfo();

                result.HashStoreList = db.Database.SqlQuery<HashStore>("select Id,Name from storeinfo").ToList();
            }
         
            return Json(result);
        }
        public ActionResult SaveARDefault(EQRInfo qr)
        {

            try
            {
               
                using (AliPayContent db = new AliPayContent())
                {
                    qr.InitModify();
                    DbEntityEntry<EQRInfo> entry = db.Entry<EQRInfo>(qr);
                    entry.State = EntityState.Unchanged;
                  
                    entry.Property(t => t.Name).IsModified = true;
                    entry.Property(t => t.Rate).IsModified = true;
                    entry.Property(t => t.ReceiveStoreId).IsModified = true;
                    entry.Property(t => t.Remark).IsModified = true;

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

        #endregion

        #region AuthList
        [IQBPayAuthorize_Admin]
        public ActionResult AuthList()
        {
         


            return View();
        }

        #endregion

        #region AuthInfo
        [IQBPayAuthorize_Admin]
        public ActionResult AuthInfo()
        {
          

            return View();
        }

        public ActionResult SaveAuth(EQRInfo qr)
        {

            try
            {
                qr.OwnnerOpenId = this.GetUserSession().OpenId;
                qr.RunResult = "OK";
                using (AliPayContent db = new AliPayContent())
                {
                    if (qr.ID > 0)
                    {
                        qr.InitModify();

                        DbEntityEntry<EQRInfo> entry = db.Entry<EQRInfo>(qr);
                        entry.State = EntityState.Unchanged;

                        entry.Property(t => t.Name).IsModified = true;
                        entry.Property(t => t.Rate).IsModified = true;
                        entry.Property(t => t.Remark).IsModified = true;
                        entry.Property(t => t.RecordStatus).IsModified = true;
                        entry.Property(t => t.Channel).IsModified = true;
                        entry.Property(t => t.APPId).IsModified = true;
                        entry.Property(t => t.MDate).IsModified = true;
                        entry.Property(t => t.MTime).IsModified = true;
                        entry.Property(t => t.ModifyDate).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {

                        qr.OwnnerOpenId = this.GetUserSession().OpenId;
                     
                        qr.Type = QRType.StoreAuth;
                        db.DBQRInfo.Add(qr);
                        db.SaveChanges();

                        qr = QRManager.CreateStoreAuthUrlById(qr);
                        db.Entry(qr).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                    }

                }
            }
            catch (Exception ex)
            {
                qr.RunResult = "Save Store Error:" + ex.Message;
                
            }
            return Json(qr);
        }
        #endregion

    }


}