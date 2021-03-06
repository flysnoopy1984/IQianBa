﻿using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBWX.Const;
using IQBCore.IQBWX.Models.WX.Template.NewMemberReview;
using IQBWX.BLL.ExternalWeb;
using IQBWX.BLL.NT;
using IQBWX.Common;
using IQBWX.Controllers;
using IQBWX.DataBase;
using IQBWX.DataBase.IQBPay;
using IQBWX.Handler;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WxPayAPI;

namespace IQBWX.BLL
{
    public class MenuEvents
    {
        IQBLog log = null;
        public MenuEvents()
        {
            log = new IQBLog();
        }

        public MenuEvents(IQBLog _log)
        {
             log= _log;
        }

        public string ResponseContent { get; set; }
        public string ResponseXml { get; set; }
        public string RedirectUrl { get; set; }
        public void ViewHandler(WXMessage msg)
        {

        }
      
        public void ClickHandler(WXMessage msg)
        {
            string url = ConfigurationManager.AppSettings["Site_IQBPay"];
            string picUrl;
            string GoUrl;
            string desc;
            try
            {
              
                IQBCore.IQBPay.Models.QR.EQRUser payQRUser;
                string ke = msg.EventKey;
                RedirectUrl = null;
                using (AliPayContent db = new AliPayContent())
                {
                    //筛选出当前的收款二维码 
                    payQRUser = db.DBQRUser.Where(u => u.OpenId == msg.FromUserName && u.QRType == QRReceiveType.Small).FirstOrDefault();
                
                    if (payQRUser == null)
                    {
                        this.ResponseXml = msg.toText("没有权限！");
                        return;
                    }
                }
                switch (ke)
                {
                    //收款二维码
                    case "pay_101":
                        picUrl = url + payQRUser.FilePath;
                        GoUrl = url+"/Wap/MyReceiveQR?FilePath="+ payQRUser.FilePath;
                        //  desc = "代理成本：" + (payQRUser.MarketRate-payQRUser.Rate)+"%  |  用户手续费：["+payQRUser.MarketRate+"%]";
                        desc = "代理成本/用户手续费请查看设置";
                        this.ResponseXml = msg.toPicText(picUrl, GoUrl, desc);
                        break;
                    //使用手册
                    case "wx_101":
                        GoUrl = "https://mp.weixin.qq.com/s?__biz=MzUyMzUwOTQ3MA==&mid=100000020&idx=1&sn=fb0bd4f65bdd44985bf137413012cf64&chksm=7a3acaa54d4d43b360d9513a1e810b11d9c13899ad0c50d0cff089f70c88c33037230ef65cad#rd";
                        this.ResponseXml = msg.toPicText("http://wx.iqianba.cn/Content/images/sysc.png", GoUrl, "", "【玉杰】使用手册");
                                                                                               //   log.log("wx_301：" + ResponseXml);
                        break;
                    //使用说明：
                    case "wx_302":
                      
                        break;
                    default:
                        break;

                }
            }
            catch(Exception ex)
            {
                log.log("WX ClickHandler Error" + ex.Message);

                this.ResponseXml = msg.toText("微信平台错误，请通知管理员");
            }

          
        }

        private void mn_202()
        {

        }

