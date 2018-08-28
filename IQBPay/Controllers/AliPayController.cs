using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using IQBCore.Common.Helper;
using IQBPay.Core;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.Sys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using IQBCore.IQBWX.Models.OutParameter;
using IQBCore.IQBPay;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.User;
using IQBCore.IQBWX.Models.WX.Template;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBPay.Models.Result;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using IQBCore.IQBPay.Models.Json;
using IQBCore.Common.Constant;
using IQBCore.IQBWX.Models.WX.Template.InviteCode;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.Report;

namespace IQBPay.Controllers
{
    public class AliPayController : BaseController
    {


        private string privateKey = "MIIEowIBAAKCAQEAwHKeFBun6j3+wQwcgmAoCG7f/TWU1ST+iTKT/oImEiyNOEGrwel3D0TI8/qmeKWVS0u/lKraICRWmIuvd0p8IO5ab3jgYxBJb88NoSCHF8QbZ46BwyLFPc+SdbEvs0VAtyLE4iVzRcBmTHqpo7gt7Ga9MAmaV+x1WUNyiLgxgZqJYQWIyfXkfnGtjL3H6ox+9InwPHIcxZxEEAtdtLZCcrwPCC1zqRp6j191RAFUZ8KNU3zN+J1+QKC1ae4iyt2nBOGtwTVK8LwrMVYIpTffs1sMBNUj3XOqTfwYX7fUFwBrtvaEk4i+CtJopVLNlUxEqauqPEtbPgF1SuxdFIxYPwIDAQABAoIBAHiDThqpdu1pBS8+tluue2NMi1e1Rg5zrDGeSq8GMXEQFR81gKld2gDlwjGGtNi4WFVeigo/M3kNSG0ejDLXogO9P0SvHVTrzhEGSDKue+qWE9M1mmzoSTv70GuDGavZoj0MuN4lNZpocadS6QhtPdTcQXzjhpOor5PGeOLE9buCQz/6YbpgWBKUxWERFZellrgoWaEumDqVSAY4xmflbwL54UIoI/AHlVe3YiKZ2a8RSDpdKQHX4JpU/NHYTI0ZNM6NlXJ5FaAbCdMXgvBQzMz40qg27iF+pCA4jkgS3a5q0BM1+KaB05TlYNh/QHxWaL0Mtt1xgmBlO0n4ho+riwECgYEA9WhqNkzINpHXlGx5opzkUCxA3VCcN2QBkeDFdrdieBfHBIGCVnhmvb+tMq5+mSsEqOdCLmPLAmj3x8XNEt1CiXn34M011/Oh1AcwXgSjtwNySozNsa+qXS57gEoS3xp1n1pkK9bB8Xkebt2l7b2ibkSMLteeuIYl1q4WkLk77qcCgYEAyMEHsyuf8OAL04aGrJWlf4npv7lh9rCqWWLeBrkvt701sDoouwfo5m80i0eWhBbY5Nlh02LqBcJd1Iwk3WSjMA7XNj9CkUWoM4u/hz9YZMigMyPu9EWo7eqGypT9DIhQ67Z1VESlTS0LK2V3bDIkxAJhumgdhCp8iELjEauLVKkCgYBWoXB1EK/Qy7UdeRmLNPVH9AdF2TH8P7pqI72xRdVl7Ybc6Vb4bXJfY22hqYWZTl1Lvq9XLvU4OZPWmtXk5eSaIUtGuUpbnG6xKYSCfALLFVVgScpHAmsSj9kbFYsJ5Q5GnaMk8p/uPUJoAqiTf1D6ugn+czFdlEWBPl1K44jrmwKBgQCE+i/yg7wfHxlWVO7SVRHaKG1YXSDB+oXsTawKQhKUn9V3VR7zvKqOMS1Z8OKHvmaPOFsvXX7sr7Hdf7NPn0DlLX9q5H5gogZnlnMY0GHp6GcNWQkIbzgV2FrOx9/StFz9tc+EMTBZrbOPXFe9qH1oBLfddOfQSyBQVhX492uEeQKBgA/Umil0P/q/nECzL+71LMlpGM6ldRcOsnHKh+5/48h5VImiFn8YhOewboYUnkoBJBJXf5upj3GA+J2OSkojxgOwfBC+EWreXmGltYw/2bZ3CO08Fs80vXDbhMaPIUcjfsS4CCxx0qIJanhf/6522kT3avj5aA4jYE04by8qV4KI";

        private string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwHKeFBun6j3+wQwcgmAoCG7f/TWU1ST+iTKT/oImEiyNOEGrwel3D0TI8/qmeKWVS0u/lKraICRWmIuvd0p8IO5ab3jgYxBJb88NoSCHF8QbZ46BwyLFPc+SdbEvs0VAtyLE4iVzRcBmTHqpo7gt7Ga9MAmaV+x1WUNyiLgxgZqJYQWIyfXkfnGtjL3H6ox+9InwPHIcxZxEEAtdtLZCcrwPCC1zqRp6j191RAFUZ8KNU3zN+J1+QKC1ae4iyt2nBOGtwTVK8LwrMVYIpTffs1sMBNUj3XOqTfwYX7fUFwBrtvaEk4i+CtJopVLNlUxEqauqPEtbPgF1SuxdFIxYPwIDAQAB";

        public string publicKey2 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAshtxB8HjXoFchAfohW4kdnF/b74hA15vVrB+JeBDG7QuRssyR8NG17Nho4VgSQ1eqkVzOpKmNcmGWHEhC7sZosFzFZyOlofqpEte75KXxvyOI/fkoHe5CtQlPuXJln7hivY0er1b/vOXv4cCeMdZUYUPaLdJqCBHZjlay6vmxuw6Y4I62i58eHoP2oVypUj/v0S560OyXv8lk0MGhe1bWfM1RsZcRKCA1lV2sor4PC7KOMTSoIO9k1GWj0FyLWuypif8oX9d9z38FscEGQycu71gY64eXq0Fq4CPf85Y4A5YFxRbstiutIjn8J9F0d8gaVHgKau4EkAdHHRWp+NkJwIDAQAB";

        public string pk1_rs1 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA3aaDFxkj4IfzV42j8lwdJCZgTPTfrwDTiFfxhQXwFk/9sstsNdSkrYzKAmMBgl95d7R9bA8ASc0A8JADgR1ye+gsky9K/l8DEI8ZbgSWgCdEkTHbuZtzLo0SN9Q+U6k/g3QWTV27+0WHXHNwECFhdk23V0s2MeF+HrYgPn0WSkpYwz58hCDV9Eh71sj05tcgWfitcEkMLSazXmDqRsv8LZjtzpXO9Chwssfi9iCWa3hfsuzfmXusk8TRwtRyUtD9hIq4Fxr2+QJ2AvMlyK7/Sgtnsgl+lIv869jVyaNydlwSv8js1TM8nXPVemTWvj7fQUnWhU0YRHVa0XcdeyvaBwIDAQAB";

        public string AppID = ConfigurationManager.AppSettings["APPID"];


        private static List<string>  _BlockList =null ;

        public object EORderInfo { get; private set; }

        public AliPayController()
        {
            if (_BlockList == null)
            {
                _BlockList = new List<string>();
                _BlockList.Add("13225930162");
            }
        }

        public string callF2FPay(EStoreInfo store,string TotalAmt,string orderNo)
        {
            AliPayManager payMag = new AliPayManager();
            EUserInfo ui = new EUserInfo();
            ui.Name = "好又多";
            AliPayResult status;
            string Res = payMag.PayF2FNew(BaseController.App, ui, store, TotalAmt, out status);

            return Res;
        }

        private string callSubAccount2()
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppID,
              privateKey, "json", "1.0", "RSA2", publicKey2, "GBK", false);
            AlipayTradeOrderSettleRequest request = new AlipayTradeOrderSettleRequest();

