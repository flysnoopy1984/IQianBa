﻿using IQBPay.Core;
using IQBCore.IQBPay.BaseEnum;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Sys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using IQBCore.IQBPay;
using IQBCore.Common.Constant;
using IQBCore.IQBPay.BLL;
using System.Data.Entity;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.User;
using Com.Alipay.Model;

namespace IQBPay.Controllers
{
    public class MainController : BaseController
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        #region Global Config  
        //  [IQBPayAuthorize_Admin]
        public ActionResult GlobalConfig()
        {
            return View();
        }


      

        public ActionResult GlobalGetOrCreate()
        {
            EGlobalConfig obj = null;
            using (AliPayContent db = new AliPayContent())
            {
                obj = db.DBGlobalConfig.FirstOrDefault();
                if (obj == null)
                {
                    obj = new EGlobalConfig();
                    obj.Init();
                    db.DBGlobalConfig.Add(obj);
                    db.SaveChanges();
                }
            }
            return Json(obj);
        }

        public ActionResult GlobalSave(EGlobalConfig obj)
        {
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    DbEntityEntry<EGlobalConfig> entry = db.Entry<EGlobalConfig>(obj);
                    entry.State = EntityState.Unchanged;

                    entry.Property(t => t.MarketRate).IsModified = true;
                    entry.Property(t => t.Note).IsModified = true;
                    entry.Property(t => t.WebStatus).IsModified = true;
                    entry.Property(t => t.QRHugeEntry).IsModified = true;
                    entry.Property(t => t.QRHugeInValidMinutes).IsModified = true;
                    entry.Property(t => t.QRHugeMin).IsModified = true;
                    entry.Property(t => t.QRHugeMax).IsModified = true;
                    entry.Property(t => t.QRMin).IsModified = true;
                    entry.Property(t => t.QRMax).IsModified = true;
                    //entry.Property(t => t.AllowRegister).IsModified = true;
                    //entry.Property(t => t.IsWXNotice_AgentTransfer).IsModified = true;

                    db.SaveChanges();

                    BaseController.GlobalConfig = obj;

                    //WX客户端
                    string url = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
                    url += "API/OutData/RefreshGlobelConfig";
                    HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, "", "application/x-www-form-urlencoded");

                }
            }
            catch (Exception ex)
            {
                return Content("Save Error" + ex.Message);
            }
            return Content("OK");
        }

        #endregion

        public ActionResult Message()
        {
        
            return View();
        }

        [IQBPayAuthorize_Admin]
        public ActionResult Init()
        {
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    if(IQBConstant.NeedDefaultQRModule)
                    {
                        EQRInfo delqr = db.DBQRInfo.Where(a=>a.Channel == Channel.PPAuto).FirstOrDefault();
                        if (delqr != null)
                            db.DBQRInfo.Remove(delqr);

                        EQRInfo qr = new EQRInfo();
                        qr.InitCreate();
                        qr.InitModify();
                        qr.Channel = Channel.PPAuto;
                        qr.Type = QRType.ARAuth;
                        qr.Rate = 5;
                        qr.Name = "平台默认二维码";

                        db.DBQRInfo.Add(qr);
                        db.SaveChanges();
                        
                        qr = QRManager.CreateMasterUrlById(qr, System.Web.HttpContext.Current);
                        db.Entry(qr).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    //AliPay Application
                    EAliPayApplication delApp = db.DBAliPayApp.FirstOrDefault();
                    if (delApp != null)
                        db.DBAliPayApp.Remove(delApp);

                    EAliPayApplication app = new EAliPayApplication();
                    app.Version = AliPayConfig.version;
                    app.SignType = AliPayConfig.sign_type;
                    app.ServerUrl = AliPayConfig.serverUrl;
                    app.Charset = AliPayConfig.charset;
                    app.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Normal;
                    app.Merchant_Private_Key = AliPayConfig.merchant_private_key;
                    app.Merchant_Public_key = AliPayConfig.merchant_public_key;
                    app.AppId = AliPayConfig.appId;
                    app.AuthUrl_Store = AliPayConfig.AuthUrl_Store;
                    app.AppName = "dingylpost@163.com";
                    app.IsCurrent = true;

                    app.InitModify();
                    app.InitCreate();
                    db.DBAliPayApp.Add(app);

                    //

                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                return Content("初始化失败:"+ex.Message);
            }


            return Content("初始化完成");
        }

        public ActionResult Login()
        {
         
            string action = Request.QueryString["action"];
            if(action == "exit")
            {
                if(GetUserSession().UserRole != UserRole.Administrator)
                {
                    return Redirect("/O2O/Login?action=exit");
                }
                base.ExitSession();
            }

            //if(BaseController.GlobalConfig.WebStatus == PayWebStatus.Stop)
            //{
            //   return RedirectToAction("Message", "Main");
            //}
          

            string WXurl =  ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
            ViewData["WXUrl"] = WXurl+ "?logintype=o2o";
            return View();
        }

        public ActionResult WebEntry(string openId)
        {

            using (AliPayContent db = new AliPayContent())
            {
                EUserInfo ui =   db.DBUserInfo.Where(u => u.OpenId == openId).FirstOrDefault();
                if(ui == null)
                {
                   return Redirect("/Main/Login?action="+ ExistAction.noRegister.ToString());
                }

            }
            this.SetUserSession(openId);

            return RedirectToAction("Profile", "User");
        }

        [IQBPayAuthorize_Admin]
        public ActionResult Tools(string act)
        {
            if(act == "1")
            {
                BaseController.CleanApp();
            }
            ViewBag.Auth_Store = BaseController.App.AuthUrl_Store;
            ViewBag.PublicKey = BaseController.App.Merchant_Public_key;
        
            return View();
        }

        

        #region AppList
        //[IQBPayAuthorize_Admin]
        public ActionResult AppList()
        {
            return View();
        }
       // [IQBPayAuthorize_Admin]
        public ActionResult AppQuery(int pageIndex = 0, int pageSize = IQBConstant.PageSize)
        {
            //throw new Exception("Test Error");
          
            List<EAliPayApplication> result = new List<EAliPayApplication>();
            try
            {
                string openId = Convert.ToString(Session["OpenId"]);

                using (AliPayContent db = new AliPayContent())
                {
                    var list = db.DBAliPayApp.OrderByDescending(i => i.CreateDate);

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
                throw ex;
            }
            return Json(result);
        }

        #endregion

        #region AppInfo
        [IQBPayAuthorize_Admin]
        public ActionResult AppInfo()
        {
            return View();
        }

      
        public ActionResult GetApp(int Id = -1)
        {
            EAliPayApplication result = null;
         
            if (Id == -1)
            {
                result = new EAliPayApplication();
                result.Version = AliPayConfig.version;
                result.SignType = AliPayConfig.sign_type;
                result.ServerUrl = AliPayConfig.serverUrl;
                result.Charset = AliPayConfig.charset;
                result.AuthUrl_Store = AliPayConfig.AuthUrl_Store;
                result.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Normal;
            }     
            else
            {
                using (AliPayContent db = new AliPayContent())
                {
                    result = db.DBAliPayApp.Where(a => a.ID == Id).FirstOrDefault();
                }
            }
            return Json(result);
        }

        public ActionResult AddApp(EAliPayApplication app)
        {
            try
            {

                using (AliPayContent db = new AliPayContent())
                {
                    if (app.IsCurrent)
                    {
                        EAliPayApplication curAPP = db.DBAliPayApp.Where(a => a.IsCurrent == true).FirstOrDefault();
                        if (curAPP != null)
                            curAPP.IsCurrent = false;
                    }

                    db.DBAliPayApp.Add(app);
                    db.SaveChanges();
                    //清楚系统 App Caceh
                    BaseController.CleanApp();

                }
            }
            catch (Exception ex)
            {
                return Content("Save Error" + ex.Message);
            }
            return Json("OK");
        }

        public ActionResult UpdateApp(EAliPayApplication app)
        {
            try
            {
               
                using (AliPayContent db = new AliPayContent())
                {
                    EAliPayApplication updateApp = db.DBAliPayApp.Single(a => a.ID == app.ID);
                    if (updateApp != null)
                    {
                        if (app.IsCurrent)
                        {
                            EAliPayApplication curAPP = db.DBAliPayApp.Where(a => a.IsCurrent == true && a.ID!=app.ID).FirstOrDefault();
                            if (curAPP != null)
                                curAPP.IsCurrent = false;
                        }

                        db.Entry(updateApp).CurrentValues.SetValues(app);
                       
                        db.SaveChanges();



                        //清楚系统 App Caceh
                        BaseController.CleanApp();
                    }
                    else
                    {
                        return Content("没有获取当前数据");
                      
                    }

                }
            }
            catch (Exception ex)
            {
               return Content("Save Error" + ex.Message);
            }
            return Json("OK");
        }
        #endregion
    }

    #region SysConfig  
    /*
    public ActionResult SysConfig()
    {
        return View();
    }

    public ActionResult GetSysConfig(string Id)
    {
        ESysConfig result = null;

        using (AliPayContent db = new AliPayContent())
        {
            result = db.DBSysConfig.FirstOrDefault();
            if (result == null)
            {
                result = new ESysConfig();
            }
        }
        return Json(result);
    }

    public ActionResult SaveSysConfig(ESysConfig obj)
    {
        try
        {
            using (AliPayContent db = new AliPayContent())
            {

                ESysConfig updateApp = db.DBSysConfig.FirstOrDefault(a => a.ID == obj.ID);
                if (updateApp != null)
                {
                    db.Entry(updateApp).CurrentValues.SetValues(obj);

                    db.SaveChanges();
                }
                else
                {
                    obj.ID = "IQBPPConfig";
                    obj.InitCreate();
                    db.DBSysConfig.Add(obj);
                    db.SaveChanges();

                }

            }

        }
        catch (Exception ex)
        {
            return Content("Save Error" + ex.Message);
        }
        return Json("OK");
    }


*/
    #endregion
}