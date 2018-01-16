﻿
using Com.Alipay.Model;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.Sys;
using IQBCore.IQBPay.Models.Tool;
using IQBCore.IQBPay.Models.User;
using IQBCore.IQBWX.Models.OutParameter;
using IQBPay.DataBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class TestController : BaseController
    {
        
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (IsTestMode)
            {
                return Redirect("/Main/WebEntry?openId=o3nwE0qI_cOkirmh_qbGGG-5G6B0");
            }

            return RedirectToAction("Login", "Main");
        }


        public ActionResult Tool_RPay()
        {


            return View();
        }

        [HttpPost]
        public ActionResult GetRPayQR()
        {
            List<ETool_QR> result = null;

            using (AliPayContent db = new AliPayContent())
            {
                result = db.DBTool_QR.ToList();
                if (result == null)
                    result = new List<ETool_QR>();
            }

            return Json(result);

        }

        [HttpPost]
        public ActionResult CreateRPayQR(string Amount,string couponAmt,string inputAmt)
        {
            AliPayManager payMag = new AliPayManager();
            string sellerId;
            string Res = "";
            ETool_QR qr = new ETool_QR();
            qr.CouponAmt = Convert.ToSingle(couponAmt);
            qr.InputAmt = Convert.ToSingle(inputAmt);
            qr.OrderAmt = Convert.ToSingle(Amount);

            using (AliPayContent db = new AliPayContent())
            {
                sellerId = db.DBStoreInfo.Where(s => s.IsReceiveAccount == true).FirstOrDefault().AliPayAccount;

                ResultEnum status;
                Res = payMag.PayF2F_ForR(BaseController.App, sellerId, Amount, qr,out status);
                if (status == ResultEnum.SUCCESS)
                {
                   
                  
                    qr.FilePath = Res;
                    db.DBTool_QR.Add(qr);
                    db.SaveChanges();
                }
            }
            return Json(qr);
        }

        public ActionResult JF()
        {
           
          

            return View();
        }
        public ActionResult Batch()
        {
            //using (AliPayContent db = new AliPayContent())
            //{
            //    List<EUserInfo> list = db.DBUserInfo.Where(u => u.Id>211 && u.Id<=279).ToList();
            //    EUserInfo updateUI = null;
            //    EUserInfo pUI = db.DBUserInfo.Where(u => u.OpenId == "o3nwE0lm5RKCTJdBVeMsSVNHGIvE").FirstOrDefault();
            //    foreach (EUserInfo ui in list)
            //    {
            //        updateUI = ui;
            //        updateUI.parentOpenId = "";
                   
            //        List<EQRUser> qrUserList = db.DBQRUser.Where(a => a.OpenId == ui.OpenId).ToList();
            //        EQRUser updateQRUesr = null;
            //        foreach (EQRUser qrUser in qrUserList)
            //        {
            //            updateQRUesr = qrUser;
            //            updateQRUesr.ParentOpenId = pUI.OpenId;
            //            updateQRUesr.ParentName = pUI.Name;
            //            updateQRUesr.ParentCommissionRate = (float)0.5;
            //        }
            //    }

            //    db.SaveChanges();
            //}
         
           return View();
        }

        public ActionResult UpdateHeaderLogo(string openId)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
              //  string openId = "o3nwE0jrONff65oS-_W96ErKcaa0";
                using (AliPayContent db = new AliPayContent())
                {
                    var list = db.DBQRUser.Where(o => o.OpenId == openId);
                    EUserInfo ui= db.DBUserInfo.Where(o => o.OpenId == openId).FirstOrDefault();
                    EQRUser qrUser;
                    foreach (EQRUser qr in list)
                    {
                        
                        qrUser = QRManager.CreateUserUrlById(qr, ui.Headimgurl);

                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.IsSuccess = false;
            }

            return Json(result);
        } 
        /// <summary>
        /// 收款码背景图更新
        /// </summary>
        /// <returns></returns>
        public ActionResult Batch_QR()
        {
            OutAPIResult result = new OutAPIResult();
            try
            {

                using (AliPayContent db = new AliPayContent())
                {
                    List<EQRUser> list = db.DBQRUser.ToList();
                    EQRUser updateQR = null;
                    foreach (EQRUser qr in list)
                    {
                        EUserInfo ui = db.DBUserInfo.Where(u => u.OpenId == qr.OpenId).FirstOrDefault();

                        updateQR = db.DBQRUser.Where(a => a.ID == qr.ID).First();

                    //    if(string.IsNullOrEmpty(updateQR.OrigQRFilePath))
                       if(ui.OpenId!= "o3nwE0qI_cOkirmh_qbGGG-5G6B0")
                             updateQR = QRManager.UpdateReceiveQR(updateQR,"2018");



                    }
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.IsSuccess = false;
            }
           
            return Json(result);
        }

        public ActionResult Batch_InviteQR()
        {
            OutAPIResult result = new OutAPIResult();
            try
            {

                using (TransactionScope sc = new TransactionScope())
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        List<EUserInfo> list = db.DBUserInfo.ToList();
                        EUserInfo updateUI;
                        foreach (EUserInfo ui in list)
                        {
                            EQRInfo qr = new EQRInfo();
                            qr.InitCreate();
                            qr.InitModify();
                            qr.OwnnerOpenId = ui.OpenId;
                            qr.ParentOpenId = ui.OpenId;

                            qr.ParentCommissionRate = Convert.ToSingle(1.5);
                            qr.Rate = Convert.ToSingle(6.5);
                            qr.ReceiveStoreId = 1;
                            qr.Channel = IQBCore.IQBPay.BaseEnum.Channel.League;
                            qr.Type = IQBCore.IQBPay.BaseEnum.QRType.ARAuth;


                            qr.Name = "[邀请码]" + ui.Name;
                            if (ui.Name.Length > 40)
                            {
                                qr.Name = qr.Name.Substring(0, 40);
                            }

                            db.DBQRInfo.Add(qr);

                            db.SaveChanges();
                            
                                string url = "http://wx.iqianba.cn/api/wx/CreateInviteQR";
                                string data = string.Format("QRId={0}&QRType={1}", qr.ID, qr.Type);
                                string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
                                SSOQR obj = JsonConvert.DeserializeObject<SSOQR>(res);
                                qr.TargetUrl = obj.TargetUrl;

                                System.Drawing.Image bkImg = ImgHelper.GetImgFromUrl(qr.TargetUrl);
                                string filePath = "/Content/QR/Invite/Orig_QRInvite_" + qr.ID + ".jpg";
                                qr.OrigFilePath = filePath;

                                string fullPath = Server.MapPath(filePath);
                                bkImg.Save(fullPath);

                                Bitmap logo = new Bitmap(Server.MapPath(@"/Content/QR/Logo_AR.png"));

                                Bitmap finImg = ImgHelper.ImageWatermark(new Bitmap(bkImg), logo);

                                filePath = "/Content/QR/Invite/QRInvite_" + qr.ID + ".jpg";
                                qr.FilePath = filePath;
                                fullPath = Server.MapPath(filePath);
                                finImg.Save(fullPath);

                                DbEntityEntry<EQRInfo> entry = db.Entry<EQRInfo>(qr);
                                entry.State = System.Data.Entity.EntityState.Modified;

                                entry.Property(t => t.OrigFilePath).IsModified = true;
                                entry.Property(t => t.FilePath).IsModified = true;
                                entry.Property(t => t.TargetUrl).IsModified = true;

                                db.SaveChanges();

                                updateUI = db.DBUserInfo.Where(a => a.Id == ui.Id).First();
                                updateUI.QRInviteCode = qr.ID;
                                db.SaveChanges();
                                
                           
                        }
                        sc.Complete();
                    }
                   
                }
                   
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
                result.IsSuccess = false;
            }

            return Json(result);
        }

        #region QRHuge

        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult QRHuge()
        {
            float Amount = 1000;

            using (AliPayContent db = new AliPayContent())
            {
                EQRHuge qrHuge = new EQRHuge
                {
                    OpenId = "o3nwE0qI_cOkirmh_qbGGG-5G6B0",
                    Amount = Convert.ToSingle(Amount.ToString("0.00")),
                    CreateDate = DateTime.Now,
                    QRHugeStatus = QRHugeStatus.Created,
                };
                db.DBQRHuge.Add(qrHuge);
                db.SaveChanges();

                string data = string.Format("ID={0}&OpenId={1}&Amount={2}", qrHuge.ID, qrHuge.OpenId, qrHuge.Amount);
                string url = "http://localhost:24068//API/QRAPI/CreateQRHuge";
                string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
                OutAPI_QRHuge Result = JsonConvert.DeserializeObject<OutAPI_QRHuge>(res);

            }

            return Content("OK");
        }
        #endregion

        public ActionResult Other()
        {
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    RQRHugeTrans qrTrans = db.DBQRHugeTrans.Select(s => new RQRHugeTrans {
                        QRHugeId = s.QRHugeId,
                    }).First();

                    EQRHugeTrans EQRHugeTrans = new EQRHugeTrans();

                    EQRHugeTrans.ID = 1;
                    EQRHugeTrans.TransStatus = QRHugeTransStatus.Closed;

                    DbEntityEntry<EQRHugeTrans> entry = db.Entry<EQRHugeTrans>(EQRHugeTrans);
                    entry.State = EntityState.Unchanged;
                    entry.Property(t => t.TransStatus).IsModified = true;

                    EQRHuge EQRHuge = new EQRHuge();
                    EQRHuge.ID = qrTrans.QRHugeId;
                    EQRHuge.QRHugeStatus = QRHugeStatus.Closed;

                    DbEntityEntry<EQRHuge> e2 = db.Entry<EQRHuge>(EQRHuge);
                    e2.State = EntityState.Unchanged;
                    e2.Property(t => t.QRHugeStatus).IsModified = true;

                    db.SaveChanges();
                }
            }
            catch( Exception ex )
            {
                throw ex;
            }
            return Content("OK");
        }
    }
}