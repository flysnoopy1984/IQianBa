using IQBCore.IQBPay.Models.AccountPayment;
using IQBPay.Core;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class TransferController : BaseController
    {
        // GET: Transfer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Info_Win()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Query(int pageIndex = 0, int pageSize = IQBConfig.PageSize)
        {
            List<ETransferAmount> result = new List<ETransferAmount>();
            try
            {
                string openId = this.GetOpenId(true);

                using (AliPayContent db = new AliPayContent())
                {
                    var list = db.DBTransferAmount.OrderByDescending(i => i.TransDate);
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
                Log.log("Order Query Error:" + ex.Message);
                return Content(ex.Message);
            }
            return Json(result);
        }
    }
}