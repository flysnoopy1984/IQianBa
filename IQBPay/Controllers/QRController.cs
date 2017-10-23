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
        public ActionResult ARList()
        {
            Session["OpenId"] = this.GetOpenId(true);
            return View();
        }

        [HttpPost]
        public ActionResult Query(QRType QRType, int pageIndex = 0, int pageSize = IQBConstant.PageSize)
        {
            List<RQRInfo> result = new List<RQRInfo>();
            try
            {
                string openId = this.GetOpenId(true);

                using (AliPayContent db = new AliPayContent())
                {
                    var list =
                    (
                        from qr in db.DBQRInfo
                        join st in db.DBStoreInfo on qr.ReceiveStoreId equals st.ID into joinedQRST 
                        from st in joinedQRST.DefaultIfEmpty()
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

                        }
                    ); 
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
        public ActionResult ARInfo()
        {
            return View();
        }

        public ActionResult Get(long Id)
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
                    result.HashStoreList = db.Database.SqlQuery<HashStore>("select Id,Name from storeinfo").ToList();
                    result.HashUserList = db.Database.SqlQuery<HashUser>("select OpenId,Name from userinfo").ToList();
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
                qr.OwnnerOpenId = this.GetOpenId(true);
                qr.RunResult = "OK";
                using (AliPayContent db = new AliPayContent())
                {
                    if(qr.ID>0)
                    {
                        qr.InitModify();

                        DbEntityEntry<EQRInfo> entry = db.Entry<EQRInfo>(qr);
                        entry.State = EntityState.Unchanged;

                        entry.Property(t => t.Name).IsModified = true;
                        entry.Property(t => t.Rate).IsModified = true;
                        entry.Property(t => t.ReceiveStoreId).IsModified = true;
                        entry.Property(t => t.Remark).IsModified = true;
                        entry.Property(t => t.ParentOpenId).IsModified = true;
                        entry.Property(t => t.ParentCommissionRate).IsModified = true;
                        entry.Property(t => t.RecordStatus).IsModified = true;

                        entry.Property(t => t.MDate).IsModified = true;
                        entry.Property(t => t.MTime).IsModified = true;
                        entry.Property(t => t.ModifyDate).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {

                        qr.OwnnerOpenId = this.GetUserSession().OpenId;
                        qr.Channel = Channel.PP;
                        qr.Type = QRType.ARAuth;
                       
                     
                        db.DBQRInfo.Add(qr);
                        db.SaveChanges();      
                                         
                        qr = QRManager.CreateMasterUrlById(qr);
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
        public ActionResult AuthList()
        {
            Session["OpenId"] = this.GetOpenId(true);



            return View();
        }

        #endregion

        #region AuthInfo
        public ActionResult AuthInfo()
        {
            Session["OpenId"] = this.GetOpenId(true);



            return View();
        }

        public ActionResult SaveAuth(EQRInfo qr)
        {

            try
            {
                qr.OwnnerOpenId = this.GetOpenId(true);
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

                        entry.Property(t => t.MDate).IsModified = true;
                        entry.Property(t => t.MTime).IsModified = true;
                        entry.Property(t => t.ModifyDate).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {

                        qr.OwnnerOpenId = this.GetOpenId(true);
                     
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