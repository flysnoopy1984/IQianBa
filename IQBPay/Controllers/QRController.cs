using IQBPay.Core;
using IQBPay.Core.BaseEnum;
using IQBPay.DataBase;
using IQBPay.Models.QR;
using IQBPay.Models.Result;
using IQBPay.Models.Store;
using System;
using System.Collections.Generic;
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
                string openId = Convert.ToString(Session["OpenId"]);

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
                        EQRInfo updateObj = db.DBQRInfo.Where(q=>q.ID == qr.ID).FirstOrDefault();
                        if (updateObj != null)
                        {
                           
                            qr.TargetUrl = updateObj.TargetUrl;
                            qr.FilePath = updateObj.FilePath;
                            qr.Channel = QRChannel.PP;
                            qr.Type = QRType.AR;
                            db.Entry(updateObj).CurrentValues.SetValues(qr);

                            db.SaveChanges();
                        }
                        else
                        {
                            return Content("Id "+ qr.ID+" 没有找到，无法更新");
                        }
                    }
                    else
                    {
                        db.IsExistQR(qr.OwnnerOpenId, qr.Name, QRType.AR);
                        qr.OwnnerOpenId = this.GetOpenId(true);
                        qr.Channel = QRChannel.PP;
                        qr.Type = QRType.AR;
                        qr = QRManager.CreateMasterUrlById(qr);
                        db.DBQRInfo.Add(qr);
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
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
                result = db.DBQRInfo.Where(a => a.Channel == QRChannel.PPAuto).FirstOrDefault();
                if (result==null) result = new EQRInfo();
            }
         
            return Json(result);
        }
        public ActionResult SaveARDefault(EQRInfo qr)
        {

            try
            {
                qr.RecordStatus = RecordStatus.Normal;
                using (AliPayContent db = new AliPayContent())
                {
                    EQRInfo updateObj = db.DBQRInfo.Where(q=>q.Channel == QRChannel.PPAuto).FirstOrDefault();
                    if (updateObj != null)
                    {
                        qr.TargetUrl = updateObj.TargetUrl;
                        qr.FilePath = updateObj.FilePath;
                        qr.Channel = QRChannel.PPAuto;
                        qr.Type = QRType.AR;
                        db.Entry(updateObj).CurrentValues.SetValues(qr);

                        db.SaveChanges();
                    }
                    else
                    {
                        qr.Channel = QRChannel.PPAuto;
                        qr.Type = QRType.AR;
                        qr = QRManager.CreateMasterUrlById(qr);

                        db.DBQRInfo.Add(qr);
                        db.SaveChanges();
                    }
                  
                }
            }
            catch (Exception ex)
            {
                return Content("Save Store Error" + ex.Message);
            }
            return Json("OK");
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