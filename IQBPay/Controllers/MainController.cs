using IQBPay.Core;
using IQBPay.Core.BaseEnum;
using IQBPay.DataBase;
using IQBPay.Models.QR;
using IQBPay.Models.System;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
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

        public ActionResult Init()
        {
            using (AliPayContent db = new AliPayContent(true))
            {
                EQRInfo qr = new EQRInfo();
                qr.InitCreate();
                qr.InitModify();
                qr.Channel = QRChannel.PPAuto;
                qr.Type = QRType.AR;
                qr.Rate = 5;
                qr.Name = "平台默认二维码";

                db.DBQRInfo.Add(qr);
                db.SaveChanges();

                qr = QRManager.CreateMasterUrlById(qr);
                db.Entry(qr).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                EAliPayApplication delApp = db.DBAliPayApp.FirstOrDefault();
                if (delApp != null)
                    db.DBAliPayApp.Remove(delApp);

                EAliPayApplication app = new EAliPayApplication();
                app.Version = AliPayConfig.version;
                app.SignType = AliPayConfig.sign_type;
                app.ServerUrl = AliPayConfig.serverUrl;
                app.Charset = AliPayConfig.charset;
                app.RecordStatus = Core.BaseEnum.RecordStatus.Normal;
                app.Merchant_Private_Key = @"MIIEowIBAAKCAQEAwHKeFBun6j3+wQwcgmAoCG7f/TWU1ST+iTKT/oImEiyNOEGrwel3D0TI8/qmeKWVS0u/lKraICRWmIuvd0p8IO5ab3jgYxBJb88NoSCHF8QbZ46BwyLFPc+SdbEvs0VAtyLE4iVzRcBmTHqpo7gt7Ga9MAmaV+x1WUNyiLgxgZqJYQWIyfXkfnGtjL3H6ox+9InwPHIcxZxEEAtdtLZCcrwPCC1zqRp6j191RAFUZ8KNU3zN+J1+QKC1ae4iyt2nBOGtwTVK8LwrMVYIpTffs1sMBNUj3XOqTfwYX7fUFwBrtvaEk4i+CtJopVLNlUxEqauqPEtbPgF1SuxdFIxYPwIDAQABAoIBAHiDThqpdu1pBS8+tluue2NMi1e1Rg5zrDGeSq8GMXEQFR81gKld2gDlwjGGtNi4WFVeigo/M3kNSG0ejDLXogO9P0SvHVTrzhEGSDKue+qWE9M1mmzoSTv70GuDGavZoj0MuN4lNZpocadS6QhtPdTcQXzjhpOor5PGeOLE9buCQz/6YbpgWBKUxWERFZellrgoWaEumDqVSAY4xmflbwL54UIoI/AHlVe3YiKZ2a8RSDpdKQHX4JpU/NHYTI0ZNM6NlXJ5FaAbCdMXgvBQzMz40qg27iF+pCA4jkgS3a5q0BM1+KaB05TlYNh/QHxWaL0Mtt1xgmBlO0n4ho+riwECgYEA9WhqNkzINpHXlGx5opzkUCxA3VCcN2QBkeDFdrdieBfHBIGCVnhmvb+tMq5+mSsEqOdCLmPLAmj3x8XNEt1CiXn34M011/Oh1AcwXgSjtwNySozNsa+qXS57gEoS3xp1n1pkK9bB8Xkebt2l7b2ibkSMLteeuIYl1q4WkLk77qcCgYEAyMEHsyuf8OAL04aGrJWlf4npv7lh9rCqWWLeBrkvt701sDoouwfo5m80i0eWhBbY5Nlh02LqBcJd1Iwk3WSjMA7XNj9CkUWoM4u/hz9YZMigMyPu9EWo7eqGypT9DIhQ67Z1VESlTS0LK2V3bDIkxAJhumgdhCp8iELjEauLVKkCgYBWoXB1EK/Qy7UdeRmLNPVH9AdF2TH8P7pqI72xRdVl7Ybc6Vb4bXJfY22hqYWZTl1Lvq9XLvU4OZPWmtXk5eSaIUtGuUpbnG6xKYSCfALLFVVgScpHAmsSj9kbFYsJ5Q5GnaMk8p/uPUJoAqiTf1D6ugn+czFdlEWBPl1K44jrmwKBgQCE+i/yg7wfHxlWVO7SVRHaKG1YXSDB+oXsTawKQhKUn9V3VR7zvKqOMS1Z8OKHvmaPOFsvXX7sr7Hdf7NPn0DlLX9q5H5gogZnlnMY0GHp6GcNWQkIbzgV2FrOx9/StFz9tc+EMTBZrbOPXFe9qH1oBLfddOfQSyBQVhX492uEeQKBgA/Umil0P/q/nECzL+71LMlpGM6ldRcOsnHKh+5/48h5VImiFn8YhOewboYUnkoBJBJXf5upj3GA+J2OSkojxgOwfBC+EWreXmGltYw/2bZ3CO08Fs80vXDbhMaPIUcjfsS4CCxx0qIJanhf/6522kT3avj5aA4jYE04by8qV4KI";
                app.Merchant_Public_key = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA3aaDFxkj4IfzV42j8lwdJCZgTPTfrwDTiFfxhQXwFk/9sstsNdSkrYzKAmMBgl95d7R9bA8ASc0A8JADgR1ye+gsky9K/l8DEI8ZbgSWgCdEkTHbuZtzLo0SN9Q+U6k/g3QWTV27+0WHXHNwECFhdk23V0s2MeF+HrYgPn0WSkpYwz58hCDV9Eh71sj05tcgWfitcEkMLSazXmDqRsv8LZjtzpXO9Chwssfi9iCWa3hfsuzfmXusk8TRwtRyUtD9hIq4Fxr2+QJ2AvMlyK7/Sgtnsgl+lIv869jVyaNydlwSv8js1TM8nXPVemTWvj7fQUnWhU0YRHVa0XcdeyvaBwIDAQAB";
                app.AppId = "2017092008828512";
                app.AppName = "dingylpost@163.com";
                app.InitModify();
                app.InitCreate();
                db.DBAliPayApp.Add(app);

                db.SaveChanges();
            }


            return View();
        }

        public ActionResult Login()
        {
            string WXurl =  ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
            ViewData["WXUrl"] = WXurl+ "?logintype=pp";
            return View();
        }

        public ActionResult WebEntry(string openId)
        {
            SetOpenId(openId);
            return RedirectToAction("Profile", "User");
        }

        #region SysConfig  
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