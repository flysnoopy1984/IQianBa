using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBWX.Models.WX.Template;
using IQBWX.BLL.ExternalWeb;
using IQBWX.BLL.NT;
using IQBWX.DataBase.IQBPay;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using IQBWX.Models.WX.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace IQBWX.Controllers
{
    public class PPController : WXBaseController
    {
        private IQBLog _Log;

        public PPController()
        {
            _Log = new IQBLog();
        }


       
        public ActionResult YunLong()
        {
            return View();
        }

        public ActionResult Demo(string Id)
        {
           
            if (Id == "1")
            {
               
            }
            else if(Id=="SMS")
            {

            }
             return View();
        }

        public ActionResult Pay(string Id)
        {
            ViewBag.QRUserId = Id;
            ViewBag.OrderNo = StringHelper.GenerateOrderNo();
            return View();
        }

        public ActionResult Auth_Store(string Rate)
        {
            ViewBag.Rate = Rate;
           
            return View();
        }
        public ActionResult AliPayAccount()
        {
            return View();
        }

        public ActionResult OrderList()
        {
            string openId = this.GetOpenId(true);
            ViewBag.OpenId = openId;
            return View();
        }


        [HttpPost]
        public ActionResult OrderQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);
            string OpenId = Request["OpenId"];
            ConditionDataType DateType =(ConditionDataType)Enum.Parse(typeof(ConditionDataType),Request["DateType"]);
            List<ROrderInfo> result = new List<ROrderInfo>();
            try
            {
                
                using (AliPayContent db = new AliPayContent())
                {
                    var list = db.DBOrderInfo.Where(o => o.AgentOpenId == OpenId).Select(o=>new ROrderInfo {
                        ID = o.ID,
                        OrderNo = o.OrderNo,
                        TransDateStr = o.TransDateStr,
                        OrderStatus = o.OrderStatus,
                        TotalAmount = o.TotalAmount,
                        RealTotalAmount = o.RealTotalAmount,
                        BuyerAliPayLoginId = o.BuyerAliPayLoginId,
                        TransDate = o.TransDate,
                    });

                    if (DateType != ConditionDataType.All)
                    {
                        if (DateType == ConditionDataType.Today)
                        {
                            list = list.Where(o => o.TransDate == DateTime.Today);
                        }
                        else if (DateType == ConditionDataType.Week)
                        {
                            DateTime startDate = UtilityHelper.GetTimeStartByType("Week", DateTime.Now);
                            DateTime endDate = UtilityHelper.GetTimeEndByType("Week", DateTime.Now);
                            list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);

                        }
                        else if (DateType == ConditionDataType.Month)
                        {
                            DateTime startDate = UtilityHelper.GetTimeStartByType("Month", DateTime.Now);
                            DateTime endDate = UtilityHelper.GetTimeEndByType("Month", DateTime.Now);
                            list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);
                        }
                    }
                    list = list.Where(o => o.OrderStatus == OrderStatus.Paid);
                    list = list.OrderByDescending(i => i.TransDate);

                    if (pageIndex == 0)
                    {
                        int totalCount = list.Count();
                        result = list.Take(pageSize).ToList();
                        if (result.Count > 0)
                        {
                            result[0].TotalCount = totalCount;
                         //   result[0].TotalAmountSum = list.Sum(o => o.TotalAmount);
                            result[0].RealTotalAmountSum = list.Sum(o => o.RealTotalAmount);
                        }
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

        public ActionResult TransferList()
        {
            string openId = this.GetOpenId(true);
            ViewBag.OpenId = openId;
            return View();
        }

        [HttpPost]
        public ActionResult TransferQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);
            string OpenId = Request["OpenId"];
            ConditionDataType DateType = (ConditionDataType)Enum.Parse(typeof(ConditionDataType), Request["DateType"]);

            List<RTransferAmount> result = new List<RTransferAmount>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {

                    var list = db.DBTransferAmount.Where(o => o.AgentOpenId == OpenId).Select(s => new RTransferAmount
                    {
                        ID = s.ID,
                        TransferId = s.TransferId,
                        TransDateStr = s.TransDateStr,
                        TransferAmount = s.TransferAmount,
                        AgentAliPayAccount = s.AgentAliPayAccount,
                        TransDate = s.TransDate,

                    });

                    if (DateType != ConditionDataType.All)
                    {
                        if (DateType == ConditionDataType.Today)
                        {
                            list = list.Where(o => o.TransDate == DateTime.Today);
                        }
                        else if (DateType == ConditionDataType.Week)
                        {
                            DateTime startDate = UtilityHelper.GetTimeStartByType("Week", DateTime.Now);
                            DateTime endDate = UtilityHelper.GetTimeEndByType("Week", DateTime.Now);
                            list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);

                        }
                        else if (DateType == ConditionDataType.Month)
                        {
                            DateTime startDate = UtilityHelper.GetTimeStartByType("Month", DateTime.Now);
                            DateTime endDate = UtilityHelper.GetTimeEndByType("Month", DateTime.Now);
                            list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);
                        }
                    }

                    list = list.OrderByDescending(i => i.TransDate);

                    if (pageIndex == 0)
                    {

                        result = list.Take(pageSize).ToList();
                        if (result.Count > 0)
                        {
                            result[0].TotalCount = list.Count();
                            result[0].TotalAmountSum = list.Sum(s => s.TransferAmount);
                        }
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

        public ActionResult DoTransfer()
        {
            string openId = this.GetOpenId(true);

            RDoTransfer result = new RDoTransfer();

            using (AliPayContent db = new AliPayContent())
            {

               var list =  db.DBOrderInfo.Where(s => s.OrderStatus == OrderStatus.Paid
                                             && s.OrderType == OrderType.Normal
                                             && s.AgentOpenId == openId);

                result.MyRemainAmount = list.Sum(s => s.RealTotalAmount);

                list = db.DBOrderInfo.Where(s => s.OrderStatus == OrderStatus.Paid
                                            && s.OrderType == OrderType.Normal
                                            && s.AgentOpenId == openId);


            }
            return View();
        }


        public ActionResult Settlement()
        {
            string accessToken = this.getAccessToken(true);
            IQBCore.IQBPay.Models.Order.EOrderInfo _ppOrder;
            using (AliPayContent db = new AliPayContent())
            {
                _ppOrder = db.DBOrderInfo.FirstOrDefault();
            }
            PPOrderPayNT notice = new PPOrderPayNT(accessToken, "orKUAw16WK0BmflDLiBYsR-Kh5bE", _ppOrder);
            return Content(notice.Push());
           
        }

    }
}