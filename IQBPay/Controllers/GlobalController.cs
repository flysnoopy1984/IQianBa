using IQBCore.IQBPay.Models.System;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
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
                    obj.Init();
                    db.DBGlobalConfig.Add(obj);
                    db.SaveChanges();
                }
            }
            return Json(obj);
        }
    }

}