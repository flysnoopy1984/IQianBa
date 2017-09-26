﻿using IQBWX.BLL.ExternalWeb;
using IQBWX.BLL.NT;
using IQBWX.Common;
using IQBWX.Controllers;
using IQBWX.DataBase;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public string ResponseContent { get; set; }
        public string ResponseXml { get; set; }
        public string RedirectUrl { get; set; }
        public void ViewHandler(WXMessage msg)
        {

        }
      
        public void ClickHandler(WXMessage msg)
        {
            UserContent db = new UserContent();
            try
            { 
                string ke = msg.EventKey;
                RedirectUrl = null;
              
                if (!db.IsMember(msg.FromUserName))
                {
                    this.ResponseXml = msg.toText("仅限会员查看");
                    return;
                }
           
                log.log("Key:"+ke);
                switch (ke)
                {
                    case "mn_201":

                        break;
                    case "mn_202":
                        EUserInfo ui = db.Get(msg.FromUserName);
                        string picUrl = "http://wx.iqianba.cn/content/qrimg/bk_"+ui.UserId+".jpg";
                        this.ResponseXml = msg.toPicText(picUrl);
                   
                        break;                

                    default:
                        break;

                }
            }
           finally
            {
                db.Dispose(); 
            }




        }

        private void mn_202()
        {

        }

        /// <summary>
        /// true 代表是微信登录
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
                    ssoToken = msg.EventKey.Substring(8);
                
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

                    //用openId注册web,如果已经注册,不会反复注册。
                    BaseExternalWeb exWeb = BaseExternalWeb.GetExternalWeb(sso.AppId);
                    RExternalWebResult result = exWeb.WXInfo(ui,msg);

                    if (result.Status == -1) return true;

                    if (sso != null)
                    {
                        log.log("WXScanLogin AppId:" + sso.AppId);
                        sso.OpenId = msg.FromUserName;
                        sso.LoginStatus = LoginStatus.QRScaned;
                        sso.IsValidate = true;

                        db.SaveChanges();

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
                    log.log("ScanHandler :" + msg.FromUserName);
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
            log.log("newUserSubscribe url:" + url_userInfo);

            WXUserInfo wxUser = HttpHelper.Get<WXUserInfo>(url_userInfo);
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
            controller.Session[IQBConst.SessionToken] = accessToken;

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
                            
                            accessToken = (string)controller.Session[IQBConst.SessionToken];
                            RegistrationNT notice = new RegistrationNT(ui, accessToken);
                            notice.Push();
                        }      

                        this.ResponseXml = msg.toText(string.Format("欢迎来到爱钱吧,亲爱的{0}",ui.nickname));
                            
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