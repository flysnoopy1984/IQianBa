﻿using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.AccountPayment;
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
    public class TransferController : BaseController
    {
        // GET: Transfer
        public ActionResult Index()
        {
            return View();
        }
        [IQBPayAuthorize]
        public ActionResult Info_Win()
        {
            return View();
        }
        [IQBPayAuthorize_Admin]
        public ActionResult List()
        {
            return View();
        }
        [IQBPayAuthorize_Admin]
        public ActionResult DoTransfer()
        {
            return View();
        }

        [IQBPayAuthorize]
        public ActionResult MyList()
        {
            ViewBag.OpenId = this.GetUserSession().OpenId;
            return View();
        }

        [HttpPost]
        public ActionResult Query(InTransfer parameter)
        {
            List<ETransferAmount> result = new List<ETransferAmount>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    IQueryable<ETransferAmount> list = null;

                    if (!string.IsNullOrEmpty(parameter.AgentName))
                    {
                        list = db.DBTransferAmount.Where(o => o.AgentName == parameter.AgentName);
                    }

                    if (!string.IsNullOrEmpty(parameter.AgentOpenId))
                    {
                        if (list == null)
                            list = db.DBTransferAmount.Where(o => o.AgentOpenId == parameter.AgentOpenId);
                        else
                            list = list.Where(o => o.AgentOpenId == parameter.AgentOpenId);
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
                      
                        if (list == null)
                            list = db.DBTransferAmount.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);
                        else
                            list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);
                    }

                    if (list == null)
                        list = db.DBTransferAmount;
                    list = list.OrderByDescending(o => o.TransDate);
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
                Log.log("Transfer Query Error:" + ex.Message);
                throw ex;
            }
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="type">1 Order看Transfer, 2 Transfer</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InfoWin(string Id,string type)
        {
         
           
            EOrderInfo order;
            ETransferAmount transfer;
            string openId = null;
            if (this.GetUserSession().UserRole != UserRole.Administrator)
            {
                openId = this.GetUserSession().OpenId;
            }
         
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
                            var list = db.DBTransferAmount.Where(t => t.OrderNo == order.OrderNo);
                            if(!string.IsNullOrEmpty(openId))
                            {
                                list = list.Where(t => t.AgentOpenId == openId);
                            }
                            result.TransferList = list.ToList();
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
                            result.OrderList = db.DBOrder.Where(t => t.OrderNo == transfer.OrderNo).ToList();
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