        /// <summary>
        /// 用户授权加入平台
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="controller"></param>
        /// <param name="qrId"></param>
        private void IQBAuth(WXMessage msg, WXBaseController controller,string qrId)
        {
            EUserInfo ui = null, pui = null;
            EQRInfo qr = null;
        
            long Id;
            string mText= null; 
            
            using (UserContent udb = new UserContent())
            {
                ui = udb.Get(msg.FromUserName);
                if (ui == null)
                    ui = newUserSubscribe(udb, msg, controller, out pui, false);
                
            }

            if (string.IsNullOrEmpty(qrId) || !long.TryParse(qrId, out Id))
            {
               // log.log("Auth_AR 没有 Id");
                mText = "【传入的Id值不正确】无法授权，请联系平台";
                ResponseXml = msg.toText(mText);
                return;
            }
            using (AliPayContent db = new AliPayContent())
            {
                qr = db.QR_GetById(Id, IQBCore.IQBPay.BaseEnum.QRType.ARAuth);
                if (qr == null)
                {
                    ResponseXml = msg.toText("【授权码不存在】无法授权，请联系平台！");
                     
                    return;
                }
                if (qr.RecordStatus == IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked)
                {
                    ResponseXml = msg.toText("【授权码已经失效】无法授权，请联系平台！");
                    
                    return;
                }
                if (qr.ReceiveStoreId == 0)
                {
                    ResponseXml = msg.toText("【授权码没有收款账户】无法授权，请联系平台！");
                    
                    return;
                }
            }

            ExtWebPay exWeb = new ExtWebPay();
           
            string result = exWeb.regeisterWebMember(ui, qr.ID);
            string url = "https://mp.weixin.qq.com/s?__biz=MzUyMzUwOTQ3MA==&mid=100000020&idx=1&sn=fb0bd4f65bdd44985bf137413012cf64&chksm=7a3acaa54d4d43b360d9513a1e810b11d9c13899ad0c50d0cff089f70c88c33037230ef65cad#rd";
            string note = string.Format("本系统不向任何人收取介绍费用，完全免费。\n请<a href='{0}'>阅读使用手册先</a>。\n如果您已被收费，请向您的介绍人索要回。\n",url);
            if (result.StartsWith("OK") || result.StartsWith("ParentOK"))
            {
            
                mText += "欢迎注册服务平台！\n";
              
                mText += "请等待邀请码的代理审核后使用";
                mText += string.Format("<a href='{0}'>点击阅读使用手册</a>",url);

                string accessToken = controller.getAccessToken(true);

                PPNewMemberReviewNT obj = new PPNewMemberReviewNT(accessToken, 
                    qr.ParentOpenId, 
                    ui.openid, 
                    ui.nickname, 
                    DateTime.Now.ToString());
                obj.Push();



            }
            //else if(result.StartsWith("ParentOK"))
            //{
            //    //IQBCore.IQBPay.Models.User.EUserInfo pUser;
            //    //using (AliPayContent db = new AliPayContent())
            //    //{
            //    //    pUser = db.DBUserInfo.Where(u => u.OpenId == qr.ParentOpenId).FirstOrDefault();
            //    //}
            //    mText += "欢迎注册服务平台！\n";
            //    //mText += string.Format("您当前费率为【{0}%】\n", WXBaseController.GlobalConfig.MarketRate - qr.Rate);
            //    //mText += "首笔订单统一费率2.8%\n之后订单\n";
            //    //mText += "【中介】费率2%\n";
            //    //mText += "【队长】费率1.8%\n";
            //    //mText += "【总代】费率1.5%\n";
            //    mText += "请等待邀请码的代理审核。";
            //    mText += string.Format("<a href='{0}'>点击阅读使用手册</a>", url);
            //}
            else if (result.StartsWith("EXIST"))
            {
                mText += string.Format("你当前收款码的成本为\n【{0}%】\n", WXBaseController.GlobalConfig.MarketRate - qr.Rate);
                mText += note;
                mText += string.Format("<a href='{0}'>请先点击阅读使用手册</a>", url);
            }
            else if (result.StartsWith("ParentEXIST"))
            {
                IQBCore.IQBPay.Models.User.EUserInfo pUser;
                using (AliPayContent db = new AliPayContent())
                {
                    pUser = db.DBUserInfo.Where(u => u.OpenId == qr.ParentOpenId).FirstOrDefault();
                }

                //mText += string.Format("您当前费率为【{0}%】\n", WXBaseController.GlobalConfig.MarketRate - qr.Rate);
                //mText += "首笔订单统一费率2.8%\n之后订单\n";
                //mText += "【中介】费率2%\n";
                //mText += "【队长】费率1.8%\n";
                //mText += "【总代】费率1.5%\n";
                mText += note;
                mText += string.Format("<a href='{0}'>请先点击阅读使用手册</a>", url);
            }
            else if (result.StartsWith("NeedVerification"))
            {
                mText += string.Format("关系您关注平台，请先验证身份\n{0}","<a href=''>进入身份验证</a>");
            }
            else
            {
                mText += result;
                mText += "\n请联系管理员";

            }
            if(!string.IsNullOrEmpty(mText))
            {
                ResponseXml = msg.toText(mText);
            }
            return;

       }

