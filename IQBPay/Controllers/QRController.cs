using IQBPay.Core;
using IQBPay.Core.BaseEnum;
using IQBPay.DataBase;
using IQBPay.Models.QR;
using IQBPay.Models.Result;
using IQBPay.Models.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Query(QRType QRType, int pageIndex = 0, int pageSize = IQBConfig.PageSize)
        {
            List<EQRInfo> result = new List<EQRInfo>();
            try
            {
                string openId = this.GetOpenId(true);

                using (AliPayContent db = new AliPayContent())
                {
                    var list = db.DBQRInfo.Where(i => i.OwnnerOpenId == openId && i.Type == QRType).OrderByDescending(i => i.CreateDate);
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
            EQRInfo result = null;

            if (Id == -1)
            {
                result = new EQRInfo();
                
                result.RecordStatus = Core.BaseEnum.RecordStatus.Normal;
            }
            else
            {
                using (AliPayContent db = new AliPayContent())
                {
                    result = db.DBQRInfo.Where(a=> a.ID == Id).FirstOrDefault();
                }
            }
            return Json(result);
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
                using (AliPayContent db = new AliPayContent())
                {
                    if(qr.ID>0)
                    {
                        qr.InitModify();

                        DbEntityEntry<EQRInfo> entry = db.Entry<EQRInfo>(qr);
                        entry.State = EntityState.Unchanged;

                        entry.Property(t => t.Name).IsModified = true;
                        entry.Property(t => t.Rate).IsModified = true;
                        entry.Property(t => t.Remark).IsModified = true;
                        entry.Property(t => t.RecordStatus).IsModified = true;

                        entry.Property(t => t.MDate).IsModified = true;
                        entry.Property(t => t.MTime).IsModified = true;
                        entry.Property(t => t.ModifyDate).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        
                        qr.OwnnerOpenId = this.GetOpenId(true);
                        qr.Channel = Channel.PP;
                        qr.Type = QRType.AR;
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
                Log.log("QR Save Error:" + ex.Message);
                return Content("Save Store Error" + ex.Message);
            }
            return Json("OK");
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
                qr.RunResult = "Save Store Error" + ex.Message;
                
            }
            return Json(qr);
        }
        #endregion

    }

    #region
    /*
                              using (AliPayContent db = new AliPayContent())
                              {
                                  IQueryable<RQRInfo> list = db.DBQRInfo.Where(i => i.OwnnerOpenId == openId).OrderByDescending(i => i.CreateDate)
                                      .Join(db.DBStoreInfo, a => a.StoreId, b => b.ID,
                                      (a, b) => new RQRInfo()
                                      {
                                          StoreId = b.ID,
                                          StoreName = b.Name,
                                          OwnnerOpenId = a.OwnnerOpenId,
                                          ID = a.ID,
                                          CDate=a.CDate,
                                          CreateDate =a.CreateDate,
                                          MDate=a.MDate,
                                          ModifyDate =a.ModifyDate,
                                          CreateUser=a.CreateUser,
                                          CTime = a.CTime,
                                          FilePath= a.FilePath,
                                          ModifyUser =a.ModifyUser,
                                          MTime =a.MTime,
                                          Name=a.Name,
                                          Rate =a.Rate,
                                          RecordStatus = a.RecordStatus,
                                          Remark = a.Remark,
                                          TotalCount = a.TotalCount,


                                      });
              */
    #endregion
}