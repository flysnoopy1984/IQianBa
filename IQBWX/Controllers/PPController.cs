using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.User;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBWX.Models.WX.Template;
using IQBCore.WxSDK;
using IQBWX.BLL;
using IQBWX.BLL.ExternalWeb;
using IQBWX.BLL.NT;
using IQBWX.DataBase.IQBPay;
using IQBWX.Models.Results;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using IQBWX.Models.WX.Template;
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

        public ActionResult Pay(string Id)
        {
           // return RedirectToAction("PayWithAccount", "PP", new { Id = Id });

            if(WXBaseController.GlobalConfig.WebStatus == PayWebStatus.Stop)
            {
                return RedirectToAction("ErrorMessage", "Home",new { code = Errorcode.SystemMaintain, ErrorMsg = WXBaseController.GlobalConfig.Note });
            }
            ViewBag.QRUserId = Id;
           // ViewBag.ReceiveNo = StringHelper.GenerateReceiveNo();
            return View();
        }

        public ActionResult PayWithAccount(string Id)
        {
            return RedirectToAction("Pay", "PP", new { Id = Id });

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

            }

            return View();
        }

        public ActionResult ReceiveOrder()
        {
            if (WXBaseController.GlobalConfig.WebStatus == PayWebStatus.Stop)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = Errorcode.SystemMaintain, ErrorMsg = WXBaseController.GlobalConfig.Note });
            }
            return View();
        }

        public ActionResult AgentCommList()
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
                        if (result.Count <= 0)
                        {
                            result.Add(new RAgentCommission());
                            //result[0].TotalCommAmount = list.ToList().Sum(s => s.CommissionAmount);
                        }
                        DateTime startDate = DateTime.Today;
                        DateTime endDate = DateTime.Today.AddDays(1);

                        var TodayComm = db.DBAgentCommission.Where(o => o.ParentOpenId == OpenId && o.TransDate >= startDate && o.TransDate <= endDate && o.AgentCommissionStatus == AgentCommissionStatus.Closed).ToList().Sum(o => o.CommissionAmount).ToString("0.00");
                        result[0].TodayCommAmt = TodayComm;

                        result[0].TotalCommAmt = db.DBAgentCommission.Where(o => o.ParentOpenId == OpenId && o.AgentCommissionStatus == AgentCommissionStatus.Closed).ToList().Sum(o => o.CommissionAmount).ToString("0.00");

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
                    var list = db.DBOrder.Where(o => o.AgentOpenId == OpenId).Select(o=>new ROrderInfo {
                        ID = o.ID,
                        OrderNo = o.OrderNo,
                        TransDateStr = o.TransDateStr,
                        OrderStatus = o.OrderStatus,
                        TotalAmount = o.TotalAmount,
                        RateAmount = o.RateAmount,
                        BuyerAliPayLoginId = o.BuyerAliPayLoginId,
                        TransDate = o.TransDate,
                    });

                    if (OrderStatus != OrderStatus.ALL)
                    {
                        list = list.Where(o => o.OrderStatus == OrderStatus);
                    }
                    else
                        list = list.Where(o => o.OrderStatus != OrderStatus.WaitingAliPayNotify);

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
                    //list = list.Where(o => o.OrderStatus == OrderStatus.Paid);
                    list = list.OrderByDescending(i => i.TransDate);

                    if (pageIndex == 0)
                    {
                        DateTime startDate = DateTime.Today;
                        DateTime endDate = DateTime.Today.AddDays(1);

                        int totalCount = list.Count();
                        result = list.Take(pageSize).ToList();
                        if (result.Count > 0)
                        {
                          //  result[0].TotalCount = totalCount;

                        }
                        else
                            result.Add(new ROrderInfo());

                        var TodayOrder = db.DBOrder.Where(o => o.AgentOpenId == OpenId && o.OrderStatus == OrderStatus.Closed && o.TransDate >= startDate && o.TransDate <= endDate);
                        result[0].AgentTodayOrderCount = TodayOrder.Count().ToString();
                        result[0].AgentTodayIncome = TodayOrder.ToList().Sum(o => o.RateAmount).ToString("0.00");

                        var allOrder = db.DBOrder.Where(o => o.AgentOpenId == OpenId && o.OrderStatus == OrderStatus.Closed);
                        result[0].AgentTotalIncome = allOrder.ToList().Sum(o => o.RateAmount).ToString("0.00");
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
            if (UserSession.UserRole < UserRole.Agent)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            InitProfilePage();

          
            return View();
        }

        public ActionResult Agent_QR_ARListQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);

            using (AliPayContent db = new AliPayContent())
            {

                var list = db.DBQRUser.Where(u=>u.OpenId == UserSession.OpenId).Select(s => new RQRUser()
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
                });

               
                list = list.OrderByDescending(o => o.ID);

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
                        DbEntityEntry<EQRUser> entry = db.Entry<EQRUser>(qrUser);
                        entry.State = EntityState.Unchanged;
                        entry.Property(t => t.IsCurrent).IsModified = true;
                        int n = db.DBQRUser.Where(o => o.OpenId == UserSession.OpenId).Count();
                        
                        if (qrUser.IsCurrent && n>1)
                        {
                           
                            var p_OpenId = new SqlParameter("@OpenId", UserSession.OpenId);

                            string sql = @"update [QRUser]
                                           set [IsCurrent] = 'false'
                                           where OpenId =@OpenId";

                            db.Database.ExecuteSqlCommand(sql, p_OpenId);
                        }
                        else
                        {
                            if (n == 1)
                            {
                                return ErrorResult("只有一个二维码不能修改当前选项");
                            }
                        }
                      

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
                    try
                    {
                        int n = db.DBQRUser.Where(o => o.OpenId == UserSession.OpenId && o.MarketRate == qrUser.MarketRate).Count();
                        if (n > 0)
                            return  ErrorResult(string.Format("已经存在用户手续费为{0}的二维码", qrUser.MarketRate));
                           
                    
                        EQRUser curQRUser = db.DBQRUser.Where(o => o.OpenId == UserSession.OpenId && o.IsCurrent == true).FirstOrDefault();
                        if(curQRUser==null)
                            return ErrorResult("没有找到当前的收款二维码，请联系管理员");   
            
                        qrUser.Rate = qrUser.MarketRate - (curQRUser.MarketRate-curQRUser.Rate);
                        qrUser.ReceiveStoreId = curQRUser.ReceiveStoreId;
                        qrUser.ParentOpenId = curQRUser.ParentOpenId;
                        qrUser.ParentCommissionRate = curQRUser.ParentCommissionRate;
                        qrUser.ParentName = curQRUser.ParentName;
                        qrUser.QRId = curQRUser.QRId;
                        qrUser.OpenId = UserSession.OpenId;
                        qrUser.UserName = UserSession.Name;
                       

                        if (qrUser.IsCurrent)
                        {
                            var p_OpenId = new SqlParameter("@OpenId", UserSession.OpenId);

                            string sql = @"update [QRUser]
                                           set [IsCurrent] = 'false'
                                           where OpenId =@OpenId";

                            db.Database.ExecuteSqlCommand(sql, p_OpenId);
                        }

                        db.DBQRUser.Add(qrUser);
                        db.SaveChanges();
                       

                        string url = ConfigurationManager.AppSettings["Site_IQBPay"] + "api/userapi/CreateAgentQR_AR";

                        string data = string.Format("ID={0}&OpenId={1}", qrUser.ID,UserSession.OpenId);
                        try
                        {
                            string res = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
                            result = JsonConvert.DeserializeObject<OutAPIResult>(res);
                        }
                        catch(Exception ex)
                        {
                            result.ErrorMsg = ex.Message;
                            result.IsSuccess = false;
                        }
                        finally
                        {
                            
                            if (!result.IsSuccess)
                            {
                                var p_ID = new SqlParameter("@ID", qrUser.ID);

                                string sql = "delete from QRUser where ID=@ID";
                                db.Database.ExecuteSqlCommand(sql, p_ID);

                                DbEntityEntry<EQRUser> entry = db.Entry<EQRUser>(curQRUser);
                                entry.State = EntityState.Unchanged;
                                curQRUser.IsCurrent = true;
                                entry.Property(t => t.IsCurrent).IsModified = true;
                                db.SaveChanges();
                                
                            }
                        }
                      
                      
                    }
                    catch(Exception ex)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = ex.Message;
                    }
                   
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
                    DbEntityEntry<EQRInfo> entry = db.Entry<EQRInfo>(qrInfo);
                    entry.State = EntityState.Unchanged;
                    entry.Property(t => t.ParentOpenId).IsModified = true;
                    entry.Property(t => t.ParentCommissionRate).IsModified = true;
                    entry.Property(t => t.Rate).IsModified = true;
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
            if (UserSession.UserRole < UserRole.DiamondAgent)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            InitProfilePage();
            string PPWeb = ConfigurationManager.AppSettings["Site_IQBPay"];
            using (AliPayContent db = new AliPayContent())
            {
                qr = db.DBQRInfo.Where(a => a.OwnnerOpenId == UserSession.OpenId).Select(a=>new RQRInfo {
                    ID = a.ID,
                    Rate=a.Rate,
                    ParentOpenId = a.ParentOpenId,
                    ParentCommissionRate = a.ParentCommissionRate,
                    FilePath = PPWeb + a.FilePath,
                    RecordStatus = a.RecordStatus,
                }).FirstOrDefault();

                if (qr == null)
                    throw new Exception("没有找到您的邀请码，请联系管理员");

                if(UserSession.UserRole == UserRole.Administrator)
                {
                    qr.StoreList = db.Database.SqlQuery<HashStore>("select Id,Name,IsReceiveAccount from storeinfo").ToList();
                    qr.ParentAgentList = db.Database.SqlQuery<HashUser>("select OpenId,Name from userinfo").ToList();
                }
                    
            }

            return View(qr);
        }

        public ActionResult AgentList()
        {
            RQRInfo qr = null;
            if (UserSession.UserRole < UserRole.DiamondAgent)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            InitProfilePage();

            return View();
        }

        public ActionResult AgentListQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);
            string AgentName = Request["AgentName"];

            List<RUser_ARQR> result = new List<RUser_ARQR>();

            using (AliPayContent db = new AliPayContent())
            {
                var query = from ui in db.DBUserInfo
                            join qrUser in db.DBQRUser
                            on ui.OpenId equals qrUser.OpenId
                            select new RUser_ARQR
                            {
                                ID = ui.Id,
                                Rate = qrUser.Rate,
                                UserName = ui.Name,
                                ParentCommissionRate = qrUser.ParentCommissionRate,
                                HeadImgUrl =ui.Headimgurl,
                                IsCurrent = qrUser.IsCurrent,
                                ParentOpenId = qrUser.ParentOpenId,
                                UserStatus = ui.UserStatus,
                                MarketRate = qrUser.MarketRate,
                                qrUserId = qrUser.ID,
                            };

                query = query.Where(a => a.IsCurrent == true);
                if (UserSession.UserRole != UserRole.Administrator)
                    query = query.Where(a => a.ParentOpenId == UserSession.OpenId);
                if(!string.IsNullOrEmpty(AgentName))
                {
                    query = query.Where(a=>a.UserName.Contains(AgentName));
                }
                if (pageIndex == 0)
                    result = query.Take(pageSize).ToList();
                else
                    result = query.OrderBy(a=>a.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();

                return Json(result);
            }
          
        }

        public ActionResult AgentDetail()
        {
            RQRInfo qr = null;
            EQRUser qrUser = null;
            if (UserSession.UserRole < UserRole.DiamondAgent)
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

        #endregion

        #region 加盟商户
        public ActionResult StoreList()
        {
            if(UserSession.UserRole < UserRole.Administrator)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            InitProfilePage();

            return View();
        }

        public ActionResult StoreQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);

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

                    CDate = s.CDate,
                    CTime = s.CTime,
                    CreateDate = s.CreateDate
                });

                if (UserSession.UserRole != UserRole.Administrator)
                    list = list.Where(o => o.OwnnerOpenId == UserSession.OpenId);

                list = list.OrderByDescending(o => o.CreateDate);

                List<RStoreInfo> result = new List<RStoreInfo>();

                if (pageIndex == 0)
                    result = list.Take(pageSize).ToList();
                else
                    result = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                return Json(result);
            }
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

      

    }
}