        /// <summary>
        /// true 代表是其他应用程序微信登录
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public Boolean WXScanLogin(WXMessage msg, WXBaseController controller)
        {
            
            try
            {
                ESSOLogin sso = null;
                string ssoToken = null;
                EUserInfo ui =null ,pui = null;
              
                if (msg.Event == "scan")
                    ssoToken = msg.EventKey;
                else if (msg.Event == "subscribe")
                {
                    try
                    {
                        ssoToken = msg.EventKey.Substring(8);
                    }
                    catch
                    {
                        return false;
                    }
                }
                

                //iqianba 代理邀请码进入
                if (ssoToken.StartsWith(IQBConstant.WXQR_IQBPAY_PREFIX))
                {
                    string qrId = ssoToken.Substring(IQBConstant.WXQR_IQBPAY_PREFIX.Length);
                    IQBAuth(msg, controller,qrId);
                    return true;
                }
                //刷单 扫码进入
                if (ssoToken.StartsWith(IQBConstant.WXQR_ShuaDan_PREFIX))
                {
                    NLogHelper.InfoTxt("ShuaDan 进入:" + ssoToken);
                    this.ResponseXml = msg.toText(UserInviteHandle.ShuaDan(msg));
                   // NLogHelper.InfoTxt("ShuaDan Response:" + ResponseXml);
                    return true;
                }

                using (WXContent db = new WXContent())
                {
                    sso = db.GetSSOEntity(ssoToken);
                    if (sso == null) return false;
                    using (UserContent udb = new UserContent())
                    {
                        ui = udb.Get(msg.FromUserName);
                        if (ui == null)
                            ui = newUserSubscribe(udb, msg, controller, out pui, false);
                    }

                    BaseExternalWeb exWeb = BaseExternalWeb.GetExternalWeb(sso.AppId);
                 
                    RExternalWebResult result = exWeb.WXInfo(ui,msg);

                    //log.log("WXScanLogin result.Status:" + result.Status);
                    //用openId注册web,如果已经注册,将不注册。
                   

                    if (sso != null)
                    {
                      //  log.log("WXScanLogin AppId:" + sso.AppId);
                        sso.OpenId = msg.FromUserName;
                        sso.LoginStatus = LoginStatus.QRScaned;
                        sso.IsValidate = true;

                        db.SaveChanges();

                        //if (result.Status == -1)
                        //{
                        //    this.ResponseXml = msg.toText(result.WXMessage);
                        //    return true;
                        //}

                        this.ResponseXml += msg.toText(result.WXMessage);
                        return true;
                    }
                  
                }
                return false;
                
            }
            catch (Exception ex)
            {
                log.log("WXScanLogin Error:" + ex.Message);
                log.log("WXScanLogin Inner Error:" + ex.StackTrace);
                throw ex;
            }
            
        }

      

        public void ScanHandler(WXMessage msg)
        {
            try
            {
               
                using (UserContent db = new UserContent())
                {
                    EUserInfo ui = db.Get(msg.FromUserName);
                 //   log.log("ScanHandler :" + msg.FromUserName);
                    if (ui != null)
                    {                        
                        this.ResponseXml = msg.toText("亲爱的"+ui.nickname+"，欢迎回来！");
                    }
                }
            }
            catch(Exception ex)
            {
                log.log("ScanHandler Error:" + ex.Message);
                log.log("ScanHandler Inner Error:" + ex.StackTrace);
            }
                
        }

       

        private EUserInfo newUserSubscribe(UserContent db,WXMessage msg, WXBaseController controller,out EUserInfo pui, bool isCheckParent = true)
        {
            JsApiPay jsApiPay = new JsApiPay(controller.HttpContext);
            string accessToken = JsApiPay.GetAccessToken();
            string url_userInfo = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}",
            accessToken, msg.FromUserName);
         //   log.log("newUserSubscribe url:" + url_userInfo);

