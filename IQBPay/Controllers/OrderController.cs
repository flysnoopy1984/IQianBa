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

        public ActionResult List()
        {
            return View();
              
        }

        public ActionResult Info_Win()
        {
            return View();
        }

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
            string sqlFormat = @"select AgentName,AgentOpenId,SUM(RateAmount) as RemainAmount from orderinfo 
                            where OrderStatus = 1 and OrderType=0 and AgentName='{0}'
                            GROUP BY AgentName";
            string sql;

            List<RUser_OrderSum> result = new List<RUser_OrderSum>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    sql = string.Format(sqlFormat, parameter.AgentName);
                    if (parameter.AgentName == "*")
                    {
                        sql = @"select AgentName,AgentOpenId,SUM(RateAmount) as RemainAmount from orderinfo 
                            where OrderStatus = 1 and OrderType=0
                            GROUP BY AgentName";
                    }
                    var list = db.Database.SqlQuery<RUser_OrderSum>(sql).ToList();

                    int totalCount = list.Count();
                    if (parameter.PageIndex == 0)
                    {
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
                return Content(ex.Message);
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
            List<EOrderInfo> result = new List<EOrderInfo>();
            int storeId = -1;
            try
            {

               
                using (AliPayContent db = new AliPayContent())
                {

                    var list = db.DBOrder.Where(o=>o.OrderType == parameter.OrderType);

                    if (!string.IsNullOrEmpty(parameter.StoreId))
                    {
                        if(parameter.StoreId !="-1")
                        {
                            storeId = Convert.ToInt32(parameter.StoreId);
                            list = list.Where(o => o.SellerStoreId == storeId);
                        }
                    }

                    if (!string.IsNullOrEmpty(parameter.AgentOpenId))
                    {
                        list = list.Where(o => o.AgentOpenId == parameter.AgentOpenId);
                    }
                    if (!string.IsNullOrEmpty(parameter.AgentName))
                    { 
                        list=list.Where(o => o.AgentName == parameter.AgentName);
                    }

                    if(parameter.OrderStatus != OrderStatus.ALL)
                    {
                        list=list.Where(o => o.OrderStatus == parameter.OrderStatus);
                    }

                    if (parameter.DataType == ConditionDataType.Today)
                    {
                        list = list.Where(o => o.TransDate == DateTime.Today);
                    }
                    else if(parameter.DataType == ConditionDataType.Week)
                    {
                        DateTime startDate = UtilityHelper.GetTimeStartByType("Week", DateTime.Now);
                        DateTime endDate = UtilityHelper.GetTimeEndByType("Week", DateTime.Now);
                        list = list.Where(o => o.TransDate >= startDate && o.TransDate<= endDate);

                    }
                    else if (parameter.DataType == ConditionDataType.Month)
                    {
                        DateTime startDate = UtilityHelper.GetTimeStartByType("Month", DateTime.Now);
                        DateTime endDate = UtilityHelper.GetTimeEndByType("Month", DateTime.Now);
                        list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);
                    }

                    list = list.OrderByDescending(i => i.TransDate);
                    int totalCount = list.Count();
                    if (parameter.PageIndex == 0)
                    {
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
                    int totalCount = list.Count();
                    if (parameter.PageIndex == 0)
                    {
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