            request.BizContent = "{" +
            "\"out_request_no\":\"201709290001\"," +
            "\"trade_no\":\"2017092921001004530201479792\"," +
            "      \"royalty_parameters\":[{" +
            "        \"trans_out\":\"2088821092484390\"," +
            "\"trans_in\":\"2088101126708402\"," +
            "\"amount\":0.5," +
            "\"amount_percentage\":30," +
            "\"desc\":\"2088721665327500\"" +
            "        }]," +
            "\"operator_id\":\"A0001\"" +
            "  }";
            AlipayTradeOrderSettleResponse response = client.Execute(request);
            return response.Body;
        }

        private string callSubAccount(string orderNo, string sellerId, long TotalAmt, int Percentage)
        {
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppID,
               privateKey, "json", "1.0", "RSA2", publicKey2, "GBK", false);


            AlipayTradeOrderSettleRequest request = new AlipayTradeOrderSettleRequest();
            Aop.Api.Domain.AlipayTradeOrderSettleModel model = new AlipayTradeOrderSettleModel();

            model.OutRequestNo = StringHelper.GenerateSubAccountTransNo();
            model.TradeNo = orderNo;

            List<OpenApiRoyaltyDetailInfoPojo> paramList = new List<OpenApiRoyaltyDetailInfoPojo>();

            OpenApiRoyaltyDetailInfoPojo p = new OpenApiRoyaltyDetailInfoPojo();
            p.TransOut = sellerId;
            p.TransIn = AliPayConfig.pid;
            p.Amount = TotalAmt;
            p.AmountPercentage = Percentage;


            paramList.Add(p);

            model.RoyaltyParameters = paramList;

            request.SetBizModel(model);


            /*
                        request.BizContent = "{" +
                        "\"out_request_no\":\"{0}\"," +
                        "\"trade_no\":\"{1}\"," +
                        "      \"royalty_parameters\":[{" +
                        "        \"trans_out\":\"{2}\"," +
                        "\"trans_in\":\"{3}\"," +
                        "\"amount\":1," +
                        "\"amount_percentage\":30," +
                        "\"desc\":\"test\"" +
                        "        }]," +
                        "\"operator_id\":\"A0001\"" +
                        "  }";

                        request.BizContent = string.Format(request.BizContent, StringHelper.GenerateSubAccountTransNo(), orderNo, sellerId, AliPayConfig.pid);
                        */
            AlipayTradeOrderSettleResponse response = aliyapClient.Execute(request, "201709BB409adf95ae524bf7809e12d114180X39");
            return response.Body;
        }


        // GET: AliPay
        public ActionResult Index()
        {

            return View();
        }

        public void UpdateUserBalance(AliPayContent db,string openId,double amt, TransactionType transType, ref EReport_Order report)
        {
            if(amt!=0)
            {
                EUserAccountBalance ub = db.DBUserAccountBalance.Where(a => a.OpenId == openId && a.UserAccountType == UserAccountType.Agent).FirstOrDefault();
                ub.Balance += Convert.ToDouble(amt.ToString("0.00"));

                EO2OTransAgent o2oTrans = new EO2OTransAgent
                {
                    AgentOpenId = openId,
                    Amount = Convert.ToDouble(amt.ToString("0.00")),
                    TransDateTime = DateTime.Now,
                    TransactionType = transType,
                };
                db.DBO2OTransAgent.Add(o2oTrans);

                if (report != null)
                {
                    switch (transType)
                    {
                        case TransactionType.Agent_Order_Comm:
                            report.AgentOpenId = openId;
                            report.AgentInCome = Convert.ToSingle(amt.ToString("0.00"));
                            break;
                        case TransactionType.Parent_Comm:
                            report.A2OpenId = openId;
                            report.A2InCome = Convert.ToSingle(amt.ToString("0.00"));
                            break;
                        case TransactionType.L3_Comm:
                            report.A3OpenId = openId;
                            report.A3InCome = Convert.ToSingle(amt.ToString("0.00"));
                            break;
                        case TransactionType.Store_Comm:
                            report.StoreOpenId = openId;
                            report.StoreInCome = Convert.ToSingle(amt.ToString("0.00"));
                            break;
                        case TransactionType.Store_L2:
                            report.S2OpenId = openId;
                            report.S2InCome = Convert.ToSingle(amt.ToString("0.00"));
                            break;
                        case TransactionType.Store_L3:
                            report.S3OpenId = openId;
                            report.S3InCome = Convert.ToSingle(amt.ToString("0.00"));
                            break;

                    }
                }
            }

        }


        public ActionResult PayNotify()
        {
            string orderNo = Request["out_trade_no"];
            AliPayManager payManager = new AliPayManager();
            ETransferAmount tranfer = null;
          
            EOrderDetail orderDetail = null;

            EQRUser _agentQR = null;
            EReport_Order _ReportOrder = new EReport_Order();

            int TransferError = 0;
            EAliPayApplication app = null;

            try
            {
                using (AliPayContent db = new AliPayContent())
                {  
                    EOrderInfo order = db.DBOrder.Where(o => o.OrderNo == orderNo).FirstOrDefault();

                    if (order == null)
                    {   
                        return View();
                    }

                    if (order.OrderStatus != IQBCore.IQBPay.BaseEnum.OrderStatus.WaitingAliPayNotify)
                        return View();

                    order.AliPayOrderNo = Request["trade_no"];
                    order.AliPayPayChannel = "";
                    order.AliPayTradeStatus = Request["trade_status"];

                    order.AliPayAppId = Request["app_id"];

                    order.BuyerAliPayId = Request["buyer_id"];
                    order.BuyerAliPayLoginId = Request["buyer_logon_id"];

                    order.SellerAliPayEmail = Request["seller_email"];

                    order.AliPayTotalAmount = Convert.ToSingle(Request["total_amount"]);
                    order.AliPayReceiptAmount = Convert.ToSingle(Request["receipt_amount"]);
                    order.AliPayBuerPayAmount = Convert.ToSingle(Request["buyer_pay_amount"]);

                    string payMethod = Request["fund_bill_list"];
                    orderDetail = new EOrderDetail();
                    orderDetail.OrderNo = order.OrderNo;
                    orderDetail.fund_bill_list = payMethod;
                    db.DBOrderDetail.Add(orderDetail);
                  //  order.AliPayTransDate = Convert.ToDateTime(Request["gmt_create"]);
                    if (order.AliPayTradeStatus == "TRADE_SUCCESS")
                    {
                        #region 原大额码逻辑

                        //不管打款是否成功，用户是成功支付了大额码
                        //if(order.EQRHugeTransId>0)
                        //{
                        //    try
                        //    {
                        //        UpdateQRHuge(db, order.EQRHugeTransId);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Log.log("PayNotify UpdateQRHuge Error:"+ ex.Message);
                        //    }
                        //}
                        #endregion

                        #region 短信通知收款码
                        //短信通知买家收款码开始
                        // base.Log.log("BuyerMobilePhone" + order.BuyerMobilePhone);
                        //if(!string.IsNullOrEmpty(order.BuyerMobilePhone))
                        //{
                        //    try
                        //    {
                        //        InSMS inSMS = new InSMS();
                        //        inSMS.Init();
                        //        inSMS.PhoneNumber = order.BuyerMobilePhone;
                        //        inSMS.Parameters = order.ReceiveNo;
                        //        inSMS.Tpl_id = Convert.ToInt32(SMSTemplate.ReceiveConfirm).ToString();

                        //        SMSManager smsMgr = new SMSManager();
                        //        smsMgr.PostSMS_API51(inSMS);
                        //    }
                        //    catch(Exception ex)
                        //    {
                        //        base.Log.log(string.Format("手机号{1}--收货确认短信发送失败{0}" + ex.Message,order.BuyerMobilePhone));

                        //    }
                        //}
                        //短信通知买家收款码结束
                        #endregion

                        order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Paid;

                        EStoreInfo store = db.DBStoreInfo.Where(s => s.ID == order.SellerStoreId).FirstOrDefault();
                        //家门店铺更新额度
                        store.RemainAmount -= order.TotalAmount;
                        if (store.RemainAmount < 0)
                            store.RecordStatus = RecordStatus.Blocked;

                        app = db.DBAliPayApp.Where(a => a.AppId == store.FromIQBAPP).FirstOrDefault();
                        //店铺佣金
                        if (!store.IsReceiveAccount)
                        {
                          //  Log.log("PayNotify 开始分账");
                            //分账
                            
                            if (app.AccountForSub!=store.AliPayAccount)
                            {
                                try
                                {
                                    NLogHelper.InfoTxt("开始分账..");
                                    AlipayTradeOrderSettleResponse res = payManager.DoSubAccount(app, order, store);
                                    if (res.Code != "10000")
                                    {
                                        order.LogRemark += string.Format("[SubAccount] SubCode:{0};Submsg:{1}; ", res.SubCode, res.SubMsg);
                                        order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Exception;
                                        TransferError++;
                                        store.RecordStatus = RecordStatus.Blocked;
                                        store.Log = string.Format("[分账错误]订单：{0}。时间：{1}", order.OrderNo, order.TransDateStr);
                                        db.SaveChanges();
                                        return View();
                                    }
                                    else
                                    {
                                        _ReportOrder.StoreOwnerRateAmount = order.SellerCommission;

                                        EUserStore us = db.DBUserStore.Where(a => a.OpenId == store.OwnnerOpenId).FirstOrDefault();

                                        if (us != null && us.OpenId != "o3nwE0i_Z9mpbZ22KdOTWeALXaus")
                                        {
                                            double amt = Convert.ToDouble((order.TotalAmount * us.Rate / 100).ToString("0.00"));
                                            UpdateUserBalance(db, store.OwnnerOpenId, amt, TransactionType.Store_Comm, ref _ReportOrder);

                                            string sql = string.Format(@"select ui.parentOpenId from UserInfo as ui
                                                                         inner join userstore as us on ui.parentOpenId = us.OpenId
                                                                         where ui.OpenId = '{0}'",
                                                                         us.OpenId);

                                            string pOpenId = db.Database.SqlQuery<string>(sql).FirstOrDefault();
                                            if (!string.IsNullOrEmpty(pOpenId))
                                            {
                                                amt = Convert.ToDouble((order.TotalAmount * us.FixComm / 100).ToString("0.00"));
                                                UpdateUserBalance(db, pOpenId, amt, TransactionType.Store_L2, ref _ReportOrder);

                                                sql = string.Format(@"select ui.parentOpenId from UserInfo as ui
                                                                         inner join userstore as us on ui.parentOpenId = us.OpenId
                                                                         where ui.OpenId = '{0}'",
                                                                         pOpenId);


                                                pOpenId = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                                                if (!string.IsNullOrEmpty(pOpenId))
                                                {
                                                    pOpenId = db.Database.SqlQuery<string>(sql).FirstOrDefault();
                                                    amt = Convert.ToDouble((order.TotalAmount * us.FixComm / 100).ToString("0.00"));
                                                    UpdateUserBalance(db, pOpenId, amt, TransactionType.Store_L3, ref _ReportOrder);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    order.LogRemark += ex.Message;
                                    store.RecordStatus = RecordStatus.Blocked;
                                    store.Remark += ex.Message;
                                }
                            }
                        }
                        string accessToken = this.getAccessToken(true);
                       
                       //代理
                        UpdateUserBalance(db, order.AgentOpenId, order.RateAmount,TransactionType.Agent_Order_Comm,ref _ReportOrder);
                        PPOrderPayNT notice = new PPOrderPayNT(accessToken, order.AgentOpenId, order);
                        notice.Push();

                        #region (原)代理打款
                        /*EUserInfo agentUI = db.DBUserInfo.Where(u => u.OpenId == order.AgentOpenId).FirstOrDefault();
                        tranfer = payManager.TransferHandler(TransferTarget.Agent, BaseController.SubApp, BaseController.SubApp, agentUI,ref order,0, accessToken,BaseController.GlobalConfig);
                        db.DBTransferAmount.Add(tranfer);
                        if(tranfer.TransferStatus != TransferStatus.Success)
                            TransferError++;
                        */
                        #endregion

                        //上级代理佣金
                        if (!string.IsNullOrEmpty(order.ParentOpenId) && order.ParentCommissionAmount > 0)
                        {

                            //agentComm = db.DBAgentCommission.Where(c => c.OrderNo == order.OrderNo && c.ParentOpenId == order.ParentOpenId && c.AgentCommissionStatus == AgentCommissionStatus.Open).FirstOrDefault();
                            //agentComm.AgentCommissionStatus = AgentCommissionStatus.Closed;

                            //EUserInfo parentUi = new EUserInfo();
                            //parentUi.AliPayAccount = agentComm.ParentAliPayAccount;

                            ////用户转账函数赋值
                            //parentUi.OpenId = agentComm.ParentOpenId;
                            //parentUi.Name = agentComm.ParentName;

                            UpdateUserBalance(db, order.ParentOpenId, order.ParentCommissionAmount,TransactionType.Parent_Comm, ref _ReportOrder);
                            //tranfer = payManager.TransferHandler(TransferTarget.ParentAgent, BaseController.SubApp, BaseController.SubApp, parentUi, ref order,0, null,BaseController.GlobalConfig);
                            //db.DBTransferAmount.Add(tranfer);
                            
                            //if(tranfer.TransferStatus == TransferStatus.Success)
                            //    agentComm.AgentCommissionStatus = AgentCommissionStatus.Closed;
                            //else
                            //    TransferError++;
                        }
                        //3级
                        if (!string.IsNullOrEmpty(order.L3OpenId) && order.L3CommissionAmount>0)
                        {
                            //agentComm = db.DBAgentCommission.Where(c => c.OrderNo == order.OrderNo && c.ParentOpenId == order.L3OpenId && c.AgentCommissionStatus == AgentCommissionStatus.Open).FirstOrDefault();
                            //agentComm.AgentCommissionStatus = AgentCommissionStatus.Closed;

                            //EUserInfo parentUi = new EUserInfo();
                            //parentUi.AliPayAccount = agentComm.ParentAliPayAccount;

                            ////用户转账函数赋值
                            //parentUi.OpenId = agentComm.ParentOpenId;
                            //parentUi.Name = agentComm.ParentName;

                            UpdateUserBalance(db, order.L3OpenId, order.L3CommissionAmount,TransactionType.L3_Comm, ref _ReportOrder);

                            //tranfer = payManager.TransferHandler(TransferTarget.L3Agent, BaseController.SubApp, BaseController.SubApp, parentUi, ref order,0, null, BaseController.GlobalConfig);
                            //db.DBTransferAmount.Add(tranfer);

                            //if (tranfer.TransferStatus == TransferStatus.Success)
                            //    agentComm.AgentCommissionStatus = AgentCommissionStatus.Closed;
                            //else
                            //    TransferError++;
                        }

                            //用户打款
                       //  Log.log("PayNotify 开始用户打款");
                        tranfer = payManager.TransferHandler(TransferTarget.User, BaseController.App, BaseController.SubApp,null, ref order,0, null, BaseController.GlobalConfig);
                        _ReportOrder.BuyerInCome = tranfer.TransferAmount;
                        _ReportOrder.BuyerPhone = order.BuyerMobilePhone;

                        db.DBTransferAmount.Add(tranfer);

                        if(tranfer.TransferStatus != TransferStatus.Success)
                            TransferError++;
                        if(TransferError>0)
                            order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Exception;
                        else
                            order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Closed;                  
                    }
                    else
                    {
                        //order.LogRemark += "支付状态为未成功！";
                        //order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Exception;
                        if (order.AliPayTradeStatus == "WAIT_BUYER_PAY")
                            return Content("success");
                        else
                        {
                            order.LogRemark += "通知状态异常：" + order.AliPayTradeStatus;
                            order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Exception;
                        }
                    }
                    _agentQR = db.DBQRUser.Where(a => a.ID == order.QRUserId).FirstOrDefault();

                    _ReportOrder.QRUserId = order.QRUserId.ToString();
                    _ReportOrder.QRType = _agentQR.QRType;
                    _ReportOrder.TransDate = order.TransDate;
                    _ReportOrder.OrderNo = order.OrderNo;
                    _ReportOrder.OrderAmount = order.TotalAmount;
                    _ReportOrder.CaluPPInCome();

                    db.DBReportOrder.Add(_ReportOrder);
                    /*
                    Agent  Policy
                     */
                //    AgentPolicy(db, _agentQR);

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                Log.log("PayNotify Error:" + ex.Message);
            }

            return View();
        }

        private void AgentPolicy(AliPayContent db,EQRUser agentQr)
        {
            const int AdjustAgentOrderNum = 10;

            int i= db.DBOrder.Where(a => a.AgentOpenId == agentQr.OpenId && a.OrderStatus == OrderStatus.Closed).Count();

            //如果超过10单，则费率调整
            //if(i> AdjustAgentOrderNum)
            //{
            //    float feeRate = 0;
            //    float jumpRate = 0;
            //    float minFeeRate = 0;
            //    //花呗
            //    if (agentQr.QRType == QRReceiveType.Small)
            //    {
            //        jumpRate = GlobalConfig.ChildFixRate;
            //        minFeeRate = GlobalConfig.HBMinFeeRate;
            //    }
            //    //信用卡
            //    else if (agentQr.QRType == QRReceiveType.CreditCard)
            //    {
            //        jumpRate = GlobalConfig.CCChildFixRate;
            //        minFeeRate = GlobalConfig.CCMinFeeRate;
            //    }

            //    agentQr.Rate += jumpRate;
            //    feeRate = agentQr.MarketRate - agentQr.Rate;
            //    if (feeRate < minFeeRate)
            //        agentQr.Rate = agentQr.MarketRate - minFeeRate;

            //}

        }

      

        /// <summary>
        /// 授权给商户
        /// </summary>
        /// <returns></returns>
        public ActionResult Auth()
        {
            string authCode = Request["app_auth_code"];
            string appId = Request["app_id"];
            string Id = Request["Id"];
            string StoreId = Request["StoreId"];
            long qrId;
            EQRStoreAuth qr = null;
            EStoreInfo store = null;
            EStoreInfo SelfStore = null;
          //  Log.log("Auth Code:"+authCode);
            EAliPayApplication app = null;
            AlipayOpenAuthTokenAppResponse response = null;
        //    NLogHelper.InfoTxt("StoreAuth Id:" + Id);
            if (!string.IsNullOrEmpty(authCode))
            {
                if (string.IsNullOrEmpty(Id) || !long.TryParse(Id, out qrId))
                {
                    NLogHelper.ErrorTxt("Auth No Id");
                
                    return Content("【传入的值不正确】无法授权，请联系平台");
                }

                try
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        qr = db.DBQRStoreAuth.Where(a => a.ID == qrId).FirstOrDefault();

                        if (qr == null)
                        {
                            return Content("【授权码不存在】无法授权，请联系平台！");
                        }
                        else if (qr.RecordStatus == IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked)
                            return Content("【授权码已被使用】无法授权，请联系平台！");

                        app = db.DBAliPayApp.Where(a => a.AppId == appId).FirstOrDefault();
                        if (app == null )
                        {
                            NLogHelper.ErrorTxt("Store Auth Error 【没有APP】无法授权或当前APP和授权码APP不一致");
                            return Content("【没有APP】无法授权或当前APP和授权码APP不一致，请联系平台");
                        }


                        IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
                        app.Merchant_Private_Key, "json", app.Version, app.SignType, app.Merchant_Public_key, "UTF-8", false);

                        AlipayOpenAuthTokenAppRequest request = new AlipayOpenAuthTokenAppRequest();

                        AlipayOpenAuthTokenAppModel model = new AlipayOpenAuthTokenAppModel();
                        model.GrantType = "authorization_code";
                        model.Code = authCode;

                        request.SetBizModel(model);
                       
                        response = alipayClient.Execute(request);
                        NLogHelper.InfoTxt(string.Format("调用支付宝，商户授权返回：" + response.Code));
                        if (response.Code == "10000")
                        {
                            if (qr.StoreId>0)
                                SelfStore = db.DBStoreInfo.Where(s => s.ID == qr.StoreId).FirstOrDefault();
                      
                            
                            store = db.Store_GetByAliPayUserId(response.UserId, appId);
                         
                            if (store == null)
                            {
                                if(SelfStore!=null)
                                {
                                    SelfStore.AliPayAccount = response.UserId;
                                    SelfStore.AliPayAuthAppId = response.AuthAppId;
                                    SelfStore.AliPayAuthToke = response.AppAuthToken;
                                    SelfStore.FromIQBAPP = qr.APPId;
                                    SelfStore.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.WaitingReview;

                                }
                                else
                                {
                                    store = new EStoreInfo
                                    {
                                        AliPayAccount = response.UserId,
                                        AliPayAuthAppId = response.AuthAppId,
                                        AliPayAuthToke = response.AppAuthToken,
                                        OwnnerOpenId = qr.OwnnerOpenId,
                                        Channel = qr.Channel,
                                        Name = qr.StoreName,
                                        Remark = qr.Remark,
                                        RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Normal,
                                        QRId = qr.ID,
                                        Rate = qr.Rate,
                                        FromIQBAPP = app.AppId,
                                        StoreType = qr.StoreType,
                                        MaxLimitAmount = qr.MaxLimitAmount,
                                        MinLimitAmount = qr.MinLimitAmount,
                                        RemainAmount = qr.RemainAmount,
                                        DayIncome = qr.DayIncome
                                    };
                                    if(store.Channel == Channel.League)
                                    {
                                        store.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.WaitingReview;
                                        EUserInfo ui = db.DBUserInfo.Where(u => u.OpenId == qr.OwnnerOpenId).FirstOrDefault();
                                        if(ui!= null)
                                            store.Provider = ui.Name.Length>30?ui.Name.Substring(0,30):ui.Name;
                                    }
                                    else if(store.Channel == Channel.PP)
                                    {
                                        store.Provider = "PP";
                                    }

                                    store.InitCreate();
                                    store.InitModify();
                                    db.DBStoreInfo.Add(store);
                                }

                               
                            }
                            else
                            {
                                if (SelfStore != null)
                                {
                                    if (store.ID != qr.StoreId)
                                    {
                                        var ErrorUrl = ConfigurationManager.AppSettings["IQBWX_SiteUrl"] + "/Home/ErrorMessage?code=2000&ErrorMsg=此授权码已被其他商户授权";
                                        return Redirect(ErrorUrl);
                                    }

                                    SelfStore.AliPayAccount = response.UserId;
                                    SelfStore.AliPayAuthAppId = response.AuthAppId;
                                    SelfStore.AliPayAuthToke = response.AppAuthToken;
                                    SelfStore.FromIQBAPP = qr.APPId;
                                    
                                }
                                else
                                {
                                    store.AliPayAccount = response.UserId;
                                    store.AliPayAuthAppId = response.AuthAppId;
                                    store.AliPayAuthToke = response.AppAuthToken;
                                    store.FromIQBAPP = app.AppId;
                                    store.OwnnerOpenId = qr.OwnnerOpenId;
                                    store.Channel = qr.Channel;
                                    store.Remark = qr.Remark;
                                    store.QRId = qr.ID;
                                    store.Rate = qr.Rate;
                                    store.StoreType = qr.StoreType;

                                    store.MaxLimitAmount = qr.MaxLimitAmount;
                                    store.MinLimitAmount = qr.MinLimitAmount;
                                    store.RemainAmount = qr.RemainAmount;
                                    store.DayIncome = qr.DayIncome;

                                    if (store.Channel == Channel.League)
                                    {
                                        store.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.WaitingReview;
                                        EUserInfo ui = db.DBUserInfo.Where(u => u.OpenId == qr.OwnnerOpenId).FirstOrDefault();
                                        if (ui != null)
                                            store.Provider = ui.Name.Length > 30 ? ui.Name.Substring(0, 30) : ui.Name;
                                    }
                                    else if (store.Channel == Channel.PP)
                                    {
                                        store.Provider = "PP";
                                    }

                                }
                              
                            }
                            if(qr.StoreId ==0)
                                qr.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked;
                           
                            db.SaveChanges();
                        }
                        else
                        {
                            NLogHelper.ErrorTxt("支付宝返回商户授权失败：" + response.Code);
                        
                            return Content("授权失败："+response.Msg);
                        }

                    }
                    string url = "";
                    if (qr.StoreId>0)
                        url = ConfigurationManager.AppSettings["IQBWX_SiteUrl"]+"/PP/Auth_Store?Rate="+SelfStore.Rate;
                    else
                        url = ConfigurationManager.AppSettings["IQBWX_SiteUrl"] + "/PP/Auth_Store?Rate=" + store.Rate;

                    NLogHelper.InfoTxt(url);
                    return Redirect(url);
                }
                catch (Exception ex)
                {
                    NLogHelper.ErrorTxt("商户授权错误："+ex.Message);
                    return View();

                }

            }
            else
            {
                return Content("No Auth Code");
            }

          
        }

        public ActionResult QueryToken()
        {
            return Content(AliDemo.callQuery_AuthToke());
        }

        public ActionResult Pay()
        {
            string amt = Request.QueryString["amount"];
            return Content(AliDemo.callAliPay_Wap(amt));
        }

        public ActionResult TestF2F(string No,long storeId,string Amount)
        {
            using (AliPayContent db = new AliPayContent())
            {
                EStoreInfo store = db.DBStoreInfo.Where(s => s.ID == storeId).FirstOrDefault();
                return Content(callF2FPay(store, Amount, No));
            }
        }

        public ActionResult Demo()
        {
           
            return View();
        }

       

        public ActionResult QRImg()
        {
           return Content(AliDemo.callQRImg());
        }

         public ActionResult QRUserImg()
        {
            //EQRUser qrUser = new EQRUser();
            //using (AliPayContent db = new AliPayContent())
            //{
            //   qrUser = db.DBQRUser.Where(a => a.ID == 12).FirstOrDefault();
            //  ViewBag.FP =   QRManager.CreateUserUrlById(qrUser).FilePath;
            //}
            return View();
        }

        public ActionResult Note()
        {
            //string accessToken = this.getAccessToken(true);

            //PPInviteCodeNT notice = null;
            //try
            //{
            //    notice = new PPInviteCodeNT(accessToken, 10, 20, "o3nwE0qI_cOkirmh_qbGGG-5G6B0");
            //    return Content(notice.Push());

            //}
            //catch
            //{

            //}
            return View();
        }

        public ActionResult QRImgWX()
        {
            string res = "";
            try
            {
                string url = "http://wx.iqianba.cn/api/wx/CreatePayQRAuth";
                string data = "QRId=13&QRType=1";
                res= HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, data, "application/x-www-form-urlencoded");
                SSOQR obj = JsonConvert.DeserializeObject<SSOQR>(res);
                 url = obj.TargetUrl;

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Content(res);
        }

        private void UpdateQRHuge(AliPayContent db,long QRHugeTransId)
        {
            RQRHugeTrans qrTrans = db.DBQRHugeTrans.Select(s => new RQRHugeTrans
            {
                QRHugeId = s.QRHugeId,
                ID = s.ID,
            }).Where(o=>o.ID == QRHugeTransId).First();

            EQRHugeTrans EQRHugeTrans = new EQRHugeTrans();

            EQRHugeTrans.ID = QRHugeTransId;
            EQRHugeTrans.TransStatus = QRHugeTransStatus.Closed;

            DbEntityEntry<EQRHugeTrans> entryTrans = db.Entry<EQRHugeTrans>(EQRHugeTrans);
            entryTrans.State = EntityState.Unchanged;
            entryTrans.Property(t => t.TransStatus).IsModified = true;

            EQRHuge EQRHuge = new EQRHuge();
            EQRHuge.ID = qrTrans.QRHugeId;
            EQRHuge.QRHugeStatus = QRHugeStatus.Closed;
           
            DbEntityEntry<EQRHuge> entryHuge = db.Entry<EQRHuge>(EQRHuge);
            entryHuge.State = EntityState.Unchanged;
            entryHuge.Property(t => t.QRHugeStatus).IsModified = true;
        }

        public ActionResult F2FHugePay(string rQRHugeId, string AliPayAccount = "")
        {
            string ErrorUrl = ConfigurationManager.AppSettings["IQBWX_SiteUrl"] + "Home/ErrorMessage?QRHugeId=" + rQRHugeId + "&code=2003&ErrorMsg=";
            long qrHugeId;
            if (string.IsNullOrEmpty(rQRHugeId) || !long.TryParse(rQRHugeId, out qrHugeId))
            {
                ErrorUrl = ConfigurationManager.AppSettings["IQBWX_SiteUrl"] + "Home/ErrorMessage?code=2000&ErrorMsg=";
                ErrorUrl += "二维码不存在，请咨询您的联系人！";
                return Redirect(ErrorUrl);
            }
            if(string.IsNullOrEmpty(AliPayAccount))
            {
                ErrorUrl += "请填写收款账户！";
                return Redirect(ErrorUrl);
            }
            if (IsBlockUser(AliPayAccount))
            {
                return Redirect("https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=%E6%88%91%E9%94%99%E4%BA%86&oq=%25E6%2588%2591%25E8%25A6%2581%25E5%25A4%25A7%25E9%25A2%259D&rsv_pq=fdc9a37600011847&rsv_t=12bbtVESTOTvr2Vka6q3RaIFGkTv3u3HZMssQ3J0JYuez4Fx0Cqqzvj%2BxqM&rqlang=cn&rsv_enter=1&inputT=13704&rsv_sug3=49&rsv_sug1=46&rsv_sug7=100&bs=%E6%88%91%E8%A6%81%E5%A4%A7%E9%A2%9D");
            }

            EAliPayApplication app;
            try
            {
               
                AliPayManager payMag = new AliPayManager();
                EQRUser qrUser = null;
                EUserInfo ui = null;
                EQRHuge qrHuge = null;
                long Id;
                
                using (AliPayContent db = new AliPayContent())
                {
                    qrHuge = db.DBQRHuge.Where(o => o.ID == qrHugeId && o.QRHugeStatus == QRHugeStatus.Created).FirstOrDefault();
                    if(qrHuge == null)
                    {
                        ErrorUrl += "二维码已更新，请您重新索要！";
                        return Redirect(ErrorUrl);
                    }
                    if(qrHuge.PayCount>=2)
                    {
                        qrHuge.QRHugeStatus = QRHugeStatus.InValid;
                        db.SaveChanges();

                        ErrorUrl += "请一次性扫码完成支付，此码已失效，请您重新索要！";
                        return Redirect(ErrorUrl);
                    }
                   
                    //校验代理二维码
                    qrUser = db.DBQRUser.Where(q => q.OpenId == qrHuge.OpenId && q.QRType == QRReceiveType.Huge && q.RecordStatus == RecordStatus.Normal).FirstOrDefault();

                    if (qrUser == null)
                    {
                        ErrorUrl += "中介费率配置未找到，请联系您的中介";
                        return Redirect(ErrorUrl);
                    }
                    if (qrUser.MarketRate <= 0)
                    {
                        ErrorUrl += "市场折扣点配置错误，请联系代理";
                        return Redirect(ErrorUrl);
                    }
                    //检验代理人
                    ui = db.DBUserInfo.Where(u => u.OpenId == qrUser.OpenId).FirstOrDefault();
                    if (ui == null)
                    {
                        ErrorUrl += "未找到收款二维码代理人";
                        return Redirect(ErrorUrl);
                    }
                    if (string.IsNullOrEmpty(ui.OpenId))
                    {
                        ErrorUrl += "收款二维码代理人微信号没有找到";
                        return Redirect(ErrorUrl);
                    }
                    if (ui.UserStatus == IQBCore.IQBPay.BaseEnum.UserStatus.JustRegister)
                    {
                        ErrorUrl += "代理被禁用,您无法支付！";
                        return Redirect(ErrorUrl);
                    }
                    if (string.IsNullOrEmpty(ui.AliPayAccount))
                    {
                        ErrorUrl += "您的联系人没有设置支付宝账户";
                        return Redirect(ErrorUrl);
                    }


                    EStoreInfo store = null;
                    string selectStoreSql = string.Format(@"select top 1 * from StoreInfo 
                                            where RecordStatus = 0 and StoreType = 4 and RemainAmount> 0 and MinLimitAmount<={0} and MaxLimitAmount>={0}
                                            order by NEWID()", qrHuge.Amount);

                    store = db.Database.SqlQuery<EStoreInfo>(selectStoreSql).FirstOrDefault();
                    if (store == null)
                    {
                        ErrorUrl += "抱歉，大额支付今天已经用完，清明天再来或去普通区支付";
                        //关闭大额入口

                        EGlobalConfig gc = db.DBGlobalConfig.First();
                        gc.QRHugeEntry = QRHugeEntry.Stop;
                        db.SaveChanges();

                        BaseController.GlobalConfig = gc;

                        //通知WX客户端
                        string url = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
                        url += "API/OutData/RefreshGlobelConfig";
                        HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, "", "application/x-www-form-urlencoded");


                        return Redirect(ErrorUrl);
                    }
                    //获取并校验商户 
                    AliPayResult status;

                    if (store.FromIQBAPP == BaseController.App.AppId)
                        app = BaseController.App;
                    else if (store.FromIQBAPP == BaseController.SubApp.AppId)
                        app = BaseController.SubApp;
                    else
                    {
                        ErrorUrl += "商户所属APP没有设置正确";
                        return Redirect(ErrorUrl);
                    }

                    //测试转账
                    if (!string.IsNullOrEmpty(AliPayAccount))
                    {
                        EBuyerInfo buyer = db.DBBuyerInfo.Where(a => a.AliPayAccount == AliPayAccount).FirstOrDefault();
                        if (buyer == null)
                        {
                            string tId;
                            AlipayFundTransToaccountTransferResponse res = payMag.DoTransferAmount(TransferTarget.User, BaseController.SubApp, AliPayAccount, "0.1", PayTargetMode.AliPayAccount, out tId);
                            if (res.Code != "10000")
                            {
                                ErrorUrl += "您输入的收款账户,无法转账,如果您输入的是手机账户，请尝试使用对应的邮箱账户！";
                                return Redirect(ErrorUrl);
                            }
                            else
                            {
                                buyer = new EBuyerInfo();
                                buyer.AliPayAccount = AliPayAccount;
                                buyer.TransDate = DateTime.Now;
                                db.DBBuyerInfo.Add(buyer);
                                db.SaveChanges();
                            }
                        }
                    }
                    DateTime startDate = DateTime.Today;
                    DateTime endDate = DateTime.Today.AddDays(1);

                    //每个用户一天只能交易3次
                    int transCount = db.DBQRHugeTrans.Where(o => o.UserAliPayAccount == AliPayAccount
                                                                   && o.TransStatus == QRHugeTransStatus.Closed
                                                                   && o.CreatedDate >= startDate
                                                                   && o.CreatedDate <= endDate).Count();
                    if(transCount>=3)
                    {
                        ErrorUrl += "您今天大额使用次数已满，请明天再使用，谢谢光临";

                        return Redirect(ErrorUrl);
                    }

                    //  string Res = payMag.PayF2F(app, ui, store, Convert.ToSingle(Amount),out status);
                    string Res = payMag.PayF2FNew(app, ui, store, qrHuge.Amount.ToString("0.00"), out status);
                    // base.Log.log("支付PayF2F：" + Res);
                    if (status == AliPayResult.SUCCESS)
                    {

                        EQRHugeTrans QRHugeTrans = EQRHugeTrans.Init(qrHuge, AliPayAccount);
                        db.DBQRHugeTrans.Add(QRHugeTrans);
                        qrHuge.PayCount++;
                        db.SaveChanges();

                        EUserStore us = db.DBUserStore.Where(a => a.OpenId == store.OwnnerOpenId).FirstOrDefault();
                        
                        //创建初始化订单
                        EOrderInfo order = payMag.InitOrder(qrUser, store,us, qrHuge.Amount,OrderType.Huge, AliPayAccount,1, ui,QRHugeTrans);
                      
                        if (!string.IsNullOrEmpty(qrUser.ParentOpenId))
                        {
                            EAgentCommission agentComm = payMag.InitAgentCommission(order, qrUser);

                            RUserInfo parentUi = db.DBUserInfo.Where(u => u.OpenId == qrUser.ParentOpenId).Select(a => new RUserInfo()
                            {
                                OpenId = a.OpenId,
                                ParentAgentOpenId = a.parentOpenId,
                                AliPayAccount = a.AliPayAccount,
                                NeedFollowUp = a.NeedFollowUp,

                            }).FirstOrDefault();

                            agentComm.ParentAliPayAccount = parentUi.AliPayAccount;

                            db.DBAgentCommission.Add(agentComm);

                            order.ParentOpenId = qrUser.ParentOpenId;
                            order.ParentCommissionAmount = agentComm.CommissionAmount;

                            //3级
                            if (parentUi.NeedFollowUp)
                            {
                                RUserInfo L3Parent = db.DBUserInfo.Where(u => u.OpenId == parentUi.ParentAgentOpenId).Select(a => new RUserInfo()
                                {
                                    OpenId = a.OpenId,
                                    Name = a.Name,
                                    AliPayAccount = a.AliPayAccount,
                                }).FirstOrDefault();
                                if (L3Parent != null)
                                {
                                    agentComm = payMag.InitAgentCommission_L3(order, qrUser, L3Parent);

                                    db.DBAgentCommission.Add(agentComm);

                                    order.L3OpenId = L3Parent.OpenId;
                                    order.L3CommissionAmount = agentComm.CommissionAmount;
                                }
                            }
                        }

                        db.DBOrder.Add(order);

                        //初始化佣金信息，如果订单未支付，将成为脏数据。

                        //买家信息记录
                        /*
                        EBuyerInfo buyInfo = new EBuyerInfo();
                        buyInfo.LastTransDate = DateTime.Now;
                        buyInfo.PhoneNumber = Phone;
                        db.DBBuyerInfo.Add(buyInfo);
                        */
                        db.SaveChanges();

                        return Redirect(Res);
                    }
                    else
                    {

                        ErrorUrl += "商户下架，抱歉，请重新扫下支付码";
                        store.RecordStatus = RecordStatus.Blocked;
                        store.Log = string.Format("[{0}][Error]商户授权出错", DateTime.Now.ToShortDateString());
                       
                        db.SaveChanges();
                        return Redirect(ErrorUrl);
                    }

                }
            }
            catch (Exception ex)
            {


                base.Log.log("支付失败：" + ex.Message);
                ErrorUrl += "支付失败：" + ex.Message;
                return Redirect(ErrorUrl);
            }



            return View();
        }
       

        private bool IsBlockUser(string AliPayAccount)
        {
           return  _BlockList.Contains(AliPayAccount);

        }

        /// <summary>
        /// 0-OK 1-速度太快
        /// </summary>
        /// <param name="AliPayAccount"></param>
        /// <returns></returns>
        private int VerifyUserAction(string AliPayAccount)
        {
            long ticks = (long)Session[IQBConstant.SK_UserPayTime];
            if (ticks > 0)
            {
                int sec = DateHelper.GetDiffSecWithNow_Ticks(ticks);
                if(sec<15)
                {
                    return 1;
                }
                else
                    Session[IQBConstant.SK_UserPayTime] = ticks;
            }
            else
                Session[IQBConstant.SK_UserPayTime] = ticks;

            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">QRUserId</param>
        /// <param name="Amount"></param>
        /// <returns></returns>   
        public ActionResult F2FPay(string qrUserId, string Amount,string BuyerPhone="",string AliPayAccount="",string TestStoreId="")
        {
            string ErrorUrl = ConfigurationManager.AppSettings["IQBWX_SiteUrl"] + "Home/ErrorMessage?QRUserId="+qrUserId+"&code=2001&ErrorMsg=";
            EAliPayApplication app;
            try
            {
               // base.Log.log(string.Format("start F2FPay:qrUserId {0} Amount{1} ReceiveNo:{2}",qrUserId,Amount,));
                AliPayManager payMag = new AliPayManager();
                EQRUser qrUser = null;
                EUserInfo ui = null;
             //   EQRInfo qrInfo = null;
                long Id;
                if (string.IsNullOrEmpty(qrUserId) || !long.TryParse(qrUserId, out Id))
                {
                    ErrorUrl += "二维码已更新，请向代理索要最新当前二维码";
                    return Redirect(ErrorUrl);
                }
                //int userAction = VerifyUserAction(AliPayAccount);
                //if (userAction==1)
                //{
                //    ErrorUrl += "您刷的太快，请等几秒后再尝试";
                //    return Redirect(ErrorUrl);
                //}
                //黑名单
                if (IsBlockUser(AliPayAccount))
                {
                    return Redirect("https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=%E6%88%91%E9%94%99%E4%BA%86&oq=%25E6%2588%2591%25E8%25A6%2581%25E5%25A4%25A7%25E9%25A2%259D&rsv_pq=fdc9a37600011847&rsv_t=12bbtVESTOTvr2Vka6q3RaIFGkTv3u3HZMssQ3J0JYuez4Fx0Cqqzvj%2BxqM&rqlang=cn&rsv_enter=1&inputT=13704&rsv_sug3=49&rsv_sug1=46&rsv_sug7=100&bs=%E6%88%91%E8%A6%81%E5%A4%A7%E9%A2%9D");
                }
                using (AliPayContent db = new AliPayContent())
                {
                    //校验代理二维码
                    qrUser = db.DBQRUser.Where(q => q.ID == Id).FirstOrDefault();
                    if (qrUser == null)
                    {
                        ErrorUrl += "请重新扫码进去，如果还是有问题，请联系您的联系人";
                        return Redirect(ErrorUrl);
                    }
                    if(qrUser.MarketRate <=0)
                    {
                        ErrorUrl += "市场折扣点配置错误，请联系代理";
                        return Redirect(ErrorUrl);
                    }
                    //检验代理人
                    ui = db.DBUserInfo.Where(u => u.OpenId == qrUser.OpenId).FirstOrDefault();
                    if(ui == null)
                    {
                        ErrorUrl += "未找到收款二维码代理人";
                        return Redirect(ErrorUrl);
                    }
                    if (string.IsNullOrEmpty(ui.OpenId))
                    {
                        ErrorUrl += "收款二维码代理人微信号没有找到";
                        return Redirect(ErrorUrl);
                    }
                    //被禁用的用户，手续费调整为8
                    if (ui.UserStatus == IQBCore.IQBPay.BaseEnum.UserStatus.JustRegister)
                    {

                        ErrorUrl += "您的联系人被禁用,无法支付！";
                        return Redirect(ErrorUrl);
                    }

                    if (string.IsNullOrEmpty(ui.AliPayAccount))
                    {
                        ErrorUrl += "您的联系人没有设置支付宝账户！";
                        return Redirect(ErrorUrl);
                    }
                    //根据支付通道选择APP
                    if (qrUser.QRType == QRReceiveType.CreditCard)
                        app = BaseController.App;
                    else
                    {
                        app = db.DBAliPayApp.Where(a => a.SupportHuaBei == true).FirstOrDefault();
                    }
                    EStoreInfo store = null;
                    string selectStoreSql = string.Format(@"select top 1 * from StoreInfo 
                                            where RecordStatus = 0 and RemainAmount> 0 and StoreType={1} 
                                            and MinLimitAmount<={0} and MaxLimitAmount>={0}
                                            and FromIQBAPP = '{2}'
                                            order by NEWID()", Amount,(int)qrUser.QRType, app.AppId);

                    NLogHelper.InfoTxt("Store Select Sql:" + selectStoreSql);

                    //测试代码
                    if (!string.IsNullOrEmpty(TestStoreId))
                    {
                        long sId = Convert.ToInt64(TestStoreId);
                        store = db.DBStoreInfo.Where(o=>o.ID == sId).FirstOrDefault();
                        app = db.DBAliPayApp.Where(a => a.AppId == store.FromIQBAPP).FirstOrDefault();
                    }
                    else
                    {
                        if (qrUser.ReceiveStoreId > 0)
                        {
                            store = db.DBStoreInfo.Where(a => a.ID == qrUser.ReceiveStoreId).FirstOrDefault();
                            if (store == null)
                            {
                                ErrorUrl += "没有找到对应的收款商户";
                                return Redirect(ErrorUrl);
                            }
                            if (store.RecordStatus == IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked)
                            {
                                ErrorUrl += "收款商户已下线";
                                return Redirect(ErrorUrl);
                            }

                            //if(store.IsReceiveAccount)
                            //{
                            //    ErrorUrl += "支付的商户不能作为收款商户";
                            //    return Redirect(ErrorUrl);
                            //}
                        }
                        else
                        {

                            store = db.Database.SqlQuery<EStoreInfo>(selectStoreSql).FirstOrDefault();
                        }
                    }
                   

                    if (store == null)
                    {
                   //     ErrorUrl += "";
                        ErrorUrl = ConfigurationManager.AppSettings["IQBWX_SiteUrl"] + "Home/ErrorMessage?code=2001&ErrorMsg=暂时没有找到可用的商户！请尝试其他通道";
                        ErrorUrl += "&backUrl=/PP/PaySelection?Id="+qrUser.OpenId;

                        return Redirect(ErrorUrl);
                    }
                    //获取并校验商户 
                    AliPayResult status;

                  
                    /*
                    if (store.FromIQBAPP == BaseController.App.AppId)
                        app = BaseController.App;
                    else if (store.FromIQBAPP == BaseController.SubApp.AppId)
                        app = BaseController.SubApp;
                    else
                    {
                        ErrorUrl += "商户所属APP没有设置正确";
                        return Redirect(ErrorUrl);
                    }
                    */
                    #region 测试转账
                    //测试转账
                    if (!string.IsNullOrEmpty(AliPayAccount))
                    {
                        EBuyerInfo buyer = db.DBBuyerInfo.Where(a => a.AliPayAccount == AliPayAccount).FirstOrDefault();
                        if (buyer == null)
                        {
                            string tId;
                            EOrderInfo testOrder = new EOrderInfo();
                            testOrder.AgentName = ui.Name;
                            testOrder.OrderNo = "T" + DateTime.Now.Ticks;
                            AlipayFundTransToaccountTransferResponse res = payMag.DoTransferAmount(TransferTarget.User, BaseController.SubApp, AliPayAccount, "0.1", PayTargetMode.AliPayAccount, out tId, testOrder);
                            if (res.Code != "10000")
                            {
                                ErrorUrl += "您输入的收款账户,无法转账,如果您输入的是手机账户，请尝试使用对应的邮箱账户！";
                                return Redirect(ErrorUrl);
                            }
                            else
                            {
                                buyer = new EBuyerInfo();
                                buyer.AliPayAccount = AliPayAccount;
                                buyer.TransDate = DateTime.Now;
                                db.DBBuyerInfo.Add(buyer);
                                db.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    string Res = payMag.PayF2FNew(app, ui, store, Amount, out status);
                                
                    if (status == AliPayResult.SUCCESS)
                    {
                        //    int ordernum = db.DBOrder.Where(a => a.OrderStatus == OrderStatus.Closed && a.AgentOpenId == qrUser.OpenId).Count();
                        int ordernum =0;
                        if (ui.HasPassRegFee)
                            ordernum = 1;
                        EUserStore us = db.DBUserStore.Where(a => a.OpenId == store.OwnnerOpenId).FirstOrDefault();
                        if(us == null)
                        {
                            string sql = @"select us.* from UserInfo as ui
                                        join UserStore as us on ui.OpenId = us.OpenId
                                        where ui.UserRole = 100";
                            us = db.Database.SqlQuery<EUserStore>(sql).FirstOrDefault();
                        }
                        //创建初始化订单
                        EOrderInfo order = payMag.InitOrder(qrUser, store,us,Convert.ToSingle(Amount),OrderType.Normal, AliPayAccount, ordernum,ui);
                        if (!string.IsNullOrEmpty(BuyerPhone))
                            order.BuyerMobilePhone = BuyerPhone;

                        if (ui.UserRole != UserRole.DiamondAgent)
                        {
                            if (!string.IsNullOrEmpty(qrUser.ParentOpenId) && qrUser.ParentOpenId!= "o3nwE0i_Z9mpbZ22KdOTWeALXaus")
                            {
                               
                                order.ParentOpenId = qrUser.ParentOpenId;
                                if(qrUser.QRType == QRReceiveType.Small)
                                    order.ParentCommissionAmount = Convert.ToSingle((order.TotalAmount* GlobalConfig.ChildFixRate/100).ToString("0.00"));
                                else
                                    order.ParentCommissionAmount = Convert.ToSingle((order.TotalAmount * GlobalConfig.CCChildFixRate/100).ToString("0.00"));

                                string sql = string.Format("select parentOpenId from userinfo where openId='{0}'", qrUser.ParentOpenId);
                                string L3OpenId =  db.Database.SqlQuery<string>(sql).FirstOrDefault();
                                if(!string.IsNullOrEmpty(L3OpenId) && L3OpenId != "o3nwE0i_Z9mpbZ22KdOTWeALXaus")
                                {
                                    order.L3OpenId = L3OpenId;
                                    if (qrUser.QRType == QRReceiveType.Small)
                                        order.L3CommissionAmount = Convert.ToSingle((order.TotalAmount * GlobalConfig.ChildFixRate/100).ToString("0.00"));
                                    else
                                        order.L3CommissionAmount = Convert.ToSingle((order.TotalAmount * GlobalConfig.CCChildFixRate/100).ToString("0.00"));
                                }

                            }
                        }

                        db.DBOrder.Add(order);
                        
                        db.SaveChanges();

                        return Redirect(Res);
                    }
                    else
                    {


                        store.Log = string.Format("[{0}][Error]商户出错-{1}", DateTime.Now.ToShortDateString(), Res);
                        store.RecordStatus = RecordStatus.Blocked;
                        db.SaveChanges();

                        ErrorUrl += "商户下架，抱歉，请重新扫下支付码";
                        return Redirect(ErrorUrl);
                    }

                }
            }
            catch(Exception ex)
            {
                
             
                base.Log.log("支付失败：" + ex.Message);
                ErrorUrl += "支付失败："+ex.Message;
                return Redirect(ErrorUrl);
            }


        
            return View();
        }

        public ActionResult AuthToISV()
        {
            return View();
        }

        public ActionResult GetAuthToken()
        {
          
            return Content(AliDemo.GetAuthToken());
        }

        public ActionResult PP()
        {
            IQBLog Log = new IQBLog();
            return View();
          //  return Redirect("https://qr.alipay.com/stx04233mlnkwff4e7sble7");
        }

        public ActionResult TestUrl()
        {
            
            string result = Request.QueryString["goto"];
            result = HttpHelper.RequestUrlSendMsg(result, HttpHelper.HttpMethod.Post, "", "text/html");
            return Content(result);
        }

        public ActionResult SubAccount(string OrderNo)
        {
            AlipayTradeOrderSettleResponse res = null;
            using (AliPayContent db = new AliPayContent())
            {
                AliPayManager payManager = new AliPayManager();
                EOrderInfo order = db.DBOrder.Where(o => o.OrderNo == OrderNo).FirstOrDefault();
                EStoreInfo store = db.DBStoreInfo.Where(s => s.ID == order.SellerStoreId).FirstOrDefault();
                EAliPayApplication app = db.DBAliPayApp.Where(a => a.AppId == "2018082261169057").FirstOrDefault();

                string subAccount = "2088131464918766";

                IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,

              app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);


                AlipayTradeOrderSettleRequest request = new AlipayTradeOrderSettleRequest();

                //   string commission = (order.TotalAmount*(100-0.38)/100).ToString("0.00");
                string commission = (order.TotalAmount - order.SellerCommission).ToString("0.00");
                // string commission = "47.00";
                request.BizContent = "{" +
                "\"out_request_no\":\"" + StringHelper.GenerateSubAccountTransNo() + "\"," +
                "\"trade_no\":\"" + order.AliPayOrderNo + "\"," +
                "\"royalty_parameters\":[{" +
                "\"trans_out\":\"" + store.AliPayAccount + "\"," +
                "\"trans_in\":\"" + subAccount + "\"," +
                "\"amount\":" + commission + "," +
                "\"desc\":\"分账\"" +
                "}]" +
                //"\"operator_id\":" +
                "}";

                res = aliyapClient.Execute(request, null, store.AliPayAuthToke);
                

            }
            return Content(res.Body);
               
        }

        public ActionResult TransferTest()
        {
            AliPayManager payManager = new AliPayManager();
            string tid; 
            AlipayFundTransToaccountTransferResponse response =  payManager.DoTransferAmount(TransferTarget.Internal,BaseController.SubApp, "song_fuwei@hotmail.com", 0.1.ToString("0.00"),PayTargetMode.AliPayAccount,out tid);

            return Content(response.Body);
           // return View();
        }

        public ActionResult PCPay()
        {

            string amt = Request.QueryString["amount"];
            return Content(AliDemo.callAliPay_PC(amt));
        }

        public ActionResult QueryOrder()
        {
            //EAliPayApplication app = BaseController.App;
            //IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
            // app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);

            //AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            //AlipayTradeQueryModel model = new AlipayTradeQueryModel();

            //model.OutTradeNo = "YJO20180109111128wo";
            //request.SetBizModel(model);

            //AlipayTradeQueryResponse response = client.execute(request);
            //Console.WriteLine(response.Body);
            return View();
        }

        [HttpPost]
        public ActionResult PartPayQR()
        {
            try
            {
                AlipayTradeOrderSettleResponse res = null;
                using (AliPayContent db = new AliPayContent())
                {
                    AliPayManager payManager = new AliPayManager();
                    EUserInfo ui = new EUserInfo();
                    ui.Name = "分期";
                    EStoreInfo store = db.DBStoreInfo.Where(s => s.IsReceiveAccount).FirstOrDefault();
                    AliPayResult status;
                    string imgPath = payManager.PartPayQR(BaseController.App, ui, store, "5100", out status);
                    return Content(imgPath);
                    //  res = payManager.DoSubAccount(BaseController.App, order, store, BaseController.SubAccount);

                }
            }
            catch(Exception ex)
            {
                return Content("error");
            }
           
           
        }



    }
}