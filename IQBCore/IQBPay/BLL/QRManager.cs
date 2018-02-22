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

        public static string TestQR(EQRUser qrUser)
        {
            string imgPath="";
            //string site = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
            //string url = site + "PP/Pay?Id=" + qrUser.ID;

            //Image LogoImg;
            //string LogoAddr = HttpContext.Current.Server.MapPath("/Content/QR/ReceiveMoney_Logo");
            //LogoImg = new Bitmap(LogoAddr);

            //Bitmap qrImg = QRManager.CreateQR(url, filePath, LogoImg);

            return imgPath;
        }
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
                string bkAdree = HttpContext.Current.Server.MapPath("/Content/QR/BK/bk_spring.jpg");
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
                 string filename = "QRARU_" +qrUser.ID+ "_"+System.DateTime.Now.ToString("yyyyMMdd") + (new Random()).Next(1, 100).ToString()
                 + ".jpg";

                 filePath += filename;

                qrUser.OrigQRFilePath = filePath;
                //Create QR
                filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);

                //Logo
                Image LogoImg = null;
                //根据头像创建收款码Logo
                //if (!string.IsNullOrEmpty(logoUrl))
                //{
                //    LogoImg = ImgHelper.GetImgFromUrl(logoUrl);
                //    LogoImg = ImgHelper.resizeImage(LogoImg, new Size(56, 56));
                //    LogoImg = ImgHelper.AddImgBorder(new Bitmap(LogoImg), 4, Color.Wheat);
                //}

                //收款码统一Logo
                string LogoAddr = HttpContext.Current.Server.MapPath("/Content/QR/ReceiveMoney_Logo.png");
                LogoImg = new Bitmap(LogoAddr);

                Bitmap qrImg = QRManager.CreateQR(url, filePath,LogoImg);
                

                //BK
                //+ "ARUserBK1.jpg";
                string bkAdree = HttpContext.Current.Server.MapPath("/Content/QR/BK/bk_spring.jpg");
                Bitmap bkImg = new Bitmap(bkAdree);
                
                //添加文字
                using (Graphics g = Graphics.FromImage(bkImg))
                {
                    string s = "欢迎使用支付宝付款";
                    Font font = new Font("黑体", 12,FontStyle.Bold);
                    
                    SolidBrush b = new SolidBrush(Color.FromArgb(50,159,250));
                    
                    g.DrawString(s, font, b, new PointF(96,125 ));
                }

                Bitmap finImg = ImgHelper.ImageWatermark(bkImg, qrImg);

                filePath = ConfigurationManager.AppSettings["QR_ARUser_FP"];
                filename = "BK_" + qrUser.ID+"_"+ filename;
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

        public static EQRInfo UpdateMasterUrlById(EQRInfo qr)
        {
            string fp = HttpContext.Current.Server.MapPath(qr.OrigFilePath);
            System.Drawing.Image qrImg = new Bitmap(HttpContext.Current.Server.MapPath(fp));

            Bitmap bkImg = ImgHelper.CreateBlankImg(qrImg.Width, qrImg.Height + 80, Brushes.White);

            using (Graphics g = Graphics.FromImage(bkImg))
            {
                string s = "此邀请码不收费！";
                Font font = new Font("黑体", 12, FontStyle.Bold);
                SolidBrush b = new SolidBrush(Color.FromArgb(50, 159, 250));

                g.DrawString(s, font, b, new PointF(20, qrImg.Height+10));

                s = "平台入驻不收费,请谨防骗子!";              
                g.DrawString(s, font, b, new PointF(20, qrImg.Height + 200));
            }
            Bitmap finImg = ImgHelper.CombineImage(bkImg, qrImg,80);
            string filePath = "/Content/QR/Invite/QRInvite_" + qr.ID + ".jpg";
            qr.FilePath = filePath;

            string fullPath = HttpContext.Current.Server.MapPath(filePath);
            finImg.Save(fullPath);

            finImg.Dispose();
            bkImg.Dispose();

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

                finImg.Dispose();
                bkImg.Dispose();

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
       /// <param name="Logo">如何没有Logo填写null</param>
       /// <returns></returns>
        public static Bitmap CreateQR(string Url,string FilePath,Image Logo,string Text="")
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
                Bitmap blankBK = null;
                if (!string.IsNullOrEmpty(Text))
                {
                    blankBK = ImgHelper.CreateBlankImg(bt.Width + 20, bt.Height + 60, Brushes.White);
                    bt = ImgHelper.CombineImage(blankBK, bt,40);
                    using (Graphics g = Graphics.FromImage(blankBK))
                    {
                        string s = Text;
                        Font font = new Font("宋体", 16);
                        SolidBrush b = new SolidBrush(Color.Black);
                        g.DrawString(s, font, b, new PointF(45, bt.Height-45));
                    }
                }
                else
                {
                    blankBK = ImgHelper.CreateBlankImg(bt.Width + 20, bt.Height + 20, Brushes.White);
                    bt = ImgHelper.CombineImage(blankBK, bt);
                }

                if (Logo!=null)
                {
                  
                    Logo = ImgHelper.resizeImage(Logo, new Size(60, 60));
                    if (!string.IsNullOrEmpty(Text))
                        bt = ImgHelper.CombineImage(bt, Logo,40);
                    else
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

        public static EQRHuge CreateQRHuge(EQRHuge qrHuge)
        {
            string site = ConfigurationManager.AppSettings["Main_SiteUrl"];
            string url = site + "Wap/PayHuge?QRHugeId=" + qrHuge.ID;

            string filePath = "/Content/QR/QRHuge/";
            string filename = "QRHuge_" + qrHuge.ID +"_"+ System.DateTime.Now.ToString("yyyyMMddHHmm") + (new Random()).Next(1, 100).ToString()
            + ".jpg";
            filePath += filename;
            qrHuge.QRUrl = url;
            qrHuge.FilePath = filePath;

            Bitmap logo = new Bitmap(System.Web.HttpContext.Current.Server.MapPath(@"/Content/QR/QRHugeLogo.png"));
           
            filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);

            Bitmap qrImg = QRManager.CreateQR(url, filePath, logo, "金额["+qrHuge.Amount+"]");


            logo.Dispose();
            qrImg.Dispose();

            
            return qrHuge;

        }

        /// <summary>
        /// 收款二维码
        /// </summary>
        public static EQRUser CreateO2OEntryQR(EQRUser qrUser)
        {
            try
            {
                string site = ConfigurationManager.AppSettings["Main_SiteUrl"];
                string url = site + "/O2OWap/Index?qrUserId=" + qrUser.ID;

                string filePath = "/Content/QR/O2O/";
                string filename = "QRO2O_" + qrUser.ID + "_" + System.DateTime.Now.ToString("yyyyMMdd") + (new Random()).Next(1, 100).ToString()
                + ".jpg";

                filePath += filename;

                qrUser.OrigQRFilePath = filePath;
                //Create QR
                filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);

                //Logo
                Image LogoImg = null;
              
                //O2O统一Logo
                string LogoAddr = HttpContext.Current.Server.MapPath("/Content/QR/O2O_Logo.png");
                LogoImg = new Bitmap(LogoAddr);

                Bitmap qrImg = QRManager.CreateQR(url, filePath, LogoImg);


                //BK
                string bkAdree = HttpContext.Current.Server.MapPath("/Content/QR/BK/bk_O2O.jpg");
                Bitmap bkImg = new Bitmap(bkAdree);

                //添加文字
                //using (Graphics g = Graphics.FromImage(bkImg))
                //{
                //    string s = "欢迎来到O2O特惠商城";
                //    Font font = new Font("黑体", 12, FontStyle.Bold);

                //    SolidBrush b = new SolidBrush(Color.FromArgb(50, 159, 250));

                //    g.DrawString(s, font, b, new PointF(96, 125));
                //}

                Bitmap finImg = ImgHelper.ImageWatermark(bkImg, qrImg);

                filePath = "/Content/QR/O2O/";
                filename = "BK_" + qrUser.ID + "_" + filename;
                filePath += filename;

                finImg.Save(HttpContext.Current.Server.MapPath(filePath));
                finImg.Dispose();
                bkImg.Dispose();


                qrUser.FilePath = filePath;
                qrUser.TargetUrl = url;

            }
            catch (Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("CreateUserUrlById Error:" + ex.Message);
                throw ex;
            }

            return qrUser;
        }
    }
}