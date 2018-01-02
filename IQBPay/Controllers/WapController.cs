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
                    EQRInfo qr = db.DBQRInfo.Where(a => a.ID == qId).FirstOrDefault();
                    if(qr ==null)
                    {
                        return Content("Error: No QR Find!请联系管理员");
                    }
                    if(qr.APPId == BaseController.App.AppId)
                    {
                        app = BaseController.App;
                    }
                    else if(qr.APPId == BaseController.SubApp.AppId)
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


    }
}