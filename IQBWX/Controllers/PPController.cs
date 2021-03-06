﻿using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.User;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBWX.Models.Json.WXMedia.News;
using IQBCore.IQBWX.Models.WX.Template;
using IQBCore.WxSDK;
using IQBWX.DataBase.IQBPay;
using LitJson;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;
using EntityFramework.Extensions;
using IQBCore.Model;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.Sys;
using IQBCore.IQBPay.Models.Page;
using IQBCore.Common.Constant;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBWX.Models.WX.Template.PaySuccessTellAdmin;

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
            try
            { 
                using (DataBase.UserContent db = new DataBase.UserContent())
                {
                    db.Get("aaa");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return View();
        }

        public ActionResult PayFromUser()
        {
            return View();
        }

        public ActionResult PaySelection()
        {
            if (WXBaseController.GlobalConfig.WebStatus == PayWebStatus.Stop)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.SystemMaintain, ErrorMsg = WXBaseController.GlobalConfig.Note });
            }
            using (AliPayContent db = new AliPayContent())
            {
                var sql = "select count(1) from AliPayApplication as a where a.SupportHuaBei = 1";
                int r = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                ViewBag.HasHuaBei = r == 0 ? false : true;
            }

            return View();
        }

        public ActionResult CardPay()
        {
            return View();
        }

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult WXPay(string Id)
        {
          

            string openId = Convert.ToString(Session[IQBConstant.BuyerOpenId]);

            bool isDev = Convert.ToBoolean(ConfigurationManager.AppSettings["DevMode"]);

            //////Jacky
            if (isDev)
            {
                ViewBag.OpenId = "o3nwE0qI_cOkirmh_qbGGG-5G6B0";
                return Pay(Id);
            }
            ////平台
            ////if (isDev) return "o3nwE0jrONff65oS-_W96ErKcaa0";
            //return Pay(Id);

            PPayData pData = null;

            JsApiPay wxAPI = new JsApiPay();

            string code = Request.QueryString["code"];

            if (!string.IsNullOrEmpty(code) && string.IsNullOrEmpty(openId))
            {
                //获取code码，以获取openid和access_token
                //    code = Request.QueryString["code"];
                //  NLogHelper.InfoTxt("[strat GetOpenidAndAccessTokenFromCode] Code:"+code);
                wxAPI.GetOpenidAndAccessTokenFromCode(code);
                //  NLogHelper.InfoTxt("[end GetOpenidAndAccessTokenFromCode]");
                openId = wxAPI.openid;

               // Id = (string)Session[IQBConstant.SK_AgentQRId];
                if(string.IsNullOrEmpty(Id))
                {
                    return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "没有获取代理信息，请联系平台！" });
                }

                ViewBag.OpenId = openId;
                Session[IQBConstant.BuyerOpenId] = openId;
                return Pay(Id);
            }
            else
            {
                if(string.IsNullOrEmpty(openId))
                {
                    try
                    {
                        var redirect_uri = System.Web.HttpUtility.UrlEncode("http://wx.iqianba.cn/PP/WXPay?Id=" + Id, System.Text.Encoding.UTF8);

                        WxPayData data = new WxPayData();
                        data.SetValue("appid", WxPayConfig_YJ.APPID);
                        data.SetValue("redirect_uri", redirect_uri);
                        data.SetValue("response_type", "code");
                        data.SetValue("scope", "snsapi_userinfo");

                        data.SetValue("state", "1" + "#wechat_redirect");
                        string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();



                        return Redirect(url);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(Id))
                    {
                        return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "没有获取代理信息，请联系平台！" });
                    }

                    ViewBag.OpenId = openId;
                    Session[IQBConstant.BuyerOpenId] = openId;
                    return Pay(Id);
                }
               
            }

            return View();

        }
     
          
        

        public ActionResult Pay(string Id)
        {
            
            //if (WXBaseController.GlobalConfig.WebStatus == PayWebStatus.Stop && Id!="118")
            //{
            //    return RedirectToAction("ErrorMessage", "Home",new { code = Errorcode.SystemMaintain, ErrorMsg = WXBaseController.GlobalConfig.Note });
            //}
            PPayData pData = null;
            string sql = string.Format(@"select qr.ID as qrId,ui.Name,qr.Rate,qr.MarketRate,qr.QRType from QRUser as qr 
                        join UserInfo as ui on ui.OpenId =qr.OpenId
                        where qr.ID ='{0}'", Id);
            
            using (AliPayContent db = new AliPayContent())
            {
                pData = db.Database.SqlQuery<PPayData>(sql).FirstOrDefault();
                if (pData == null)
                    pData = new PPayData();

                object ps = Session[IQBConstant.BuyerPhone];
                pData.BuyerPhone = ps == null?"":Convert.ToString(ps);
            }

            return View(pData);
        }

        public ActionResult PayWithAccount(string Id)
        {
            // return RedirectToAction("Pay", "PP", new { Id = Id });

            if (WXBaseController.GlobalConfig.WebStatus == PayWebStatus.Stop)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.SystemMaintain, ErrorMsg = WXBaseController.GlobalConfig.Note });
            }
           

            ViewBag.QRUserId = Id;
            // ViewBag.ReceiveNo = StringHelper.GenerateReceiveNo();
            return View();
        }

        public ActionResult Pay2(string Id)
        {
            if (WXBaseController.GlobalConfig.WebStatus == PayWebStatus.Stop)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.SystemMaintain, ErrorMsg = WXBaseController.GlobalConfig.Note });
            }
            ViewBag.QRUserId = Id;
            // ViewBag.ReceiveNo = StringHelper.GenerateReceiveNo();
            return View();
        }

        public ActionResult Auth_Store(string Rate)
        {
            ViewBag.Rate = Rate;
           
            return View();
        }

        [HttpPost]
        public ActionResult  UpdateAliPayAccount()
        {
            string Id = Request["ID"];
            string AliPayAccount = Request["AliPayAccount"];
            IQBCore.IQBPay.Models.User.EUserInfo ui = new IQBCore.IQBPay.Models.User.EUserInfo();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    ui.Id = Convert.ToInt32(Id);
                    ui.AliPayAccount = AliPayAccount;

                    DbEntityEntry<IQBCore.IQBPay.Models.User.EUserInfo> entry = db.Entry<IQBCore.IQBPay.Models.User.EUserInfo>(ui);
                    entry.State = EntityState.Unchanged;

                    entry.Property(t => t.AliPayAccount).IsModified = true;

                    db.SaveChanges();

                    UserSession.AliPayAccount = AliPayAccount;
                }
            }
            catch(Exception ex)
            {
                Content(ex.Message);
            }
         

           return Content("OK");

        }

        public ActionResult AliPayAccount()
        {
            string openId = this.GetOpenId();
            RUserInfo ui = null;
            if (!string.IsNullOrEmpty(openId))
            {
                using (AliPayContent db = new AliPayContent())
                {
                    ui= db.DBUserInfo.Where(u => u.OpenId == openId).Select(a => new RUserInfo()
                    {
                        AliPayAccount = a.AliPayAccount,
                        Id = a.Id, 
                       
                    }).FirstOrDefault();
                }
            }
            if(ui!=null)
            {
                ViewBag.ID = ui.Id;
                ViewBag.AliPayAccount = ui.AliPayAccount;

                UserSession.AliPayAccount = ui.AliPayAccount;

            }

            return View();
        }

        public ActionResult ReceiveOrder()
        {
            if (WXBaseController.GlobalConfig.WebStatus == PayWebStatus.Stop)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.SystemMaintain, ErrorMsg = WXBaseController.GlobalConfig.Note });
            }
            ViewBag.ppUrl = ConfigurationManager.AppSettings["Site_IQBPay"];

            ViewBag.Phone = Session[IQBConstant.BuyerPhone];
            ViewBag.AliAccount = Session[IQBConstant.BuyerAliAccount];
            return View();
        }

        public ActionResult AgentCommList()
        {

            if (UserSession.UserRole < UserRole.Agent || UserSession.UserStatus == UserStatus.WaitRewiew)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            //if (msg != "OK")
            //    return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.NormalErrorNoButton, ErrorMsg = msg });
            InitProfilePage();
            return View();
        }

        public ActionResult OrderList()
        {
            /*    string openId = this.GetOpenId();
                string msg = this.CheckPPUserRole(openId);
                */
            if (UserSession.UserRole < UserRole.Agent)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            //if (msg != "OK")
            //    return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.NormalErrorNoButton, ErrorMsg = msg });
            InitProfilePage();
           
            return View();
        }

        public ActionResult TransferList()
        {
            if (UserSession.UserRole < UserRole.Agent)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            //if (msg != "OK")
            //    return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.NormalErrorNoButton, ErrorMsg = msg });
            InitProfilePage();
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

                    var list = db.DBAgentCommission.Where(o => o.ParentOpenId== OpenId && o.AgentCommissionStatus == AgentCommissionStatus.Closed).Select(s => new RAgentCommission
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

                            //result[0].TotalCommAmount = list.ToList().Sum(s => s.CommissionAmount);

                            DateTime startDate = DateTime.Today;
                            DateTime endDate = DateTime.Today.AddDays(1);

                            var TodayComm = db.DBAgentCommission.Where(o => o.ParentOpenId == OpenId && o.TransDate >= startDate && o.TransDate <= endDate && o.AgentCommissionStatus == AgentCommissionStatus.Closed).ToList().Sum(o => o.CommissionAmount).ToString("0.00");
                            result[0].TodayCommAmt = TodayComm;

                            result[0].TotalCommAmt = db.DBAgentCommission.Where(o => o.ParentOpenId == OpenId && o.AgentCommissionStatus == AgentCommissionStatus.Closed).ToList().Sum(o => o.CommissionAmount).ToString("0.00");
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
            List<RUser_Order> result = new List<RUser_Order>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    var list = db.DBOrder.Where(o => o.AgentOpenId == OpenId || o.ParentOpenId == OpenId || o.L3OpenId == OpenId).Select(o => new RUser_Order
                    {
                        OrderNo = o.OrderNo,
                        TransDateStr = o.TransDateStr,
                        TotalAmount = o.TotalAmount,
                        AgentAmount = o.RateAmount,
                        ParentCommissionAmount = o.ParentCommissionAmount,
                        L3CommissionAmount = o.L3CommissionAmount,
                        TransDate =o.TransDate,
                        OrderStatus = o.OrderStatus,
                    }).Where(o=>o.OrderStatus == OrderStatus.Closed).OrderByDescending(i => i.TransDate);

                    if (pageIndex == 0)
                    {
                        DateTime startDate = DateTime.Today;
                        DateTime endDate = DateTime.Today.AddDays(1);

                        int totalCount = list.Count();
                        result = list.Take(pageSize).ToList();
                        if (result.Count > 0)
                        {
                            var PList = db.DBAgentCommission.Where(o => o.ParentOpenId == OpenId && o.AgentCommissionStatus == AgentCommissionStatus.Closed && o.TransDate >= startDate && o.TransDate <= endDate).Select(o => new RAgentCommission
                            {
                                CommissionAmount = o.CommissionAmount,
                            });

                            var AllPLList = db.DBAgentCommission.Where(o => o.ParentOpenId == OpenId && o.AgentCommissionStatus == AgentCommissionStatus.Closed).Select(o => new RAgentCommission
                            {
                                CommissionAmount = o.CommissionAmount,
                            });

                            var TodayOrder = db.DBOrder.Where(o => o.AgentOpenId == OpenId && o.OrderStatus == OrderStatus.Closed && o.TransDate >= startDate && o.TransDate <= endDate);
                            var myToday = TodayOrder.ToList();
                            //   result[0].AgentTodayOrderAmount = myToday.Sum(o => o.TotalAmount).ToString("0.00");
                            result[0].AgentTodayOrderAmount = myToday.Count.ToString();
                            result[0].AgentTodayIncome = (myToday.Sum(o => o.RateAmount) + PList.ToList().Sum(o => o.CommissionAmount)).ToString("0.00");

                            var allOrder = db.DBOrder.Where(o => o.AgentOpenId == OpenId && o.OrderStatus == OrderStatus.Closed);
                            result[0].AgentTotalIncome = (allOrder.ToList().Sum(o => o.RateAmount) + AllPLList.ToList().Sum(o => o.CommissionAmount)).ToString("0.00");

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

        //[HttpPost]
        //public ActionResult OrderQuery()
        //{
        //    int pageIndex = Convert.ToInt32(Request["Page"]);
        //    int pageSize = Convert.ToInt32(Request["PageSize"]);
        //    string OpenId = Request["OpenId"];
        //    ConditionDataType DateType =(ConditionDataType)Enum.Parse(typeof(ConditionDataType),Request["DateType"]);
        //    OrderStatus OrderStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), Request["OrderStatus"]);
        //    List<ROrderInfo> result = new List<ROrderInfo>();
        //    try
        //    {

        //        using (AliPayContent db = new AliPayContent())
        //        {
        //            var list = db.DBOrder.Where(o => o.AgentOpenId == OpenId).Select(o=>new ROrderInfo {
        //                ID = o.ID,
        //                OrderNo = o.OrderNo,
        //                TransDateStr = o.TransDateStr,
        //                OrderStatus = o.OrderStatus,
        //                TotalAmount = o.TotalAmount,
        //                RateAmount = o.RateAmount,
        //                BuyerAliPayLoginId = o.BuyerAliPayLoginId,
        //                TransDate = o.TransDate,
        //            });



        //            if (OrderStatus != OrderStatus.ALL)
        //            {
        //                list = list.Where(o => o.OrderStatus == OrderStatus);
        //            }
        //            else
        //                list = list.Where(o => o.OrderStatus != OrderStatus.WaitingAliPayNotify && o.OrderStatus!=OrderStatus.SystemClose);

        //            if (DateType != ConditionDataType.All)
        //            {
        //                DateTime startDate = DateTime.Today;
        //                DateTime endDate = DateTime.Today.AddDays(1);

        //                if (DateType == ConditionDataType.Today)
        //                {
        //                    startDate = DateTime.Today;
        //                    endDate = DateTime.Today.AddDays(1);

        //                }
        //                else if (DateType == ConditionDataType.Week)
        //                {
        //                    startDate = UtilityHelper.GetTimeStartByType("Week", DateTime.Now);
        //                    endDate = UtilityHelper.GetTimeEndByType("Week", DateTime.Now);
        //                }
        //                else if (DateType == ConditionDataType.Month)
        //                {
        //                    startDate = UtilityHelper.GetTimeStartByType("Month", DateTime.Now);
        //                    endDate = UtilityHelper.GetTimeEndByType("Month", DateTime.Now);

        //                }
        //                list = list.Where(o => o.TransDate >= startDate && o.TransDate <= endDate);
        //            }
        //            //list = list.Where(o => o.OrderStatus == OrderStatus.Paid);
        //            list = list.OrderByDescending(i => i.TransDate);

        //            if (pageIndex == 0)
        //            {
        //                DateTime startDate = DateTime.Today;
        //                DateTime endDate = DateTime.Today.AddDays(1);

        //                int totalCount = list.Count();
        //                result = list.Take(pageSize).ToList();
        //                if (result.Count > 0)
        //                {
        //                    var PList = db.DBAgentCommission.Where(o => o.ParentOpenId == OpenId && o.AgentCommissionStatus == AgentCommissionStatus.Closed && o.TransDate >= startDate && o.TransDate <= endDate).Select(o => new RAgentCommission
        //                    {
        //                        CommissionAmount = o.CommissionAmount,
        //                    });

        //                    var AllPLList = db.DBAgentCommission.Where(o => o.ParentOpenId == OpenId && o.AgentCommissionStatus == AgentCommissionStatus.Closed).Select(o => new RAgentCommission
        //                    {
        //                        CommissionAmount = o.CommissionAmount,
        //                    });

        //                    var TodayOrder = db.DBOrder.Where(o => o.AgentOpenId == OpenId && o.OrderStatus == OrderStatus.Closed && o.TransDate >= startDate && o.TransDate <= endDate);
        //                    var myToday = TodayOrder.ToList();
        //                    //   result[0].AgentTodayOrderAmount = myToday.Sum(o => o.TotalAmount).ToString("0.00");
        //                    result[0].AgentTodayOrderAmount = myToday.Count.ToString();
        //                    result[0].AgentTodayIncome = (myToday.Sum(o => o.RateAmount) + PList.ToList().Sum(o => o.CommissionAmount)).ToString("0.00");

        //                    var allOrder = db.DBOrder.Where(o => o.AgentOpenId == OpenId && o.OrderStatus == OrderStatus.Closed);
        //                    result[0].AgentTotalIncome = (allOrder.ToList().Sum(o => o.RateAmount) + AllPLList.ToList().Sum(o => o.CommissionAmount)).ToString("0.00");

        //                }

        //            }
        //            else
        //                result = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(result);

        //}

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

                    IQBCore.IQBPay.Models.User.EUserInfo ui = db.DBUserInfo.Where(u => u.OpenId == OpenId).FirstOrDefault();
                    var list = db.DBTransferAmount.Where(o => o.AgentOpenId == OpenId && o.TransferStatus == TransferStatus.Success).Select(s => new RTransferAmount
                    {
                        ID = s.ID,
                        TransferId = s.TransferId,
                        TransDateStr = s.TransDateStr,
                        TransferAmount = s.TransferAmount,
                        TargetAccount = s.TargetAccount,
                        TransDate = s.TransDate,
                        AliPayAccount = ui.AliPayAccount,
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
                        list = list.Where(o => o.TransDate >= startDate && o.TransDate < endDate);
                    }

                    list = list.OrderByDescending(i => i.TransDate);

                    if (pageIndex == 0)
                    {
                       
                        result = list.Take(pageSize).ToList();
                        if (result.Count > 0)
                        {
                          //  result[0].TotalCount = list.Count();
                           
                        }
                        else
                        {
                            result.Add(new RTransferAmount());
                        }

                        DateTime startDate = DateTime.Today;
                        DateTime endDate = DateTime.Today.AddDays(1);

                        var TodayTransfer = db.DBTransferAmount.Where(o => o.AgentOpenId == OpenId && o.TransferStatus == TransferStatus.Success && o.TransDate >= startDate && o.TransDate < endDate);
                        result[0].TodayTransferAmt = TodayTransfer.ToList().Sum(s => s.TransferAmount).ToString("0.00");

                        result[0].TotalTransferAmt = db.DBTransferAmount.Where(o => o.AgentOpenId == OpenId && o.TransferStatus == TransferStatus.Success).ToList().Sum(s => s.TransferAmount).ToString("0.00");

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

            if (WXBaseController.GlobalConfig.WebStatus == PayWebStatus.Stop)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.SystemMaintain, ErrorMsg = WXBaseController.GlobalConfig.Note });
            }

            RDoTransfer result = new RDoTransfer();

            //using (AliPayContent db = new AliPayContent())
            //{

            //    var order = db.DBOrder.Where(s => s.OrderStatus == OrderStatus.Paid
            //                                 && s.OrderType == OrderType.Normal
            //                                 && s.AgentOpenId == openId);

            //    result.MyOrderTotalAmount = order.ToList().Sum(s => s.RateAmount);


            //    var agentcomm = db.DBAgentCommission.Where(s => s.AgentCommissionStatus == AgentCommissionStatus.Paid
            //                                && s.ParentOpenId == openId);

            //    result.MyAgentOrderTotalAmount = agentcomm.ToList().Sum(s => s.CommissionAmount);

            //    result.MyRemainAmount = result.MyOrderTotalAmount + result.MyAgentOrderTotalAmount;

            //    var ui = db.DBUserInfo.Where(u => u.OpenId == openId).Select(a => new RUserInfo()
            //    {
            //        AliPayAccount = a.AliPayAccount,
            //    }).FirstOrDefault();

            //    result.AliPayAccount = ui.AliPayAccount;
            //    result.OpenId = openId;

            //}
            return View(result);
        }

        /// <summary>
        /// 没有收款码了
        /// </summary>
        /// <returns></returns>
        [HttpPost] 
        public ActionResult OrderReceive()
        {
            string receiveNo = this.Request["ReceiveNo"];
            List<ROrder_Receive> list = new List<ROrder_Receive>();

            //using (AliPayContent db = new AliPayContent())
            //{
            //    list = db.DBOrder.Where(o => o.ReceiveNo == receiveNo && o.OrderStatus == OrderStatus.WaitingBuyerConfirm).Select(a => new ROrder_Receive
            //    {
            //        OrderStatus = a.OrderStatus,
            //        Amount = a.TotalAmount,
            //        OrderNo = a.OrderNo,
            //        TransDateStr = a.TransDateStr
            //    }).ToList();
            //}
            return Json(list);
        }

        [HttpPost]
        public ActionResult ConfirmRO()
        {
            ROrder_Receive result = new ROrder_Receive();
          
            string OrderNo = this.Request["OrderNo"];
            if(string.IsNullOrEmpty(OrderNo))
            {
                result.RunResult = "订单参数未获取，请联系平台服务商";
                return Json(result);
            }
            #region Old Method
            //

            //string sql = "update orderinfo set OrderStatus = {0} where OrderNo=@OrderNo";
            //sql = string.Format(sql, Convert.ToInt32(OrderStatus.Paid));

            //var p_OrderNo = new SqlParameter("@OrderNo", OrderNo);

            //try
            //{
            //    using (AliPayContent db = new AliPayContent())
            //    {
            //        int r = db.Database.ExecuteSqlCommand(sql, p_OrderNo);
            //        if (r > 0)
            //        {
            //            result.RunResult = "OK";
            //        }
            //        else
            //            result.RunResult = "更新错误，请联系代理!";

            //    }
            //}
            //catch(Exception ex)
            //{
            //    result.RunResult = "更新错误，请联系代理!";
            //}
            #endregion

            try
            {
                string sql = "update orderinfo set OrderStatus = {0} where OrderNo=@OrderNo";
                sql = string.Format(sql, Convert.ToInt32(OrderStatus.Paid));

                var p_OrderNo = new SqlParameter("@OrderNo", OrderNo);

                using (AliPayContent db = new AliPayContent())
                {
                    result = db.DBOrder.Where(o => o.OrderNo == OrderNo).Select(o => new ROrder_Receive
                    {
                        OrderStatus = o.OrderStatus,

                    }).FirstOrDefault();
                    if (result == null)
                    {
                        result.RunResult = "订单未获取，请联系平台服务商";
                        return Json(result);
                    }
                    if (result.OrderStatus == OrderStatus.WaitingBuyerConfirm)
                    {
                        try
                        {
                            int r = db.Database.ExecuteSqlCommand(sql, p_OrderNo);
                            if (r > 0)
                            {
                                result.RunResult = "OK";
                            }
                            else
                                result.RunResult = "更新错误，请联系代理!";
                        }
                        catch (Exception ex)
                        {
                            result.RunResult = "更新错误，请联系代理!" + ex.Message;
                        }

                    }
                    else
                    {
                        result.RunResult = "已收货确认";
                    }
                }
            }
            catch (Exception ex)
            {
                result.RunResult = "更新错误，请联系代理!";
            }
            return Json(result);
        }


        public ActionResult Settlement()
        {
            string accessToken = this.getAccessToken(true);
            IQBCore.IQBPay.Models.Order.EOrderInfo _ppOrder;
            using (AliPayContent db = new AliPayContent())
            {
                _ppOrder = db.DBOrder.FirstOrDefault();
            }
            PPOrderPayNT notice = new PPOrderPayNT(accessToken, "orKUAw16WK0BmflDLiBYsR-Kh5bE", _ppOrder);
            return Content(notice.Push());

        }

        #region Pay
        [HttpPost]
        public ActionResult CheckPhoneIsExist(string phone)
        {
            EBuyerInfo buyer = null;
            using (AliPayContent db = new AliPayContent())
            {
                buyer = db.DBBuyerInfo.Where(b => b.PhoneNumber == phone).FirstOrDefault();
                if(buyer == null)
                {
                    buyer = new EBuyerInfo();
                    buyer.HasPhone = false;
                }
                else
                {
                    buyer.HasPhone = true;
                }
            }
            if (buyer == null)
                buyer = new EBuyerInfo();
            return Json(buyer);
        }
        #endregion

        #region 代理
        [HttpPost]
        public ActionResult GetJSSDK(string AuthUrl)
        {
            string AccessToken = this.getAccessToken(true);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", AccessToken);

            string result = HttpService.Get(url);
            //请求url以获取数据
            JsonData jd = JsonMapper.ToObject(result);

            string ticket = (string)jd["ticket"];

            WXSign wxSign = new WXSign();
            wxSign.timestamp = WxPayApi.GenerateTimeStamp();
            wxSign.AppId = WxPayConfig.APPID;
            wxSign.nonceStr = WxPayApi.GenerateNonceStr();
            wxSign.jsapi_ticket = ticket;

            string sign = "jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}";

            sign = string.Format(sign, ticket, wxSign.nonceStr, wxSign.timestamp, AuthUrl);

            wxSign.signature = UtilityHelper.DoSHA1(sign, Encoding.Default).ToLower();

            return Json(wxSign);


        }

        public ActionResult UserVerification()
        {
            return View();
        }

      

       [HttpPost]
        public ActionResult Agent_QR_AR_Add(EQRUser qrUser)
        {
            
            return View();
        }

        public ActionResult Agent_QR_ARList()
        {
            if (UserSession.UserStatus == UserStatus.WaitRewiew)
            {
                UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "等待您上级的审核" });
            }

            if (UserSession.UserRole < UserRole.Agent)
            {
                if (UserSession.UserStatus == UserStatus.WaitRewiew) UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            InitProfilePage();

            return View();
        }

        [HttpPost]
        public ActionResult GetQRUser(QRReceiveType qrType)
        {
            EQRUser qrUser = null;
            using (AliPayContent db = new AliPayContent())
            {
                qrUser = db.DBQRUser.Where(u => u.OpenId == UserSession.OpenId && u.RecordStatus == RecordStatus.Normal && u.QRType == qrType).FirstOrDefault();
            }
            if (qrUser == null)
                qrUser = new EQRUser();
            return Json(qrUser);
        }

        public ActionResult Agent_QR_ARListQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);

            using (AliPayContent db = new AliPayContent())
            {

                var list = db.DBQRUser
                             .Where(u=>u.OpenId == UserSession.OpenId && 
                                    u.RecordStatus == RecordStatus.Normal && 
                                    (u.QRType == QRReceiveType.Small || 
                                    u.QRType == QRReceiveType.Huge || 
                                    u.QRType == QRReceiveType.CreditCard))
                             .Select(s => new RQRUser()
                {
                    QRId = s.QRId,
                    Rate = s.Rate,
                    MarketRate = s.MarketRate,
                    ParentOpenId = s.ParentOpenId,
                    ParentName = s.ParentName,
                    ParentCommissionRate = s.ParentCommissionRate,
                    ReceiveStoreId = s.ReceiveStoreId,
                    OrigQRFilePath = s.OrigQRFilePath,
                    IsCurrent = s.IsCurrent,
                    ID = s.ID,
                    QRType = s.QRType,
                });

               
                list = list.OrderBy(o => o.QRType);

                List<RQRUser> result = new List<RQRUser>();

                if (pageIndex == 0)
                    result = list.Take(pageSize).ToList();
                else
                    result = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                return Json(result);
            }  
        }

        [HttpPost]
        public ActionResult Agent_QR_ARSave(EQRUser qrUser)
        {

            OutAPIResult result = new OutAPIResult();
            result.IsSuccess = true;

            using (AliPayContent db = new AliPayContent())
            {
                if (qrUser.ID > 0)
                {
                    try
                    {
                        if(qrUser.Rate<=0)
                        {
                            result.IsSuccess = false;
                            result.ErrorMsg = "用户手续费设置过低，费率不能小于0";
                            return Json(result);
                        }

                        DbEntityEntry<EQRUser> entry = db.Entry<EQRUser>(qrUser);
                        entry.State = EntityState.Unchanged;

                        entry.Property(t => t.Rate).IsModified = true;
                        entry.Property(t => t.MarketRate).IsModified = true;


                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = ex.Message;
                    }

                }
                else
                {
                    //    try
                    //    {
                    //        int n = db.DBQRUser.Where(o => o.OpenId == UserSession.OpenId && o.MarketRate == qrUser.MarketRate).Count();
                    //        if (n > 0)
                    //            return  ErrorResult(string.Format("已经存在用户手续费为{0}的二维码", qrUser.MarketRate));


                    //        EQRUser curQRUser = db.DBQRUser.Where(o => o.OpenId == UserSession.OpenId && o.IsCurrent == true).FirstOrDefault();
                    //        if(curQRUser==null)
                    //            return ErrorResult("没有找到当前的收款二维码，请联系管理员");   

                    //        qrUser.Rate = qrUser.MarketRate - (curQRUser.MarketRate-curQRUser.Rate);
                    //        qrUser.ReceiveStoreId = curQRUser.ReceiveStoreId;
                    //        qrUser.ParentOpenId = curQRUser.ParentOpenId;
                    //        qrUser.ParentCommissionRate = curQRUser.ParentCommissionRate;
                    //        qrUser.ParentName = curQRUser.ParentName;
                    //        qrUser.QRId = curQRUser.QRId;
                    //        qrUser.OpenId = UserSession.OpenId;
                    //        qrUser.UserName = UserSession.Name;


                    //        if (qrUser.IsCurrent)
                    //        {
                    //            var p_OpenId = new SqlParameter("@OpenId", UserSession.OpenId);

                    //            string sql = @"update [QRUser]
                    //                           set [IsCurrent] = 'false'
                    //                           where OpenId =@OpenId";

                    //            db.Database.ExecuteSqlCommand(sql, p_OpenId);
                    //        }

                    //        db.DBQRUser.Add(qrUser);
                    //        db.SaveChanges();


                    //        string url = ConfigurationManager.AppSettings["Site_IQBPay"] + "api/userapi/CreateAgentQR_AR";

                    //        string data = string.Format("ID={0}&OpenId={1}", qrUser.ID,UserSession.OpenId);
                    //        try
                    //        {
                    //            string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
                    //            result = JsonConvert.DeserializeObject<OutAPIResult>(res);
                    //        }
                    //        catch(Exception ex)
                    //        {
                    //            result.ErrorMsg = ex.Message;
                    //            result.IsSuccess = false;
                    //        }
                    //        finally
                    //        {

                    //            if (!result.IsSuccess)
                    //            {
                    //                var p_ID = new SqlParameter("@ID", qrUser.ID);

                    //                string sql = "delete from QRUser where ID=@ID";
                    //                db.Database.ExecuteSqlCommand(sql, p_ID);

                    //                DbEntityEntry<EQRUser> entry = db.Entry<EQRUser>(curQRUser);
                    //                entry.State = EntityState.Unchanged;
                    //                curQRUser.IsCurrent = true;
                    //                entry.Property(t => t.IsCurrent).IsModified = true;
                    //                db.SaveChanges();

                    //            }
                    //        }


                    //    }
                    //    catch(Exception ex)
                    //    {
                    //        result.IsSuccess = false;
                    //        result.ErrorMsg = ex.Message;
                    //    }
                    result.IsSuccess = false;
                    result.ErrorMsg = "没有获取费率信息，请联系平台管理员";
                }

            }

            return Json(result);
        }

        public ActionResult Agent_QR_ARDelete(EQRUser qrUser)
        {
            OutAPIResult result = new OutAPIResult();
            
            try
            {
                if(qrUser.IsCurrent)
                {
                    return base.ErrorResult("当前收款账户不能删除");
                }
                using (AliPayContent db = new AliPayContent())
                {
                    db.Entry<EQRUser>(qrUser).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                return base.ErrorResult(ex.Message);
            }
            return Json(result);

        }

        [HttpPost]
        public ActionResult InviteCodeUpdate(EQRInfo qrInfo)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    qrInfo.InitModify();
                    qrInfo.RecordStatus = RecordStatus.Normal;
                    DbEntityEntry<EQRInfo> entry = db.Entry<EQRInfo>(qrInfo);
                    entry.State = EntityState.Unchanged;

                    entry.Property(t => t.RecordStatus).IsModified = true;
                    entry.Property(t => t.ParentOpenId).IsModified = true;
                    //entry.Property(t => t.ParentCommissionRate).IsModified = true;
                    //entry.Property(t => t.Rate).IsModified = true;
                    entry.Property(t => t.ReceiveStoreId).IsModified = true;

                    entry.Property(t => t.MDate).IsModified = true;
                    entry.Property(t => t.MTime).IsModified = true;
                    entry.Property(t => t.ModifyDate).IsModified = true;
                    db.SaveChanges();
                }
                    

              
            }
            catch(Exception ex)
            {
                return base.ErrorResult(ex.Message);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult InviteCodeUpdate_Status(EQRInfo qrInfo)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                
                using (AliPayContent db = new AliPayContent())
                {
                    qrInfo.RecordStatus = RecordStatus.Normal;
                    qrInfo.InitModify();
                    DbEntityEntry<EQRInfo> entry = db.Entry<EQRInfo>(qrInfo);
                    entry.State = EntityState.Unchanged;
                    entry.Property(t => t.RecordStatus).IsModified = true;

                    entry.Property(t => t.MDate).IsModified = true;
                    entry.Property(t => t.MTime).IsModified = true;
                    entry.Property(t => t.ModifyDate).IsModified = true;
                    db.SaveChanges();
                }



            }
            catch (Exception ex)
            {
                return base.ErrorResult(ex.Message);
            }
            return Json(result);
        }

        public ActionResult InviteCode()
        {
            RQRInfo qr = null;

            string type = Request.QueryString["type"];
            if (UserSession.UserStatus == UserStatus.WaitRewiew)
            {
                UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "等待您上级的审核" });
            }

            if (!string.IsNullOrEmpty(type) && type == "us")
            {
                /*暂时不支持
                if (!HasUserStore())
                    return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
                */
                return RedirectToAction("ErrorMessage", "Home", new { code = (int)Errorcode.NoStoreAuthorized });
            }
            else
            {
                if (UserSession.UserRole < UserRole.Agent)
                {
                   
                    return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
                }
            }
            if (UserSession.UserStatus == UserStatus.WaitRewiew) UserSession = null;

            //else
            //{
            //    if (!UserSession.HasPassInviteFee)
            //    {

            //        using (AliPayContent db = new AliPayContent())
            //        {
            //            float num = db.DBOrder.Where(o => o.AgentOpenId == UserSession.OpenId && o.OrderStatus == OrderStatus.Closed).ToList().Sum(o=>o.TotalAmount);
            //            if(num> RuleManager.PayRule().Agent_InviteFee)
            //            {
            //                IQBCore.IQBPay.Models.User.EUserInfo ui = new IQBCore.IQBPay.Models.User.EUserInfo();
            //                ui.Id = UserSession.Id;
            //                ui.HasPassInviteFee = true;

            //                DbEntityEntry<IQBCore.IQBPay.Models.User.EUserInfo> entry = db.Entry<IQBCore.IQBPay.Models.User.EUserInfo>(ui);
            //                entry.State = EntityState.Unchanged;
            //                entry.Property(t => t.HasPassInviteFee).IsModified = true;
            //                db.SaveChanges();

            //                UserSession.HasPassInviteFee = true;
            //                UserSession = UserSession;
            //            }
            //            else
            //            {
            //                return RedirectToAction("PrivilegeError", "Home", new { code = 1000, curAmt= num });
            //            }
            //        }
            //    }
            //}

            InitProfilePage();
            string PPWeb = ConfigurationManager.AppSettings["Site_IQBPay"];
            using (AliPayContent db = new AliPayContent())
            {
                qr = db.DBQRInfo.Where(a => a.OwnnerOpenId == UserSession.OpenId).Select(a=>new RQRInfo {
                    ID = a.ID,
                    Rate=10-a.Rate,
                    ParentOpenId = a.ParentOpenId,
                    ParentCommissionRate = a.ParentCommissionRate,
                    FilePath = PPWeb + a.FilePath,
                    RecordStatus = a.RecordStatus,
                   
                }).FirstOrDefault();

                if (qr == null)
                    throw new Exception("没有找到您的邀请码，请联系管理员");

                if(UserSession.UserRole == UserRole.Administrator)
                {
                   // qr.StoreList = db.Database.SqlQuery<HashStore>("select Id,Name,IsReceiveAccount from storeinfo").ToList();
                    qr.ParentAgentList = db.Database.SqlQuery<HashUser>("select OpenId,Name from userinfo where UserRole = 3 or UserRole=100").ToList();
                }
                    
            }

            return View(qr);
        }

        public ActionResult AgentList()
        {
            RQRInfo qr = null;

            if (UserSession.UserStatus == UserStatus.WaitRewiew)
            {
                UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "等待您上级的审核" });
            }

            if (UserSession.UserRole < UserRole.Agent)
            {
                if (UserSession.UserStatus == UserStatus.WaitRewiew) UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            string type = Request.QueryString["type"];
            if(!string.IsNullOrEmpty(type) && type == "us")
            {
                if (!HasUserStore())
                {

                    return RedirectToAction("ErrorMessage", "Home", new { code = (int)Errorcode.NoStoreAuthorized });
                }
               
            }
           

            InitProfilePage();

            return View();
        }

        [HttpPost]
        public ActionResult DoBlockUser(string ID)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    IQBCore.IQBPay.Models.User.EUserInfo ui = new IQBCore.IQBPay.Models.User.EUserInfo();
                    ui.Id = Convert.ToInt32(ID);
                    ui.UserStatus = UserStatus.JustRegister;
                    DbEntityEntry<IQBCore.IQBPay.Models.User.EUserInfo> entry = db.Entry<IQBCore.IQBPay.Models.User.EUserInfo>(ui);
                    entry.State = EntityState.Unchanged;
                    entry.Property(t => t.UserStatus).IsModified = true;
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
           
            return Json(result);
        }

        public ActionResult AgentListQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);
            string AgentName = Request["AgentName"];

            List<RUser_ARQR> data = new List<RUser_ARQR>();

            using (AliPayContent db = new AliPayContent())
            {

                var sqlScript = @"select ui.Id as userId,ui.Name as UserName, 
CONVERT(varchar(100), ui.RegisterDate, 111) as RegisterDate,
ui.HeadImgUrl,MemberTotalAmount,qr.QRType,(qr.MarketRate-qr.Rate) as FeeRate,ui.UserStatus 
from UserInfo as ui
right join QRUser as qr on qr.OpenId = ui.OpenId and (qr.QRType =1 or qr.QRType=2)
left join
(
select sum(o.TotalAmount) as MemberTotalAmount ,o.AgentOpenId,o.OrderType from OrderInfo as o 
where o.OrderStatus =2
group by o.AgentOpenId ,o.OrderType

) as od on od.AgentOpenId = ui.OpenId and od.OrderType = qr.QRType where 1=1 ";

                if (UserSession.UserRole != UserRole.Administrator 
                    && UserSession.OpenId != "o3nwE0i_Z9mpbZ22KdOTWeALXaus")
                    sqlScript += string.Format(" and ui.UserStatus = 1 and ui.parentOpenId = '{0}'", UserSession.OpenId);
                else
                    sqlScript += " and ui.UserStatus = 1 ";

                if (!string.IsNullOrEmpty(AgentName))
                {
                    sqlScript += string.Format(" and UserName like '%{0}%' ", AgentName);
                }

                sqlScript += " order by userId desc,qr.QRType";

              //  string sql = string.Format(sqlScript, "1");
                var query = db.Database.SqlQuery<RUser_ARQR>(sqlScript);
               
                if (pageIndex == 0)
                {
                    data = query.Take(pageSize).ToList();

                    //if (data.Count > 0)
                    //{
                    //    data[0].TotalMember = db.DBUserInfo.Where(o => o.parentOpenId == UserSession.OpenId && o.UserStatus == UserStatus.PPUser).Count();
                    //    sql = string.Format("select MaxInviteCount from QRInfo where OwnnerOpenId = '{0}'", UserSession.OpenId);
                    //    data[0].MaxInviteCount = db.Database.SqlQuery<int>(sql).FirstOrDefault();

                    //    sql = string.Format("select sum(TotalAmount) as TotalAmount from OrderInfo where OrderStatus =2 and ParentOpenId = '{0}'", UserSession.OpenId);
                    //    try
                    //    {
                    //        double d = db.Database.SqlQuery<double>(sql).FirstOrDefault();
                    //        data[0].TotalAmount = (float)d;
                    //    }
                    //    catch
                    //    {
                    //        data[0].TotalAmount = 0;
                    //    }
                       
                    //}
                    
                }
                else
                    data = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                ////List<RUser_ARQR> hugeData = null;
                ////sql = string.Format(sqlScript, "4");
                ////query = db.Database.SqlQuery<RUser_ARQR>(sql);
                ////hugeData = query.ToList();

                ////foreach (RUser_ARQR d in data)
                ////{
                ////   foreach(RUser_ARQR huge in hugeData)
                ////    {
                ////        if(d.userId == huge.userId)
                ////        {
                ////            d.HugeQR = huge;
                ////            break;
                ////        }
                ////    }
                ////}

                return Json(data);
            }
          
        }

        public ActionResult AgentDetail()
        {
            RQRInfo qr = null;
            EQRUser qrUser = null;
            if (UserSession.UserRole < UserRole.DiamondAgent || UserSession.UserStatus == UserStatus.WaitRewiew)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            InitProfilePage();

            string inQRUser = Request.QueryString["QrUserId"];
            if(string.IsNullOrEmpty(inQRUser))
            {
                return RedirectToAction("AgentList", "PP");
            }
            long qrUserId = Convert.ToInt64(inQRUser);
            using (AliPayContent db = new AliPayContent())
            {
                qrUser =  db.DBQRUser.Where(a => a.ID == qrUserId).FirstOrDefault();
                
            }
            if (qrUser == null)
                qrUser = new EQRUser();
                return View(qrUser);
        }
        [HttpPost]
        public ActionResult AgentDetailSave(EQRUser qrUser)
        {
            OutAPIResult result = new OutAPIResult();
            result.IsSuccess = true;

            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EQRUser CurrentQr = db.DBQRUser.Where(q => q.ID == qrUser.ID).FirstOrDefault();
                    int childs = db.DBQRUser.Where(q => q.ParentOpenId == CurrentQr.OpenId).Count();
                    if(childs>0)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "该会员有下面有子代理，涉及到子代理点问题，手机上不能修改！";
                        return Json(result);
                    }
                    float RDiff = qrUser.Rate - CurrentQr.Rate;
                    float PcDiff = qrUser.ParentCommissionRate - CurrentQr.ParentCommissionRate;
                    List<EQRUser> list = db.DBQRUser.Where(q => q.OpenId == CurrentQr.OpenId).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        EQRUser upQrUser = list[i];
                        upQrUser.Rate += RDiff;
                        if (!string.IsNullOrEmpty(CurrentQr.ParentOpenId))
                        {
                            upQrUser.ParentCommissionRate += PcDiff;
                        }         
                    }
                    
                  
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            
            return Json(result);
        }

        [HttpPost]
        public ActionResult UpdateAgentPhone()
        {
            string phone = Request["Phone"];

            OutAPIResult result = new OutAPIResult();
            try
            {
                if(string.IsNullOrEmpty(phone))
                {
                    result.IsSuccess = false;
                    result.ErrorMsg="手机号为空";
                    return Json(result);
                }
                using (AliPayContent db = new AliPayContent())
                {
                    var sql = string.Format("select count(1) from userinfo where UserPhone = '{0}'",phone);
                    int count = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                    if(count>0)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "此手机号已被注册";
                        return Json(result);
                    }
                    else
                    {
                        db.DBUserInfo.Where(a => a.OpenId == UserSession.OpenId).Update(a => new EUserInfo()
                        {
                            UserPhone = phone,
                        });
                        UserSession.AgentPhone = phone;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;

            }
            return Json(result);
        }

        public ActionResult AgentAccount()
        {
            if (UserSession.UserStatus == UserStatus.WaitRewiew)
            {
                UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "等待您上级的审核" });
            }

            if (UserSession.UserRole < UserRole.Agent)
            {
                if (UserSession.UserStatus == UserStatus.WaitRewiew) UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            if(string.IsNullOrEmpty(UserSession.AliPayAccount))
            {
                string sql = string.Format(@"select AliPayAccount from UserInfo where openId='{0}'", UserSession.OpenId);
                using (AliPayContent db = new AliPayContent())
                {
                    string r = db.Database.SqlQuery<string>(sql).FirstOrDefault();
                    ViewBag.AlipayAccount = r;
                }
            }

        
            InitProfilePage();
            ViewBag.AgentPhone = UserSession.AgentPhone;
            ViewBag.AlipayAccount = UserSession.AliPayAccount;
            ViewBag.ppUrl = ConfigurationManager.AppSettings["Site_IQBPay"];
            EUserAccountBalance ub;
            using (AliPayContent db = new AliPayContent())
            {
                string openId = UserSession.OpenId;
                ub = db.DBUserAccountBalance.Where(a => a.OpenId == openId && a.UserAccountType == UserAccountType.Agent).FirstOrDefault();
                if (ub == null)
                {
                    ub = new EUserAccountBalance()
                    {
                        OpenId = openId,
                        UserAccountType = UserAccountType.Agent,
                        Balance =0,
                        O2OShipInCome =0,
                        O2OShipOutCome =0,
                    };
                    db.DBUserAccountBalance.Add(ub);
                    db.SaveChanges();
                }
            }

            return View(ub);
        }

        [HttpPost]
        public ActionResult AgentAccountTransQuery()
        {
            string OpenId = UserSession.OpenId;
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);

            NResult<RO2OTransAgent> result = new NResult<RO2OTransAgent>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                   var list =  db.DBO2OTransAgent.Where(a => a.AgentOpenId == OpenId).Select(a=>new RO2OTransAgent() {
                       AgentOpenId = a.AgentOpenId,
                       Amount = a.Amount,
                       Id = a.Id,
                       TransactionType = a.TransactionType,
                       TransDateTime = a.TransDateTime,
                   }).OrderByDescending(a=>a.TransDateTime);

                    if (pageIndex == 0)
                        result.resultList = list.Take(pageSize).ToList();
                    else
                        result.resultList = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                 }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;

            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult TransferAmoutToAgent()
        {
            OutAPIResult result = new OutAPIResult();
            string openId = UserSession.OpenId;
            AliPayManager payManager = new AliPayManager();
         
            try
            {
                double amt = Convert.ToDouble(Request["Amt"]);
                int minAmt = Convert.ToInt32(Request["minAmt"]);
                if (amt < minAmt )
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = string.Format("金额不正确,必须大于{0}", minAmt);
                    return Json(result);
                }
                using (AliPayContent db = new AliPayContent())
                {
                    EUserAccountBalance ub = db.DBUserAccountBalance.Where(a => a.OpenId == openId && a.UserAccountType == UserAccountType.Agent).FirstOrDefault();
                    if(ub == null)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "账户余额表出错，请联系平台";
                        return Json(result);
                    }
                    if(ub.Balance<amt)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "提款金额不能大于余额";
                        return Json(result);
                    }
                    if(ub.Balance<1)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "提现金额必须大于1元";
                        return Json(result);
                    }
                    EUserInfo ui = db.DBUserInfo.Where(a => a.OpenId == openId).FirstOrDefault();
                    if(string.IsNullOrEmpty(ui.AliPayAccount))
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "没有设置支付宝账户";
                        result.IntMsg = -10;
                        return Json(result);
                    }
                    //支付宝转账给代理（提现操作）
                    ETransferAmount AliTrans = new ETransferAmount()
                    {
                        AgentOpenId = ui.OpenId,
                        AgentName = ui.Name,
                        TargetAccount = ui.AliPayAccount,
                        TransDate = DateTime.Now,
                        TransDateStr = DateTime.Now.ToShortDateString(),
                        TransferStatus = TransferStatus.Open,
                        TransferTarget = TransferTarget.Agent,
                        TransferAmount = (float)amt, 

                    };
                    EAliPayApplication app;
                    app = db.DBAliPayApp.Where(a => a.IsSubAccount == true).FirstOrDefault();
                    if (app == null)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "没有AliPay应用,请联系管理员";
                        return Json(result);
                    }
                    AliTrans = payManager.O2OTransferHandler(AliTrans, app, app);
                    if (AliTrans.TransferStatus == TransferStatus.Failure)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "转账失败:"+ AliTrans.Log;
                        return Json(result);
                    }
                    else
                    {
                        db.DBTransferAmount.Add(AliTrans);

                        EO2OTransAgent o2oTrans = new EO2OTransAgent()
                        {
                            AgentOpenId = openId,
                            Amount = amt,
                            TransDateTime = DateTime.Now,
                            TransactionType = TransactionType.GetCash,

                        };
                        db.DBO2OTransAgent.Add(o2oTrans);

                        //账户变动（支出）
                        ub.Balance -= amt;
                        // ub.Balance = 
                        if (ub.Balance > 0)
                        {
                            ub.Balance = double.Parse(ub.Balance.ToString("0.00"));
                        }
                      //  ub.O2OShipOutCome += amt;

                        db.SaveChanges();
                    }

                }
                    
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }

        #endregion

        #region 加盟商户
        public ActionResult StoreList()
        {
            if (UserSession.UserStatus == UserStatus.WaitRewiew)
            {
                UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "等待您上级的审核" });
            }
           
            if (!HasUserStore())
            {

                return RedirectToAction("ErrorMessage", "Home", new { code = (int)Errorcode.NoStoreAuthorized });
               
            }
            
            ViewBag.ppUrl = ConfigurationManager.AppSettings["Site_IQBPay"];

            InitProfilePage();

            return View();
        }

        [HttpPost]
        public ActionResult APPList()
        {
            NResult<EAliPayApplication> result = new NResult<EAliPayApplication>();
            try
            {
              
                using (AliPayContent db = new AliPayContent())
                {
                    result.resultList = db.DBAliPayApp.ToList();
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
              
            }
            return Json(result);
        }

        public ActionResult StoreQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);
            string openId = Request["OpenId"];
            string SelAppId = Request["SelAppId"];

            NResult<RStoreInfo> result = new NResult<RStoreInfo>();

            if (string.IsNullOrEmpty(SelAppId))
            {
                result.ErrorMsg = "应用ID没有获取";
                return Json(result);
            }
            try
            {
                using (AliPayContent db = new AliPayContent())
                {

                    var list = db.DBStoreInfo.Select(s => new RStoreInfo()
                    {
                        Name = s.Name,
                        Rate = s.Rate,
                        IsAuth = !string.IsNullOrEmpty(s.AliPayAccount),
                        DayIncome = s.DayIncome,
                        MaxLimitAmount = s.MaxLimitAmount,
                        MinLimitAmount = s.MinLimitAmount,
                        RemainAmount = s.RemainAmount,
                        StoreType = s.StoreType,
                        ID = s.ID,
                        RecordStatus = s.RecordStatus,
                        OwnnerOpenId = s.OwnnerOpenId,
                        FromIQBAPP = s.FromIQBAPP,
                        CDate = s.CDate,
                        CTime = s.CTime,
                        CreateDate = s.CreateDate
                    }).Where(a=>a.FromIQBAPP == SelAppId);

                    if (UserSession.UserRole != UserRole.Administrator)
                        list = list.Where(o => o.OwnnerOpenId == openId);

                    list = list.OrderByDescending(o => o.CreateDate);

                   

                    if (pageIndex == 0)
                        result.resultList = list.Take(pageSize).ToList();
                    else
                        result.resultList = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                   
                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }

        public ActionResult StoreAdd()
        {
            if (UserSession.UserRole < UserRole.Administrator)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            InitProfilePage();

            return View();
        }
        #endregion

        #region 系统

        public  ActionResult SysMain()
        {
            if (UserSession.UserRole < UserRole.Administrator)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            return View();
        }
        [HttpPost]
        public ActionResult RefreshSession()
        {
            WXBaseController.RefreshSession = true;
            return Content("OK");
        }
        #endregion

        #region 报表
        public ActionResult Report_OverView()
        {
            if (UserSession.UserRole < UserRole.Administrator)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            InitProfilePage();

            return View();
        }


        #endregion

        #region 大额入口设置
        public ActionResult QRHugeEntry()
        {
            return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg="产品研发中，尽情期待！" });

            if (WXBaseController.GlobalConfig.QRHugeEntry == IQBCore.IQBPay.BaseEnum.QRHugeEntry.Stop)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "大额通道维护中，请明天再尝试！" });
            }

            if (!UserSession.HasQRHuge)
            {
                using (AliPayContent db = new AliPayContent())
                {
                    float num = db.DBOrder.Where(o => o.AgentOpenId == UserSession.OpenId && o.OrderStatus == OrderStatus.Closed).ToList().Sum(o => o.TotalAmount);
                    if (num > RuleManager.PayRule().Agent_QRHugeFee)
                    {
                       
                        EQRUser bQRUser = db.DBQRUser.Where(o => o.OpenId == UserSession.OpenId && o.QRType == QRReceiveType.Huge).FirstOrDefault();
                        if (bQRUser == null)
                        {
                            EQRUser sQRUser = db.DBQRUser.Where(o => o.OpenId == UserSession.OpenId && o.QRType == QRReceiveType.Small).First();
                            //大额码参数
                            bQRUser = EQRUser.CopyToQRUserForHuge(sQRUser);
                            if (UserSession.UserRole == UserRole.Agent)
                                bQRUser.Rate = (float)6.5;
                            else
                                bQRUser.Rate = 7;
                            bQRUser.MarketRate = 10;

                            db.DBQRUser.Add(bQRUser);

                           
                        }

                        IQBCore.IQBPay.Models.User.EUserInfo ui = new IQBCore.IQBPay.Models.User.EUserInfo();
                        ui.Id = UserSession.Id;
                        ui.HasQRHuge = true;

                        DbEntityEntry<IQBCore.IQBPay.Models.User.EUserInfo> entry = db.Entry<IQBCore.IQBPay.Models.User.EUserInfo>(ui);
                        entry.State = EntityState.Modified;
                        entry.Property(t => t.HasQRHuge).IsModified = true;

                       
                        db.SaveChanges();

                       


                        UserSession.HasQRHuge = true;
                        UserSession = UserSession;
                    }
                    else
                    {
                        return RedirectToAction("PrivilegeError", "Home", new { code = 1001, curAmt = num });
                    }
                }
            }
           

            InitProfilePage();
            RQRUser qrUser;

      


            using (AliPayContent db = new AliPayContent())
            {
                qrUser = db.DBQRUser.Select(o=>new RQRUser {
                    FeeRate = o.MarketRate - o.Rate,
                    MarketRate = o.MarketRate,
                    OpenId = o.OpenId,
                    RecordStatus = o.RecordStatus,
                    QRType = o.QRType,
                    ID = o.ID,
                }).Where(o => o.OpenId == UserSession.OpenId 
                                        && o.RecordStatus == RecordStatus.Normal
                                        && o.QRType == QRReceiveType.Huge).FirstOrDefault();
                if(qrUser == null)
                {
                    return RedirectToAction("ErrorMessage", "Home", new {code = Errorcode.QRHugeQRUserMiss });
                }

                if (qrUser.RecordStatus == RecordStatus.Blocked)
                {
                    return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.QRHugeBlock });
                }

                List<EQRHuge> qrHugeList = db.DBQRHuge.Where(o => o.OpenId == UserSession.OpenId && o.QRHugeStatus == QRHugeStatus.Created).ToList();
                
                foreach(EQRHuge qrHuge in qrHugeList)
                {
                    if(DateHelper.IsOverTime(qrHuge.CreateDate,600))
                    {
                        qrHuge.QRHugeStatus = QRHugeStatus.InValid;
                    }
                }
                if(qrHugeList.Count>0)
                    db.SaveChanges();
            }

           ViewBag.PPSite = ConfigurationManager.AppSettings["Site_IQBPay"];
            ViewBag.QRUserId = qrUser.ID;
            return View(qrUser);
        }

        private OutAPI_QRHuge CreateQRHuge(AliPayContent db,string OpenId, float Amount)
        {
            EQRHuge qrHuge = new EQRHuge
            {
                OpenId = OpenId,
                AgentName = UserSession.Name,
                Amount = Convert.ToSingle(Amount.ToString("0.00")),
                CreateDate = DateTime.Now,
                QRHugeStatus = QRHugeStatus.Created,
            };
            db.DBQRHuge.Add(qrHuge);
            db.SaveChanges();

            string url = ConfigurationManager.AppSettings["Site_IQBPay"] + "API/QRAPI/CreateQRHuge";
            string data = string.Format("ID={0}&OpenId={1}&Amount={2}", qrHuge.ID, qrHuge.OpenId, qrHuge.Amount);

            string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
            OutAPI_QRHuge result = JsonConvert.DeserializeObject<OutAPI_QRHuge>(res);
            result.RQRHuge.CreateDate = qrHuge.CreateDate;
            return result;
        }

        public ActionResult QRHugeMake(string OpenId,float Amount)
        {
            OutAPI_QRHuge result = new OutAPI_QRHuge();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    result = CreateQRHuge(db, OpenId, Amount);
                   
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 返回-1说明不需要控制按钮
        /// </summary>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QRHugeGet(string OpenId)
        {
            OutAPI_QRHuge result = new OutAPI_QRHuge();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    RQRHuge qrHuge = db.DBQRHuge.Select(o => new RQRHuge
                    {
                        AgentName = o.AgentName,
                        Amount = o.Amount,
                        CreateDate = o.CreateDate,
                        FilePath = o.FilePath,
                        PayCount = o.PayCount,
                        QRHugeStatus = o.QRHugeStatus,
                        OpenId = o.OpenId,
                        ID = o.ID

                    }).Where(o => o.OpenId == OpenId).OrderByDescending(o => o.CreateDate).FirstOrDefault();

                    if (qrHuge != null)
                    {
                        bool IsOverTime = DateHelper.IsOverTime(qrHuge.CreateDate, 600);
                        if(IsOverTime)
                        {
                            //超时 还能找到创建的QRHuge,可能是没有扫，手动变为失效
                            if (qrHuge.QRHugeStatus == QRHugeStatus.Created)
                            {
                                EQRHuge obj = new EQRHuge();
                                obj.ID = qrHuge.ID;
                                obj.QRHugeStatus = QRHugeStatus.InValid;

                                DbEntityEntry<EQRHuge> entryHuge = db.Entry<EQRHuge>(obj);
                                entryHuge.State = EntityState.Unchanged;
                                entryHuge.Property(t => t.QRHugeStatus).IsModified = true;

                                db.SaveChanges();
                            }
                            result.DiffSec = -1;
                        }
                        else
                        {
                            if (qrHuge.QRHugeStatus == QRHugeStatus.Created)
                            {
                                result.RQRHuge = qrHuge;
                                result.DiffSec = DateHelper.GetDiffSec(qrHuge.CreateDate, DateTime.Now);
                            }
                            else if(qrHuge.QRHugeStatus == QRHugeStatus.InValid)
                            {
                                result.DiffSec = DateHelper.GetDiffSec(qrHuge.CreateDate, DateTime.Now);
                            }
                            else
                                result.DiffSec = -1;

                        }    
                    }
                    else
                        result.DiffSec = -1;
                }

            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }

        public ActionResult QRHugeList(string openId)
        {
            List<RQRHuge> list = null;
            using (AliPayContent db = new AliPayContent())
            {
                list = db.DBQRHuge.Select(o => new RQRHuge
                {
                    Amount = o.Amount,
                    CreateDate = o.CreateDate,
                    QRHugeStatus = o.QRHugeStatus,
                    ID = o.ID,
                    OpenId = o.OpenId,
                }).Where(o => o.OpenId == openId)
                .OrderByDescending(o=>o.ID).Take(10).ToList();
                if (list == null)
                    list = new List<RQRHuge>();

               
            }
            return Json(list);
        }
        #endregion

        #region News
        [OutputCache(Duration = 60*60)]
        public ActionResult News()
        {
            WXHelperController wx = new WXHelperController();
            JOMedia_News news = null;
            List<RNews> result = new List<RNews>();
            try
            {
                string access_token = this.getAccessToken();
                news = wx.GetNews(access_token);
                foreach(ItemItem item in news.item)
                {
                    foreach (News_itemItem newsItem in item.content.news_item)
                    {
                        if (newsItem.author == "虔诚" || newsItem.author=="玉杰官网")
                        {
                            RNews r = new RNews();
                            r.DetailUrl = newsItem.url.Replace("\\","");
                            r.ImgUrl = newsItem.thumb_url.Replace("\\", "");
                            r.Title = newsItem.title;
                            r.Summery= System.Web.HttpUtility.HtmlEncode(newsItem.content.Substring(0, 100));
                                    
                            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                            DateTime dt = startTime.AddSeconds(item.content.create_time);
                            r.CreateDate = dt.ToString("yyyy-MM-dd hh:mm:ss");
                            result.Add(r);
                          
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("ErrorMessage", "Home",new { code = 2000, ErrorMsg ="获取信息错误，请联系管理员！"});
            }
            

            return View(result);
        }


        #endregion

        #region O2O
        public ActionResult O2OQrEntry()
        {

            if (UserSession.O2OUserRole != O2OUserRole.Agent && UserSession.UserRole != UserRole.Administrator)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }

            return View();
        }

       
        #endregion

        public ActionResult StoreAuth()
        {
            RQRStoreAuth qrAuth = null;
            try
            {
                long Id = Convert.ToInt32(Request["Id"]);
                string sql = string.Format(@"select s.Name as StoreName,sa.FilePath from StoreInfo as s
                               join QRStoreAuth as sa on sa.StoreId = s.ID
                               where s.Id='{0}'",Id);

                using (AliPayContent db = new AliPayContent())
                {
                    qrAuth = db.Database.SqlQuery<RQRStoreAuth>(sql).FirstOrDefault();
                    qrAuth.FilePath = ConfigurationManager.AppSettings["Site_IQBPay"] + qrAuth.FilePath;
                }
            }
            catch(Exception ex)
            {
               return RedirectToAction("ErrorMessage", "Home",new { code="9000", ErrorMsg=ex.Message, backUrl="/PP/StoreList" });
            }
            if (qrAuth == null)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = "9000", ErrorMsg = "授权码缺失，请联系平台", backUrl = "/PP/StoreList" });
            }

            return View(qrAuth);
        }

        public ActionResult UserStoreInfo()
        {
            EUserStore us = null;
            if (UserSession.UserStatus == UserStatus.WaitRewiew)
            {
                UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "等待您上级的审核" });
            }
                

            if (UserSession.UserRole < UserRole.Agent)
            {
              
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            InitProfilePage();

            using (AliPayContent db = new AliPayContent())
            {
               us = db.DBUserStore.Where(a => a.OpenId == UserSession.OpenId).FirstOrDefault();
            }
            if(us == null)
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg ="没有店铺权限，需联系平台单独开通" });
            else
                return View(us);
        }

        public ActionResult PaySuccess()
        {
            string OrderNo = Request.QueryString["No"];
        
            if(!string.IsNullOrEmpty(OrderNo))
            {
                try
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        EOrderInfo order = db.DBOrder.Where(a => a.OrderNo == OrderNo &&
                                                                 a.OrderStatus == OrderStatus.WaitingAliPayNotify &&
                                                                 a.OrderType == OrderType.WX).FirstOrDefault();
                      
                        NLogHelper.ErrorTxt("PaySuccess -- No:"+OrderNo);

                        if (order != null)
                        {
                            order.OrderStatus = OrderStatus.WaitingSysPay;
                            db.SaveChanges();

                            var accessToken = JsApiPay.GetAccessToken();
                            string openId = "o3nwE0qI_cOkirmh_qbGGG-5G6B0";
                            PaySuccessTellAdminNT notice = new PaySuccessTellAdminNT(accessToken, openId, order);
                            notice.Push();
                        }
                    }
                }
                catch (Exception ex)
                {
                    NLogHelper.ErrorTxt("PaySuccess -- " + ex.Message);
                }
            }
             return View();
        }

        /// <summary>
        /// 新会员审核
        /// </summary>
        /// <returns></returns>
        public ActionResult NewMemberReview()
        {
            //if (UserSession.UserRole< UserRole.Agent)
            //{
            //    return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            //}

            string newOpenId = Request.QueryString["nOpenId"];
            if (string.IsNullOrEmpty(newOpenId))
                newOpenId = Request["nOpenId"];
           // newOpenId = "o3nwE0jrONff65oS-_W96ErKcaa0";
            ViewBag.NewOpenId = newOpenId;
            NLogHelper.InfoTxt("NewMemberReview NewOpenId:" + newOpenId);

            if (!string.IsNullOrEmpty(newOpenId))
            {
                
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = string.Format(@"select name as NewName,UserStatus,OpenId as NewOpenId 
                                              from userInfo where openId = '{0}'", newOpenId);

                    var obj = db.Database.SqlQuery<PNewMemberReview>(sql).FirstOrDefault();
                    if(obj == null)
                        return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "没找到会员，请联系平台" });
                    else
                    {
                        ViewBag.UserStatus = (int)obj.UserStatus;
                        ViewBag.NewName = obj.NewName;
                    }                   
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "微信通知Bug，请返回尝试重新进入审核页面" });
            }
           
            return View();
        }

        public ActionResult AgentQRAdjustment()
        {
            if (UserSession.UserStatus == UserStatus.WaitRewiew)
            {
                UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg = "等待您上级的审核" });
            }

            if (UserSession.UserRole < UserRole.Agent)
            {
                if (UserSession.UserStatus == UserStatus.WaitRewiew) UserSession = null;
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }

            InitProfilePage();
            return View();
        }


    }
}