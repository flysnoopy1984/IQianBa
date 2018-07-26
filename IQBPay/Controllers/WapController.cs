using IQBPay.Core;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;
using LitJson;
using IQBCore.WxSDK;
using System.Security.Cryptography;
using System.Text;
using IQBCore.Common.Helper;
using System.IO;
using IQBCore.IQBPay.Models.OutParameter;
using Aop.Api;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.Store;
using Aop.Api.Response;
using System.Configuration;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBWX.BaseEnum;

namespace IQBPay.Controllers
{
    public class WapController : BaseController
    {
        // GET: Wap
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyReceiveQR()
        {
            string FilePath = Request.QueryString["FilePath"];
            if (string.IsNullOrEmpty(FilePath))
                ViewBag.ImgSrc = "/Content/Images/noPic.jpg";
            else
            {
                ViewBag.ImgSrc = FilePath;
            }
            return View();
        }

        public ActionResult Auth_Store(string Id)
        {
          
            EAliPayApplication app = null;
            long qId = 0;
            if(long.TryParse(Id, out qId))
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EQRStoreAuth qr = db.DBQRStoreAuth.Where(a => a.ID == qId).FirstOrDefault();
                    if(qr ==null)
                    {
                        return Content("Error: No QR Find!请联系管理员");
                    }
                    if (qr.APPId == BaseController.App.AppId)
                    {
                        app = BaseController.App;
                    }
                    else if (qr.APPId == BaseController.SubApp.AppId)
                    {
                        app = BaseController.SubApp;
                    }
                    else
                    {
                        return Content("Error: 二维码没有对应的应用!请联系管理员");
                    }

                }
            }
            else
            {
                return Content("Error: No Id Comming!请联系管理员");
            }

        
           
            return Redirect(app.AuthUrl_Store + "&Id=" + Id); 
            //return Content("OK");

        }

        public ActionResult UserVerify()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UploadVerifyFile()
        {
            HttpPostedFileBase file0 = Request.Files[0];
            Stream stream;
            UploadObj UploadObj = new UploadObj();

            int size = file0.ContentLength / 1024; //文件大小KB

            if (size>20480)
            {
                return Content("超过20M");
            }

            byte[] fileByte = new byte[2];//contentLength，这里我们只读取文件长度的前两位用于判断就好了，这样速度比较快，剩下的也用不到。
            stream = file0.InputStream;
            stream.Read(fileByte, 0, 2);//contentLength，还是取前两位
                                        //  Stream stream;
            string fileFlag = "";
            if (fileByte != null || fileByte.Length<= 0)//图片数据是否为空
            {
                fileFlag = fileByte[0].ToString()+fileByte[1].ToString();
            }
            string[] fileTypeStr = { "255216", "7173", "6677", "13780" };//对应的图片格式jpg,gif,bmp,png
            if (fileTypeStr.Contains(fileFlag))
            {
              
                string path = "/Content/UploadFile/AgentVerify";
              
                string fullpath = path;
                if (!Directory.Exists(Server.MapPath(fullpath)))
                {
                    Directory.CreateDirectory(Server.MapPath(fullpath));
                }
                fullpath += "/"+file0.FileName;

                file0.SaveAs(Server.MapPath(fullpath));

                UploadObj.ImgSrc = fullpath;
            }
            else
            {
                return Content("上传错误");
            }

            stream.Close();

            return Json(UploadObj);
        }

        #region MakeQR
        public ActionResult MakeQR(string Amount)
        {
            EAliPayApplication app = BaseController.App;
            string path;
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
           app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);

            EStoreInfo store = null;
            using (AliPayContent db = new AliPayContent())
            {
                store = db.DBStoreInfo.Where(o => o.IsReceiveAccount == true).FirstOrDefault();
            }
            
            F2FPayHandler _handler = new F2FPayHandler();
            IQBCore.IQBPay.Models.User.EUserInfo ui = new IQBCore.IQBPay.Models.User.EUserInfo();
            ui.Name = "大额";
            AlipayTradePrecreateResponse builder = _handler.BuildNew(app, store, ui, Amount,false);
            if(builder.Code == "10000")
            {
                path = _handler.CreateF2FQR(builder.QrCode,true);
                return Content(path);
            }
            else
            {
                return Content("Error");
            }
          
        }

        public ActionResult QRProcess()
        {
            return View();
        }

        #endregion

        #region QRHugeEntry
        public ActionResult PayHuge()
        {
            string reqQRHugeId = Request.QueryString["QRHugeId"];
            string wxSite = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
            string ErrorUrl = wxSite + "Home/ErrorMessage?code=2000&ErrorMsg=";

            if (BaseController.GlobalConfig.WebStatus == PayWebStatus.Stop)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.SystemMaintain, ErrorMsg = BaseController.GlobalConfig.Note });
            }
            if (BaseController.GlobalConfig.QRHugeEntry == QRHugeEntry.Stop)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.SystemMaintain, ErrorMsg ="大额通道维护中，请稍后进入！" });
            }

            EQRHuge qrHuge;
            try
            {
                if (string.IsNullOrEmpty(reqQRHugeId))
                {
                    ErrorUrl += "非扫码进入!";
                    return Redirect(ErrorUrl);
                }
                long QRHugeId;
                if (long.TryParse(reqQRHugeId, out QRHugeId))
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        qrHuge = db.DBQRHuge.Where(o => o.ID == QRHugeId).FirstOrDefault();
                       
                        if (qrHuge == null)
                        {
                            ErrorUrl += "此二维码已损坏，请重新向索要!";
                            return Redirect(ErrorUrl);
                        }
                        if (qrHuge.QRHugeStatus != IQBCore.IQBPay.BaseEnum.QRHugeStatus.Created)
                        {
                            ErrorUrl += "二维码已失效，请重新向索要!";
                            return Redirect(ErrorUrl);
                        }
                        if (DateHelper.IsOverTime(qrHuge.CreateDate, 10*60))
                        {
                            ErrorUrl += "二维码已失效，请重新索要!";
                            qrHuge.QRHugeStatus = IQBCore.IQBPay.BaseEnum.QRHugeStatus.InValid;
                            db.SaveChanges();
                            return Redirect(ErrorUrl);
                        }
                        ViewBag.WXSite_JS_Cookie = wxSite + "Content/Script/jsCookie.js";
                    }
                }
                else
                {
                    ErrorUrl += "二维码ID错误，请重新索要!";
                    return Redirect(ErrorUrl);
                }
            }
            catch (Exception ex)
            {
                ErrorUrl += "错误，请联系您的联系人!";
                Log.log(ex.Message);
                return Redirect(ErrorUrl);
            }


            return View(qrHuge);
         
        }
            #endregion


        }
}