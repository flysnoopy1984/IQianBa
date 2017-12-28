using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Sys;
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
using System.Security.Policy;
using System.IO;

namespace IQBCore.IQBPay.BLL
{
    public class QRManager
    {
        /// <summary>
        /// 更新收款二维码
        /// </summary>
        /// <param name="qrUser"></param>
        /// <param name="logoUrl"></param>
        /// <returns></returns>
        public static EQRUser UpdateReceiveQR(EQRUser qrUser,string SeqNo)
        {
            try
            {
             


               
                string filename = qrUser.OrigQRFilePath;
                string filePath = HttpContext.Current.Server.MapPath(qrUser.OrigQRFilePath);
                FileInfo fi = new FileInfo(filePath);


                Bitmap qrImg = new Bitmap(filePath);


                //BK
                //+ "ARUserBK1.jpg";
                string bkAdree = HttpContext.Current.Server.MapPath("/Content/QR/BK/ARUserBK1.jpg");
                Bitmap bkImg = new Bitmap(bkAdree);
                Bitmap finImg = ImgHelper.ImageWatermark(bkImg, qrImg);

                filePath = ConfigurationManager.AppSettings["QR_ARUser_FP"];
                filename = "BK_"+ SeqNo +"_"+ fi.Name;
                filePath += filename;

                finImg.Save(HttpContext.Current.Server.MapPath(filePath));
                finImg.Dispose();
                bkImg.Dispose();
                

                qrUser.FilePath = filePath;
              

            }
            catch (Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("CreateUserUrlById Error:" + ex.Message);
                throw ex;
            }

            return qrUser;
        }

        /// <summary>
        /// 收款二维码
        /// </summary>
        public static EQRUser CreateUserUrlById(EQRUser qrUser,string logoUrl)
        {
            try
            { 
                 string site = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
                 string url = site + "PP/Pay?Id=" + qrUser.ID;

                 string filePath = ConfigurationManager.AppSettings["QR_ARUser_FP"];
                 string filename = "QRARU_" +qrUser.ID+ System.DateTime.Now.ToString("yyyyMMdd") + (new Random()).Next(1, 100).ToString()
                 + ".jpg";

                 filePath += filename;

                qrUser.OrigQRFilePath = filePath;
                //Create QR
                filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);

                //Logo
                Image LogoImg = null;
                if (!string.IsNullOrEmpty(logoUrl))
                {
                    LogoImg = ImgHelper.GetImgFromUrl(logoUrl);
                    LogoImg = ImgHelper.resizeImage(LogoImg, new Size(56, 56));
                    LogoImg = ImgHelper.AddImgBorder(new Bitmap(LogoImg), 4, Color.Wheat);
                }
                

                Bitmap qrImg = QRManager.CreateQR(url, filePath,LogoImg);
                

                //BK
                //+ "ARUserBK1.jpg";
                string bkAdree = HttpContext.Current.Server.MapPath("/Content/QR/BK/ARUserBK1.jpg");
                Bitmap bkImg = new Bitmap(bkAdree);
                Bitmap finImg = ImgHelper.ImageWatermark(bkImg, qrImg);

                filePath = ConfigurationManager.AppSettings["QR_ARUser_FP"];
                filename = "BK_" + filename;
                filePath += filename;

                finImg.Save(HttpContext.Current.Server.MapPath(filePath));
                finImg.Dispose();
                bkImg.Dispose();


                qrUser.FilePath = filePath;
                qrUser.TargetUrl = url;

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

        public static EQRInfo CreateMasterUrlById(EQRInfo qr, HttpContext context)
        {
            try
            {

                if(qr.ID ==0 )
                {
                    throw new Exception("创建QR错误，QR ID 不存在");
                }
                string url = "http://wx.iqianba.cn/api/wx/CreateInviteQR";
                string data = string.Format("QRId={0}&QRType={1}",qr.ID,qr.Type);
                string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
                SSOQR obj = JsonConvert.DeserializeObject<SSOQR>(res);
                qr.TargetUrl = obj.TargetUrl;

                System.Drawing.Image bkImg = ImgHelper.GetImgFromUrl(qr.TargetUrl);
                string filePath = "/Content/QR/Invite/Orig_QRInvite_" + qr.ID + ".jpg";
                qr.OrigFilePath = filePath;

                string fullPath = context.Server.MapPath(filePath);
                bkImg.Save(fullPath);

                Bitmap logo = new Bitmap(context.Server.MapPath(@"/Content/QR/Logo_AR.png"));

                Bitmap finImg = ImgHelper.ImageWatermark(new Bitmap(bkImg), logo);

                filePath = "/Content/QR/Invite/QRInvite_" + qr.ID + ".jpg";
                qr.FilePath = filePath;
                fullPath = context.Server.MapPath(filePath);
                finImg.Save(fullPath);
                return qr;

            }
            catch (Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("CreateMasterUrlById Error:" + ex.Message);
                throw ex;
            }
          
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
                QRManager.CreateQR(url, filePath,null);
            }
            catch (Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("CreateStoreAuthUrlById Error:" + ex.Message);
                throw ex;
            }
            return qr;
        }

        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static Bitmap CreateQR(string Url,string FilePath,Image Logo)
        {
            IQBLog log = new IQBLog();
            Bitmap bt = null;
            try
            { 
               
         
                string enCodeString = Url;
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                qrCodeEncoder.QRCodeScale = 4;
                qrCodeEncoder.QRCodeVersion = 9;
                bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            //    log.log("CreateQR qrCodeEncoder");

                Bitmap blankBK = ImgHelper.CreateBlankImg(bt.Width + 20, bt.Height + 20, Brushes.White);
          //     log.log("CreateQR blankBK");
                bt = ImgHelper.CombineImage(blankBK, bt);
            //    log.log("CreateQR Combine BK");
                if (Logo!=null)
                {
                    bt = ImgHelper.CombineImage(bt, Logo);
                }
             //   log.log("CreateQR Combine Log");
                bt.Save(FilePath);
            }
            catch(Exception ex)
            {
               
                log.log("CreateQR Error:" + ex.Message);
                log.log("CreateQR Error:" + ex.InnerException.Message);
                throw ex;
            }


            return bt;

        }



    }
}