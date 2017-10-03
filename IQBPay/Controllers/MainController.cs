using IQBPay.Core;
using IQBPay.DataBase;
using IQBPay.Models.System;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Login()
        {
            string WXurl =  ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
            ViewData["WXUrl"] = WXurl+ "?logintype=pp";
            return View();
        }

        public ActionResult Register()
        {

            return View();
        }

        #region SysConfig  
        public ActionResult SysConfig()
        {
            return View();
        }

        public ActionResult GetSysConfig(string Id)
        {
            ESysConfig result = null;

            using (SysContent db = new SysContent())
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
                using (SysContent db = new SysContent())
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

       

        #endregion

        #region AppList

        public ActionResult AppList()
        {
            return View();
        }

        public ActionResult AppQuery(int pageIndex = 0, int pageSize = IQBConfig.PageSize)
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
                result.RecordStatus = Core.BaseEnum.RecordStatus.Normal;
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
                    db.DBAliPayApp.Add(app);
                    db.SaveChanges();

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
                        db.Entry(updateApp).CurrentValues.SetValues(app);
                       
                        db.SaveChanges();
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
}