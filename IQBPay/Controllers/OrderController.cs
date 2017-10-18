using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.InParameter;
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
    public class OrderController : BaseController
    {

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
            try
            {
                string openId = this.GetOpenId(true);
               
                using (AliPayContent db = new AliPayContent())
                {
                    var list = db.DBOrder.Where(o => o.OrderStatus == parameter.OrderStatus
                                               && o.OrderType == parameter.OrderType)
                                        .OrderByDescending(i => i.TransDate);
                    if (parameter.OrderStatus == OrderStatus.ALL)
                    {
                        list = db.DBOrder.Where(o=>o.OrderType == parameter.OrderType)
                                        .OrderByDescending(i => i.TransDate);

                    }
                    
                    
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
                return Content(ex.Message);
            }
            return Json(result);
        }

    }
}