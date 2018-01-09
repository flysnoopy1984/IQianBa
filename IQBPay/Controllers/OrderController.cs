using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.Result;
using IQBPay.Core;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class OrderController : BaseController
    {
        public string OpenId { get; set; }
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [IQBPayAuthorize_Admin]
        public ActionResult List()
        {
            return View();
              
        }

        [IQBPayAuthorize]
        public ActionResult Info_Win()
        {
            return View();
        }

        [IQBPayAuthorize]
        public ActionResult Info_DoTransferOrder()
        {
            return View();
        }

        [IQBPayAuthorize]
        public ActionResult MyList()
        {
            ViewBag.OpenId = this.GetUserSession().OpenId;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuerySum(InOrderSum parameter)
        {
            string sqlFormat = @"select AgentName,AgentOpenId,SUM(RealTotalAmount) as RemainAmount,ui.AliPayAccount from orderinfo 
                                join userinfo as ui on ui.OpenId = orderinfo.AgentOpenId
                                where OrderStatus = 1 and OrderType=0 and AgentName='{0}'
                                GROUP BY AgentName
";
            string sql;

            List<RUser_OrderSum> result = new List<RUser_OrderSum>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    
                    if (string.IsNullOrEmpty(parameter.AgentName))
                    {
                        sql = @"select AgentName,AgentOpenId,SUM(RealTotalAmount) as RemainAmount,ui.AliPayAccount from orderinfo 
                                join userinfo as ui on ui.OpenId = orderinfo.AgentOpenId
                                where OrderStatus = 1 and OrderType=0 
                                GROUP BY AgentOpenId
                                ";
                    }
                    else
                        sql = string.Format(sqlFormat, parameter.AgentName);

                    List<RUser_OrderSum> list = db.Database.SqlQuery<RUser_OrderSum>(sql).ToList();
                    foreach(RUser_OrderSum obj in list)
                    {
                        var comm = db.DBAgentCommission.Where(s => s.AgentCommissionStatus == AgentCommissionStatus.Paid && s.ParentOpenId == obj.AgentOpenId)
                                    .Select(a => new RAgentCommission()
                                    {
                                        CommissionAmount = a.CommissionAmount,
                                    }).ToList().Sum(a => a.CommissionAmount);
                        obj.CommissionAmount = comm;
                    }
                    
                    if (parameter.PageIndex == 0)
                    {
                        int totalCount = list.Count();
                        result = list.Take(parameter.PageSize).ToList();

                        if (result.Count > 0)
                            result[0].TotalCount = totalCount;
                    }
                    else
                        result = list.Skip(parameter.PageIndex * parameter.PageSize).Take(parameter.PageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Remark">OrderList.js调用</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Query(InOrder parameter)
        {
            List<ROrderInfo> result = new List<ROrderInfo>();
            int storeId = -1;
            try
            {

                using (AliPayContent db = new AliPayContent())
                {

                    var list = db.DBOrder.Where(o => o.OrderType == parameter.OrderType).Select(o => new ROrderInfo {

                        ID = o.ID,
                        OrderNo = o.OrderNo,
                        TransDateStr = o.TransDateStr,
                        OrderStatus = o.OrderStatus,
                        TotalAmount = o.TotalAmount,
                        RateAmount = o.RateAmount,
                        ParentCommissionAmount = o.ParentCommissionAmount,
                        L3CommissionAmount = o.L3CommissionAmount,
                        SellerCommission = o.SellerCommission,
                        BuyerTransferAmount = o.BuyerTransferAmount,
                        AgentName= o.AgentName,
                        Rate = o.Rate,
                        SellerName = o.SellerName,
                        SellerChannel = o.SellerChannel,
                        AliPayOrderNo = o.AliPayOrderNo,
                        BuyerAliPayLoginId = o.BuyerAliPayLoginId,
                        TransDate = o.TransDate,
                        AgentOpenId = o.AgentOpenId,
                        LogRemark = o.LogRemark,
                        
                    });

                    if (!string.IsNullOrEmpty(parameter.StoreId))
                    {
                        if(parameter.StoreId !="-1")
                        {
                            storeId = Convert.ToInt32(parameter.StoreId);
                            list = list.Where(o => o.SellerStoreId == storeId);
                        }
                    }

                    if (!string.IsNullOrEmpty(parameter.OrderNo))
                    {
                        list = list.Where(o => o.OrderNo.Contains(parameter.OrderNo));
                    }

                    if (!string.IsNullOrEmpty(parameter.AliPayOrderNo))
                    {
                        list = list.Where(o => o.OrderNo.Contains(parameter.AliPayOrderNo));
                    }

                    if (!string.IsNullOrEmpty(parameter.AgentOpenId))
                    {
                        list = list.Where(o => o.AgentOpenId == parameter.AgentOpenId);
                    }
                    if (!string.IsNullOrEmpty(parameter.AgentName))
                    { 
                        list=list.Where(o => o.AgentName.Contains(parameter.AgentName));
                    }

                    if(parameter.OrderStatus != OrderStatus.ALL)
                    {
                        list=list.Where(o => o.OrderStatus == parameter.OrderStatus);
                    }
                    else
                    {
                        list = list.Where(o => o.OrderStatus != OrderStatus.WaitingAliPayNotify);
                    }

                    if (parameter.DataType != ConditionDataType.All)
                    {
                        DateTime startDate = DateTime.Today;
                        DateTime endDate = DateTime.Today.AddDays(1);

                        if (parameter.DataType == ConditionDataType.Today)
                        {
                            startDate = DateTime.Today;
                            endDate = DateTime.Today.AddDays(1);
                        }
                        else if (parameter.DataType == ConditionDataType.Week)
                        {
                            startDate = UtilityHelper.GetTimeStartByType("Week", DateTime.Now);
                            endDate = UtilityHelper.GetTimeEndByType("Week", DateTime.Now);
                        }
                        else if (parameter.DataType == ConditionDataType.Month)
                        {
                            startDate = UtilityHelper.GetTimeStartByType("Month", DateTime.Now);
                            endDate = UtilityHelper.GetTimeEndByType("Month", DateTime.Now);

                        }
                        list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);
                    }

                    list = list.OrderByDescending(i => i.TransDate);
                    if (parameter.PageIndex == 0)
                    {
                        int totalCount = list.Count();
                        result = list.Take(parameter.PageSize).ToList();

                        if (result.Count > 0)
                        {
                            result[0].TotalCount = totalCount;
                            result[0].RealTotalAmountSum = list.ToList().Sum(o => o.RateAmount);
                            result[0].ParentAmountSum = list.ToList().Sum(o => o.ParentCommissionAmount);
                            result[0].BuyerTransferSum = list.ToList().Sum(o => o.BuyerTransferAmount);
                            result[0].StoreAmountSum = list.ToList().Sum(o => o.SellerCommission);
                            result[0].TotalAmountSum = list.ToList().Sum(o => o.TotalAmount);
                            result[0].L3AmountSum = list.ToList().Sum(o => o.L3CommissionAmount);

                            float AliPayFee = list.Where(o=>o.SellerCommission==0).ToList().Sum(o => o.TotalAmount)*(float)(0.6/100);
                            result[0].PPIncome = result[0].TotalAmountSum - AliPayFee - result[0].RealTotalAmountSum - result[0].ParentAmountSum - result[0].BuyerTransferSum - result[0].StoreAmountSum - result[0].L3AmountSum;
                        }
                    }
                    else
                        result = list.Skip(parameter.PageIndex * parameter.PageSize).Take(parameter.PageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.log("Order Query Error:" + ex.Message);
                throw ex;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult QueryForDoTransferOrder(InOrder parameter)
        {
            List<RUser_Order> result = new List<RUser_Order>();
            string sqlFormat = @"select OrderNo,TransDateStr,TotalAmount,RealTotalAmount from orderinfo 
                            where OrderStatus = 1 and OrderType=0 and AgentOpenId='{0}'";
                           
            string sql;

            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    sql = string.Format(sqlFormat, parameter.AgentOpenId);

                    var list = db.Database.SqlQuery<RUser_Order>(sql).ToList();
                   
                    if (parameter.PageIndex == 0)
                    {
                        int totalCount = list.Count();
                        result = list.Take(parameter.PageSize).ToList();
                        if (result.Count > 0)
                            result[0].TotalCount = totalCount;
                    }
                    else
                        result = list.Skip(parameter.PageIndex * parameter.PageSize).Take(parameter.PageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.log("QueryForDoTransferOrder Error:" + ex.Message);
                return Content(ex.Message);
            }
            return Json(result);
        }

    }
}