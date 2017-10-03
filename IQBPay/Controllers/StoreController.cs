using IQBPay.Core;
using IQBPay.DataBase;
using IQBPay.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class StoreController : BaseController
    {
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            Session["OpenId"] = this.GetOpenId(true) ;

        
            return View();
        }

        [HttpPost]
        public ActionResult Query(int pageIndex = 0, int pageSize = IQBConfig.PageSize)
        {
            List<EStoreInfo> result = new List<EStoreInfo>();
            try
            {
                string openId = Convert.ToString(Session["OpenId"]);

                using (AliPayContent db = new AliPayContent())
                {
                    var list = db.DBStoreInfo.Where(i => i.OwnnerOpenId == openId).OrderByDescending(i => i.CreateDate);

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
            catch(Exception ex)
            {
                throw ex;
            }
            return Json(result);
        }

        public ActionResult Add(EStoreInfo store)
        {
            try
            {
              
                store.OwnnerOpenId = this.GetOpenId(true);
                using (AliPayContent db = new AliPayContent())
                {
                    if(db.IsExistStore(store.OwnnerOpenId, store.Name))
                    {
                        return Content("同名店铺已经存在");
                    }
                    else
                    {
                        db.DBStoreInfo.Add(store);
                        db.SaveChanges();
                    }

                }
            }
            catch(Exception ex)
            {
                Content("Save Store Error"+ex.Message);
            }
            return Json("OK");
        }


        public ActionResult Info()
        {
            return View();
        }

        
    }
}