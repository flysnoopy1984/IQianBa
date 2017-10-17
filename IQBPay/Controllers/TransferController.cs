using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.Result;
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

        public ActionResult DoTransfer()
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
                Log.log("Transfer Query Error:" + ex.Message);
                return Content(ex.Message);
            }
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="type">1 Order, 2 Transfer</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InfoWin(string Id,string type)
        {
            string openId = this.GetOpenId(true);
           
            EOrderInfo order;
            ETransferAmount transfer;

            ROrder_Transfer result = new ROrder_Transfer();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    if(type == "1")
                    {
                        order = db.DBOrder.Where(u => u.OrderNo == Id).FirstOrDefault();
                        if(order!=null)
                        {
                            result.Order = order;
                            result.TransferList = db.DBTransferAmount.Where(t => t.OrderNo == order.OrderNo).ToList();
                            if (result.TransferList == null || result.TransferList.Count == 0)
                                result.Result = -1;
                        }
                        else
                            result.Result = -2;
                    }
                    if(type =="2")
                    {
                        transfer = db.DBTransferAmount.Where(u => u.TransferId == Id).FirstOrDefault();
                        if (transfer != null)
                        {
                            result.Transfer = transfer;
                            result.OrderList = db.DBOrder.Where(t => t.TransferId == transfer.TransferId).ToList();
                            if (result.OrderList == null || result.OrderList.Count == 0)
                                result.Result = -1;
                        }
                        else
                            result.Result = -2;
                    }
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