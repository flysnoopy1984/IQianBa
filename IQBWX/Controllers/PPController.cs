using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBWX.BaseEnum;
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
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
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
            ViewBag.ReceiveNo = StringHelper.GenerateReceiveNo();
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

        public ActionResult AgentCommList()
        {
         
            string openId = this.GetOpenId();
            string msg = this.CheckPPUserRole(openId);
            if(msg != "OK")
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.NormalErrorNoButton, ErrorMsg = msg });

            ViewBag.OpenId = openId;
            return View();
        }

        public ActionResult OrderList()
        {
            string openId = this.GetOpenId();
            string msg = this.CheckPPUserRole(openId);
            if (msg != "OK")
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.NormalErrorNoButton, ErrorMsg = msg });
            ViewBag.OpenId = openId;
            return View();
        }

        public ActionResult TransferList()
        {
            string openId = this.GetOpenId();
            string msg = this.CheckPPUserRole(openId);
            if (msg != "OK")
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.NormalErrorNoButton, ErrorMsg = msg });
            ViewBag.OpenId = openId;
            return View();
        }

        [HttpPost]
        public ActionResult AgentCommQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);
            string OpenId = Request["OpenId"];
            ConditionDataType DateType = (ConditionDataType)Enum.Parse(typeof(ConditionDataType), Request["DateType"]);

            List<RAgentCommission> result = new List<RAgentCommission>(); 
            try
            {
                using (AliPayContent db = new AliPayContent())
                {

                    var list = db.DBAgentCommission.Where(o => o.ParentOpenId== OpenId).Select(s => new RAgentCommission
                    {
                        ID = s.ID,
                        ChildName = s.ChildName,
                        CommissionAmount = s.CommissionAmount,
                        OrderNo = s.OrderNo,
                        TransDate = s.TransDate,
                        AgentCommissionStatus = s.AgentCommissionStatus,
                        CommissionRate = s.CommissionRate,
                        TransDateStr = s.TransDateStr,

                    });

                   

                    if (DateType != ConditionDataType.All)
                    {
                        DateTime startDate = DateTime.Today;
                        DateTime endDate = DateTime.Today.AddDays(1);

                        if (DateType == ConditionDataType.Today)
                        {
                            startDate = DateTime.Today;
                            endDate = DateTime.Today.AddDays(1);
                           
                        }
                        else if (DateType == ConditionDataType.Week)
                        {
                            startDate = UtilityHelper.GetTimeStartByType("Week", DateTime.Now);
                            endDate = UtilityHelper.GetTimeEndByType("Week", DateTime.Now);
                            

                        }
                        else if (DateType == ConditionDataType.Month)
                        {
                            startDate = UtilityHelper.GetTimeStartByType("Month", DateTime.Now);
                            endDate = UtilityHelper.GetTimeEndByType("Month", DateTime.Now);
                           
                        }
                        list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);
                    }

                    list = list.OrderByDescending(i => i.TransDate);

                    if (pageIndex == 0)
                    {

                        result = list.Take(pageSize).ToList();
                        if (result.Count > 0)
                        {
                          
                            result[0].TotalCommAmount = list.ToList().Sum(s => s.CommissionAmount);
                        }
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

        [HttpPost]
        public ActionResult OrderQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);
            string OpenId = Request["OpenId"];
            ConditionDataType DateType =(ConditionDataType)Enum.Parse(typeof(ConditionDataType),Request["DateType"]);
            OrderStatus OrderStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), Request["OrderStatus"]);
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

                    if (OrderStatus != OrderStatus.ALL)
                    {
                        list = list.Where(o => o.OrderStatus == OrderStatus);
                    }

                    if (DateType != ConditionDataType.All)
                    {
                        DateTime startDate = DateTime.Today;
                        DateTime endDate = DateTime.Today.AddDays(1);

                        if (DateType == ConditionDataType.Today)
                        {
                            startDate = DateTime.Today;
                            endDate = DateTime.Today.AddDays(1);

                        }
                        else if (DateType == ConditionDataType.Week)
                        {
                            startDate = UtilityHelper.GetTimeStartByType("Week", DateTime.Now);
                            endDate = UtilityHelper.GetTimeEndByType("Week", DateTime.Now);


                        }
                        else if (DateType == ConditionDataType.Month)
                        {
                            startDate = UtilityHelper.GetTimeStartByType("Month", DateTime.Now);
                            endDate = UtilityHelper.GetTimeEndByType("Month", DateTime.Now);

                        }
                        list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);
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
                            result[0].RealTotalAmountSum = list.ToList().Sum(o => o.RealTotalAmount);
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
                        DateTime startDate = DateTime.Today;
                        DateTime endDate = DateTime.Today.AddDays(1);

                        if (DateType == ConditionDataType.Today)
                        {
                            startDate = DateTime.Today;
                            endDate = DateTime.Today.AddDays(1);

                        }
                        else if (DateType == ConditionDataType.Week)
                        {
                            startDate = UtilityHelper.GetTimeStartByType("Week", DateTime.Now);
                            endDate = UtilityHelper.GetTimeEndByType("Week", DateTime.Now);


                        }
                        else if (DateType == ConditionDataType.Month)
                        {
                            startDate = UtilityHelper.GetTimeStartByType("Month", DateTime.Now);
                            endDate = UtilityHelper.GetTimeEndByType("Month", DateTime.Now);

                        }
                        list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);
                    }

                    list = list.OrderByDescending(i => i.TransDate);

                    if (pageIndex == 0)
                    {

                        result = list.Take(pageSize).ToList();
                        if (result.Count > 0)
                        {
                            result[0].TotalCount = list.Count();
                            result[0].TotalAmountSum = list.ToList().Sum(s => s.TransferAmount);
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
            string openId = this.GetOpenId();
            string msg = this.CheckPPUserRole(openId);
            if (msg != "OK")
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.NormalErrorNoButton, ErrorMsg = msg });

            RDoTransfer result = new RDoTransfer();

            using (AliPayContent db = new AliPayContent())
            {

               var order =  db.DBOrderInfo.Where(s => s.OrderStatus == OrderStatus.Paid
                                             && s.OrderType == OrderType.Normal
                                             && s.AgentOpenId == openId);

                result.MyOrderTotalAmount = order.ToList().Sum(s => s.RealTotalAmount);
               

                var agentcomm = db.DBAgentCommission.Where(s => s.AgentCommissionStatus == AgentCommissionStatus.Paid
                                            && s.ParentOpenId == openId);

                result.MyAgentOrderTotalAmount = agentcomm.ToList().Sum(s =>s.CommissionAmount);

                result.MyRemainAmount = result.MyOrderTotalAmount + result.MyAgentOrderTotalAmount;

                var ui = db.DBUserInfo.Where(u => u.OpenId == openId).Select(a => new RUserInfo()
                {
                    AliPayAccount = a.AliPayAccount,
                }).FirstOrDefault();

                result.AliPayAccount = ui.AliPayAccount;
                result.OpenId = openId;

            }
            return View(result);
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