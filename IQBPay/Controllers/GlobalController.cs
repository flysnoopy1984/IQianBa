using IQBCore.IQBPay.Models.System;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class GlobalController : BaseController
    {
        // GET: Global
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult GetOrCreate()
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

        public ActionResult Save(EGlobalConfig obj)
        {
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    DbEntityEntry<EGlobalConfig> entry = db.Entry<EGlobalConfig>(obj);
                    entry.State = EntityState.Unchanged;

                    entry.Property(t => t.Note).IsModified = true;
                    entry.Property(t => t.WebStatus).IsModified = true;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Content("Save Error" + ex.Message);
            }
            return Content("OK");
        }
    }

}