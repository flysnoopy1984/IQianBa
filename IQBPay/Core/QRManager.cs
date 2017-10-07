using IQBCore.Common.Helper;
using IQBPay.Controllers;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.System;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;
using IQBCore.IQBWX.Models.OutParameter;
using Newtonsoft.Json;

namespace IQBPay.Core
{
    public class QRManager
    {
        
        /// <summary>
        /// 普通用户扫码进入后生成二维码
        /// </summary>
        public static EQRUser CreateUserUrlById(EQRUser qrUser)
        {
            try
            { 
            string site = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
            string url = site + "PP/Pay?Id=" + qrUser.QRId;

            string filePath = ConfigurationManager.AppSettings["QR_ARUser_FP"];
            string filename = "QRARU" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + (new Random()).Next(1, 100).ToString()
            + ".jpg";

            filePath += filename;
            qrUser.FilePath = filePath;
            qrUser.TargetUrl = url;

            //Create QR
            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            QRManager.CreateQR(url, filePath);
            }
            catch(Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("CreateUserUrlById Error:" + ex.Message);
                throw ex;
            }

            return qrUser;
        }

        public static EQRInfo Create_PP_AR(EQRInfo qr)
        {/*
            string site = ConfigurationManager.AppSettings["Main_SiteUrl"];
            string url = site + "Wap/Auth_AR?qrId="+qr.ID;

            string filePath = QRManager.CreateQR(url);
            qr.Url = url;
            qr.FilePath = filePath;
            */
            return qr;
        }

        public static EQRInfo CreateMasterUrlById(EQRInfo qr)
        {
            try
            {
                /*
                 string site = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
                 string url = site + "PP/Auth_AR?Id=" + qr.ID;

                 string filePath = ConfigurationManager.AppSettings["QR_ARMaster_FP"];
                 string filename = "QRARU" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + (new Random()).Next(1, 100).ToString()
                 + ".jpg";

                 filePath += filename;
                 qr.FilePath = filePath;
                 qr.TargetUrl = url;

                 //Create QR
                 // filePath = PageController.Server.MapPath(filePath);
                 filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
                 QRManager.CreateQR(url, filePath);
                 */
                if(qr.ID ==0 )
                {
                    throw new Exception("创建QR错误，QR ID 不存在");
                }
                string url = "http://wx.iqianba.cn/api/wx/CreatePayQRAuth";
                string data = string.Format("QRId={0}&QRType={1}",qr.ID,qr.Type);
                string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
                SSOQR obj = JsonConvert.DeserializeObject<SSOQR>(res);
                qr.TargetUrl = obj.QRImgUrl;
                
            }
            catch(Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("CreateMasterUrlById Error:" + ex.Message);
                throw ex;
            }
            return qr;
        }

        public static EQRInfo CreateStoreAuthUrlById(EQRInfo qr)
        {
            try
            {
                string site = ConfigurationManager.AppSettings["Main_SiteUrl"];
                string url = site + "Wap/Auth_Store?Id=" + qr.ID;

                string filePath = ConfigurationManager.AppSettings["QR_AuthStore_FP"];
                string filename = "QRAS" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + (new Random()).Next(1, 100).ToString()
                + ".jpg";

                filePath += filename;
                qr.FilePath = filePath;
                qr.TargetUrl = url;

                //Create QR
                // filePath = PageController.Server.MapPath(filePath);
                filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);
                QRManager.CreateQR(url, filePath);
            }
            catch (Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("CreateStoreAuthUrlById Error:" + ex.Message);
                throw ex;
            }
            return qr;
        }

        public static bool CreateQR(string Url,string FilePath)
        {
            try
            { 
                Bitmap bt;
         
                string enCodeString = Url;
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                qrCodeEncoder.QRCodeScale = 3;
                qrCodeEncoder.QRCodeVersion = 0;
                bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
           
           
                bt.Save(FilePath);
            }
            catch(Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("CreateQR Error:" + ex.Message);

                throw ex;
            }


            return true;

        }



    }
}