            WXUserInfo wxUser = IQBCore.Common.Helper.HttpHelper.Get<WXUserInfo>(url_userInfo);
            EUserInfo ui = new EUserInfo(wxUser);
            pui = null;
            ui.SubscribeDateTime = DateTime.Now;

            if (isCheckParent && !string.IsNullOrEmpty(msg.Ticket))
            {
                ui.ScanSceneId = msg.EventKey.Substring(8);

                //获取对应的UserInfo, 若没有获取到需要报错
                pui = db.GetBySceneId(ui.ScanSceneId);
                ui.ParentOpenId = pui.openid;
            }

            controller.Session[IQBWXConst.SessionToken] = accessToken;

            db.InsertUserInfo(ui);
            return ui;

        }

        /// <summary>
        /// 关注后触发，可能会带sceneId
        /// </summary>
        /// <param name="msg"></param>
        public void SubscribeHandler(WXMessage msg, WXBaseController controller)
        {
            try
            {
                //log.log("SubscribeHandler EventKey:" + msg.EventKey);
                string accessToken = null;
                using (UserContent db = new UserContent())
                {
                    EUserInfo ui = db.Get(msg.FromUserName);
                    EUserInfo pui = null;
                    if (ui==null)
                    {
                        ui = newUserSubscribe(db, msg, controller,out pui);
                        if (pui != null)
                        {
                            
                            accessToken = (string)controller.Session[IQBWXConst.SessionToken];
                            RegistrationNT notice = new RegistrationNT(ui, accessToken);
                            notice.Push();
                        }      

                        this.ResponseXml = msg.toText(string.Format("欢迎关注,亲爱的{0},本系统免费，不收取任何介绍费。",ui.nickname));
                            
                    }
                    else
                    {
                       
                        this.ResponseXml = msg.toText(string.Format("亲爱的{0}，您回来啦！", ui.nickname));
                    }
                 
                }
              
            }
            catch(Exception ex)
            {
                log.log("SubscribeHandler Error:" + ex.Message);
                log.log("SubscribeHandler Inner Error:" + ex.StackTrace);
            }
         
        }

       
        /*
        public void SubscribeHandler(WXMessage msg, WXBaseController controller)
        {
            try
            {
                //log.log("SubscribeHandler EventKey:" + msg.EventKey);

                using (UserContent db = new UserContent())
                {
                    bool ui = db.IsExistUserInfo(msg.FromUserName);
                    EUserInfo pui = null;
                    if (ui == null)
                    {
                        JsApiPay jsApiPay = new JsApiPay(controller.HttpContext);
                        string accessToken = JsApiPay.GetAccessToken();
                        string url_userInfo = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}",
                        accessToken, msg.FromUserName);

                        WXUserInfo wxUser = HttpHelper.Get<WXUserInfo>(url_userInfo);
                        ui = new EUserInfo(wxUser);
                        ui.SubscribeDateTime = DateTime.Now;
                        if (!string.IsNullOrEmpty(msg.Ticket))
                        {
                            ui.ScanSceneId = msg.EventKey.Substring(8);

                            //获取对应的UserInfo, 若没有获取到需要报错
                            pui = db.GetBySceneId(ui.ScanSceneId);
                            ui.ParentOpenId = pui.openid;

                        }
                        //  controller.Session[IQBConst.SessionOpenId] = msg.FromUserName;
                        // controller.Session[IQBConst.SessionToken] = accessToken;

                        db.InsertUserInfo(ui);

                        if (pui != null)
                        {
                            RegistrationNT notice = new RegistrationNT(ui, accessToken);
                            notice.Push();
                        }

                        this.ResponseXml = msg.toText(@"上线三个月以来，已打造几百个万元户。目前会员人数更是遍布全国。 收益满满，期待满满。全民秒贷，努力打造2017最火爆的贷款直销平台，贷款销售新模式，快来加入吧，赚取属于你的2017第一桶金！");

                    }
                    else
                    {
                        this.ResponseXml = msg.toText(string.Format("欢迎回来，{0}", ui.nickname));
                    }

                }

            }
            catch (Exception ex)
            {
                log.log("SubscribeHandler Error:" + ex.Message);
                log.log("SubscribeHandler Inner Error:" + ex.StackTrace);
            }

        }
        */
       
    }
}