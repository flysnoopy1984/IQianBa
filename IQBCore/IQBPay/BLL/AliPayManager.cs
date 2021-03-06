﻿using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.Sys;
using IQBCore.IQBPay.Models.Tool;
using IQBCore.IQBPay.Models.User;
using IQBCore.IQBWX.Models.WX.Template;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IQBCore.IQBPay.BLL
{   
    public class AliPayManager
    {
        private F2FPayHandler _handler =null;

        public AlipayTradeOrderSettleResponse DoSubAccount(EAliPayApplication app,EOrderInfo order,EStoreInfo store)
        {
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
              app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);


            AlipayTradeOrderSettleRequest request = new AlipayTradeOrderSettleRequest();

         //   string commission = (order.TotalAmount*(100-0.38)/100).ToString("0.00");
            string commission = (order.TotalAmount - order.SellerCommission).ToString("0.00");
        // string commission = "47.00";
            request.BizContent = "{" +
            "\"out_request_no\":\"" + StringHelper.GenerateSubAccountTransNo() + "\"," +
            "\"trade_no\":\""+order.AliPayOrderNo+"\"," +
            "\"royalty_parameters\":[{" +
            "\"trans_out\":\""+ store.AliPayAccount + "\"," + 
            "\"trans_in\":\"" + app.AccountForSub + "\"," +
            "\"amount\":"+ commission + "," +
            "\"desc\":\"分账\"" +
            "}]" +
            //"\"operator_id\":" +
            "}";

            AlipayTradeOrderSettleResponse response = aliyapClient.Execute(request,null, store.AliPayAuthToke);
            return response;
        }
       
        public void NoticeToAgentForOrder(string accessToken,string openId,EOrderInfo order)
        {
            try
            {

                if (!string.IsNullOrEmpty(accessToken))
                {
                    PPOrderPayNT notice = new PPOrderPayNT(accessToken, openId, order);
                  
                    notice.Push();
                }

            }
            catch (Exception ex)
            {
                // log.log("TransferHandler WX note Error:"+ex.Message);
            }
        }
       

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="target">转帐方向</param>
        /// <param name="app">支付宝App</param>
        /// <param name="ui">转账对象</param>
        /// <param name="order">根据订单获取转账金额</param>
        /// <param name="accessToken"></param>
        /// <param name="GlobalConfig">获取是否微信转账配置</param>
        /// <returns></returns>
        public ETransferAmount TransferHandler(TransferTarget target,EAliPayApplication app, EAliPayApplication subApp,EUserInfo ui,ref EOrderInfo order,float AmountNotInOrder=0, string accessToken=null,EGlobalConfig GlobalConfig =null)
        {
            string TransferId ="";
            ETransferAmount transfer = null;
            AlipayFundTransToaccountTransferResponse res = null;
            string AliPayAccount = null;
            float TransferAmount = 0;
            PayTargetMode PayTargetMode = PayTargetMode.AliPayAccount;
            switch (target)
            {
                case TransferTarget.Agent:
                    AliPayAccount = ui.AliPayAccount;
                    TransferAmount = order.RateAmount;
                    break;
                case TransferTarget.L3Agent:
                    AliPayAccount = ui.AliPayAccount;
                    TransferAmount = order.L3CommissionAmount;
                    break;
                case TransferTarget.ParentAgent:
                    AliPayAccount = ui.AliPayAccount;
                    TransferAmount = order.ParentCommissionAmount;
                    break;
                case TransferTarget.User:
                    if (string.IsNullOrEmpty(order.BuyerAliPayAccount))
                    {
                        AliPayAccount = order.BuyerAliPayId;
                        PayTargetMode = PayTargetMode.AliPayId;
                    }
                    else
                        AliPayAccount = order.BuyerAliPayAccount;

                    TransferAmount = order.BuyerTransferAmount;
                    break;
                case TransferTarget.MidStore:
                    AliPayAccount = ui.AliPayAccount;
                    TransferAmount = AmountNotInOrder;
                    break;
            }
            if(target == TransferTarget.User)
            {
                res = DoTransferAmount(target, subApp, AliPayAccount, TransferAmount.ToString("0.00"), PayTargetMode, out TransferId, order);
                //if (res.Code == "40004" && res.SubCode == "PAYER_BALANCE_NOT_ENOUGH")
                //{
                //    string tid;
                //    Random r = new Random();
                //    int num = r.Next(11890, 15588);
                //    AlipayFundTransToaccountTransferResponse response = DoTransferAmount(TransferTarget.Internal,app, "hanyiadmin@126.com", num.ToString("0.00"), PayTargetMode.AliPayAccount, out tid);
                //    if(response.Code == "10000")
                //    {
                //        res = DoTransferAmount(target, subApp, AliPayAccount, TransferAmount.ToString("0.00"), PayTargetMode, out TransferId, order);
                //    }
                //}
            }
            else
                res = DoTransferAmount(target, app, AliPayAccount, TransferAmount.ToString("0.00"), PayTargetMode, out TransferId, order);

            transfer = ETransferAmount.Init(target, TransferId, TransferAmount, AliPayAccount, order,ui);
            transfer.AliPayOrderId = res.OrderId;

            if (res.Code == "10000")
            {
              
                //转账记录开始
                transfer.TransferStatus = TransferStatus.Success;
         

               
            }
            else
            {
               
                transfer.TransferStatus = TransferStatus.Failure;
                transfer.Log += string.Format("[Transfer to {2}] SubCode:{0};Submsg:{1}", res.SubCode, res.SubMsg, target.ToString());

                order.LogRemark += "【转账错误】"+ string.Format("[Transfer to {2}] SubCode:{0};Submsg:{1}", res.SubCode, res.SubMsg, target.ToString()); 
                order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Exception;
            }
            return transfer;
        }

       // public bool

         /// <summary>
         /// 通过转账验证账户
         /// </summary>
         /// <returns></returns>
        public OutAPIResult TestAccountByTransfer(string AliPayAccount, EAliPayApplication app)
        {
        
            OutAPIResult result = new OutAPIResult();
            string tId;
            try
            {
                
              
                AlipayFundTransToaccountTransferResponse res = DoTransferAmount(TransferTarget.User, 
                    app, AliPayAccount, "0.1", 
                    PayTargetMode.AliPayAccount, 
                    out tId,null,"账户校验");
                if(res.Code != "10000")
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = res.Msg+"--"+res.SubMsg;
                }
               
                return result;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
          
            return result;
        }

        public AlipayFundTransToaccountTransferResponse DoTransferAmount(TransferTarget target,
                                                                EAliPayApplication app,
                                                                string toAliPayAccount,
                                                                string Amount, 
                                                                PayTargetMode PayTargetMode,
                                                                out string TransferId, 
                                                                EOrderInfo order = null,
                                                                string ShowName =null)
        {
          
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
             app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);

            AlipayFundTransToaccountTransferRequest request = new AlipayFundTransToaccountTransferRequest();

            TransferId = StringHelper.GenerateTransferNo(target);
            AlipayFundTransToaccountTransferModel model = new AlipayFundTransToaccountTransferModel();
            model.Amount = Amount;
            model.OutBizNo = TransferId;
            if(PayTargetMode == PayTargetMode.AliPayAccount)
                model.PayeeType = "ALIPAY_LOGONID";
            else
                model.PayeeType = "ALIPAY_USERID";
          
            model.PayeeAccount = toAliPayAccount;
            if(!string.IsNullOrEmpty(ShowName))
            {
                model.PayerShowName = ShowName;
            }
            else
            {
                string profix = "";
                if (target == TransferTarget.ParentAgent)
                    profix = "(上级佣金)";
                else if (target == TransferTarget.Agent)
                    profix = "(代理费)";
                else if (target == TransferTarget.User)
                    profix = "(打款)";
                else if (target == TransferTarget.L3Agent)
                    profix = "(三级)";
                else if (target == TransferTarget.MidStore)
                    profix = "(码商)";
                model.PayerShowName = profix + "服务费";
            }
           
            if(order!=null)
                model.Remark = string.Format("#{0}-订单金额：{1}-订单ID：{2}",order.AgentName,order.TotalAmount,order.OrderNo);
          
            request.SetBizModel(model);

            AlipayFundTransToaccountTransferResponse response =  aliyapClient.Execute(request);

            return response;
        }

   

        public EOrderInfo InitUnKnowOrderForAliPayNotice(HttpRequestBase Request)
        {
            EOrderInfo order = new EOrderInfo();
            order.AliPayOrderNo = Request["trade_no"];
            order.AliPayPayChannel = "";
            order.AliPayTradeStatus = Request["trade_status"];
            order.AliPayAppId = Request["app_id"];

            order.OrderNo = Request["out_trade_no"];

            order.BuyerAliPayId = Request["buyer_id"];
            order.BuyerAliPayLoginId = Request["buyer_logon_id"];
            order.SellerAliPayId = Request["seller_id"];
            order.SellerAliPayEmail = Request["seller_email"];

            order.AliPayTotalAmount = Convert.ToSingle(Request["total_amount"]);
            order.AliPayReceiptAmount = Convert.ToSingle(Request["receipt_amount"]);
            order.AliPayBuerPayAmount = Convert.ToSingle(Request["buyer_pay_amount"]);

            order.OrderType = BaseEnum.OrderType.UnKnow;
            order.OrderStatus = BaseEnum.OrderStatus.Paid;
            //order.AliPayTransDate =Convert.ToDateTime(Request["gmt_create"]);
            return order;
        }
        public EAgentCommission InitAgentCommission(EOrderInfo order, EQRUser qrUser)
        {
            EAgentCommission comm = new EAgentCommission
            {
                OrderNo = order.OrderNo,
                AgentCommissionStatus = BaseEnum.AgentCommissionStatus.Open,
                ParentOpenId = qrUser.ParentOpenId,
                ChildOpenId = qrUser.OpenId,
                CommissionAmount = (float)Math.Round((qrUser.ParentCommissionRate / 100) * order.TotalAmount, 2, MidpointRounding.ToEven),
                Level = 2,
                CommissionRate = qrUser.ParentCommissionRate,
                ChildName = qrUser.UserName,
                ParentName = qrUser.ParentName,
                TransDate = DateTime.Now,
                TransDateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),

            };
            return comm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="qrUser">2级QRUser</param>
        /// <param name="topUser">1级代理人</param>
        /// <returns></returns>
        public EAgentCommission InitAgentCommission_L3(EOrderInfo order, EQRUser qrUser, RUserInfo topUser)
        {
            double L3Comm = 0.2;

            //if (qrUser.QRType == QRType.ARHuge)
            //    L3Comm = 0.2;

            EAgentCommission comm = new EAgentCommission
            {
                OrderNo = order.OrderNo,
                AgentCommissionStatus = BaseEnum.AgentCommissionStatus.Open,
                ParentOpenId = topUser.OpenId,
                ChildOpenId = qrUser.OpenId,
                CommissionAmount = (float)Math.Round((L3Comm / 100) * order.TotalAmount, 2, MidpointRounding.ToEven),
                Level = 3,
               
                CommissionRate = (float)L3Comm,
                ChildName = qrUser.UserName,
                ParentName = topUser.Name,
                TransDate = DateTime.Now,
                TransDateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                ParentAliPayAccount = topUser.AliPayAccount,

            };
            return comm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qrUser"></param>
        /// <param name="store"></param>
        /// <param name="TotalAmount">订单总金额</param>
        /// <param name="orderType">订单类型:大额/小额</param>
        /// <param name="AliPayAccount">支付宝账户</param>
        /// <param name="orderNum">是否有订单，没有费率则为全局费率</param>
        /// <param name="ui">如果用户被禁用，传入</param>
        /// <param name="QRHugeTrans"></param>
        /// <returns></returns>
        public EOrderInfo InitOrder(EQRUser qrUser,EStoreInfo store,EUserStore userStore, float TotalAmount,OrderType orderType,string AliPayAccount = "",int orderNum=0,EUserInfo ui = null,EQRHugeTrans QRHugeTrans = null)
        {
            
            EOrderInfo order = new EOrderInfo()
            {
                OrderNo = _handler.OrderNo,
                OrderStatus = BaseEnum.OrderStatus.WaitingAliPayNotify,
                QRUserId = qrUser.ID,
                AgentName = qrUser.UserName,
                AgentOpenId = qrUser.OpenId,
                TotalAmount = TotalAmount,
                Rate = qrUser.Rate,
                RateAmount = (float)Math.Round(TotalAmount * (qrUser.Rate / 100), 2, MidpointRounding.AwayFromZero),
                TransDate = DateTime.Now,
                TransDateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                SellerAliPayId = store.AliPayAccount,
                SellerStoreId = store.ID,
                SellerName = store.Name,
                SellerChannel = store.Channel,
                SellerRate = store.Rate,
                SellerCommission = (float)Math.Round(TotalAmount * (userStore.OwnerRate) / 100, 2, MidpointRounding.ToEven),
                OrderType = orderType,
               
                BuyerMarketRate = qrUser.MarketRate,
                BuyerTransferAmount = (float)Math.Round(TotalAmount * (100-qrUser.MarketRate) / 100, 2, MidpointRounding.AwayFromZero),
                BuyerAliPayAccount = AliPayAccount,
              
                //ReceiveNo = StringHelper.GenerateReceiveNo(),
                
            };
            if(QRHugeTrans!=null)
            {
                order.EQRHugeTransId = QRHugeTrans.ID;

                //大单用户手续费
                order.BuyerTransferAmount -= (float)RuleManager.PayRule().User_ServerFee_HQ;
            }
            else
            {
                //double FOFeeRate = RuleManager.PayRule().Agent_FOFeeRate;
                //小单用户手续费
                //if(order.TotalAmount>=199)
                //    order.BuyerTransferAmount -= (float)RuleManager.PayRule().User_ServerFee_Q;
                order.BuyerTransferAmount -= (float)RuleManager.PayRule().User_ServerFee_Q;

                ////首单费率
                //if(orderNum == 0 && (qrUser.MarketRate-qrUser.Rate)< FOFeeRate)
                //    order.RateAmount = (float)Math.Round(TotalAmount * ((qrUser.MarketRate-FOFeeRate) / 100), 2, MidpointRounding.ToEven);

                if (ui.UserStatus == UserStatus.JustRegister)
                {
                    order.RateAmount = (float)Math.Round(TotalAmount * (0.5 / 100), 2, MidpointRounding.ToEven);
                }

            }
           

            return order;
           
        }

        public EOrderInfo InitWXOrder(string No,EQRUser qrUser,float TotalAmount,string phone)
        {
            EOrderInfo order = new EOrderInfo()
            {
                OrderNo = No,
                OrderStatus = BaseEnum.OrderStatus.WaitingAliPayNotify,
                QRUserId = qrUser.ID,
                AgentName = qrUser.UserName,
                AgentOpenId = qrUser.OpenId,
                TotalAmount = TotalAmount,
                Rate = qrUser.Rate,
                RateAmount = (float)Math.Round(TotalAmount * (qrUser.Rate / 100), 2, MidpointRounding.AwayFromZero),
                TransDate = DateTime.Now,
                TransDateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),

                OrderType = OrderType.WX,

                BuyerMarketRate = qrUser.MarketRate,
                BuyerTransferAmount = (float)Math.Round(TotalAmount * (100 - qrUser.MarketRate) / 100, 2, MidpointRounding.AwayFromZero),
                BuyerMobilePhone = phone
                // BuyerAliPayAccount = AliPayAccount,
            };
            return order;
        }

        public string PayF2F_ForR(EAliPayApplication app, string SellerId,string amount, ETool_QR qr,out ResultEnum status)
        {
            string result = "";

            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(app.ServerUrl, app.AppId, app.Merchant_Private_Key, app.Version,
                                     app.SignType, app.Merchant_Public_key, app.Charset);

            _handler = new F2FPayHandler();

            AlipayTradePrecreateContentBuilder builder = _handler.BuildPrecreateContent_ForR(app,"", amount);

            AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder);

            status = precreateResult.Status;

            switch (precreateResult.Status)
            {
                case ResultEnum.SUCCESS:
                    result = _handler.CreateQR_ForR(precreateResult,qr);
                   
                    break;
                case ResultEnum.FAILED:
                    result = precreateResult.response.Body;

                    break;

                case ResultEnum.UNKNOWN:
                    if (precreateResult.response == null)
                    {
                        result = "配置或网络异常，请检查后重试";
                    }
                    else
                    {
                        result = "系统异常，请更新外部订单后重新发起请求";
                    }

                    break;
            }
         
            return result;
        }

        public string PayF2F(EAliPayApplication app, EUserInfo AgentUi, EStoreInfo storeInfo, float TotalAmount, out ResultEnum status)
        {
            string result = "";
            string NotifyUrl = ConfigurationManager.AppSettings["Main_SiteUrl"]+ "AliPay/PayNotify";
            /*
            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(AliPayConfig.serverUrl, AliPayConfig.appId, AliPayConfig.merchant_private_key, AliPayConfig.version,
                           AliPayConfig.sign_type, AliPayConfig.alipay_public_key, AliPayConfig.charset);
          */
            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(app.ServerUrl, app.AppId, app.Merchant_Private_Key, app.Version,
                                       app.SignType, app.Merchant_Public_key, app.Charset);

            _handler = new F2FPayHandler();

            AlipayTradePrecreateContentBuilder builder = _handler.BuildPrecreateContent(app, AgentUi,storeInfo.AliPayAccount, TotalAmount.ToString());

            AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder, NotifyUrl);

            status = precreateResult.Status;

            switch (precreateResult.Status)
            {
                case ResultEnum.SUCCESS:
                    result = _handler.CreateQR(precreateResult);
                    result = _handler.DeQR(result);

                    break;
                case ResultEnum.FAILED:
                    result = precreateResult.response.Body;

                    break;

                case ResultEnum.UNKNOWN:
                    if (precreateResult.response == null)
                    {
                        result = "配置或网络异常，请检查后重试";
                    }
                    else
                    {
                        result = "系统异常，请更新外部订单后重新发起请求";
                    }

                    break;
            }
            return result;
        }

        public string PayTest(EAliPayApplication app,EStoreInfo store,out AliPayResult status)
        {
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
          app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);


            _handler = new F2FPayHandler();
            EUserInfo ui = new EUserInfo();
            ui.Name = "Check Store";
            AlipayTradePrecreateResponse builder = _handler.BuildNew(app, store, ui, "1.00");
            if (builder.Code == "10000")
            {
                status = AliPayResult.SUCCESS;
            }
            else
            {
                status = AliPayResult.FAILED;
            }
            return builder.Code;
        }
        public string PayF2FNew(EAliPayApplication app, EUserInfo AgentUi, EStoreInfo storeInfo, string TotalAmount, out AliPayResult status)
        {
            string result = "";
            bool NeedControl = false;

            /*
            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(AliPayConfig.serverUrl, AliPayConfig.appId, AliPayConfig.merchant_private_key, AliPayConfig.version,
                           AliPayConfig.sign_type, AliPayConfig.alipay_public_key, AliPayConfig.charset);
          */
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
            app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);


            _handler = new F2FPayHandler();

            //if (AgentUi.UserRole == UserRole.Administrator)
            //    NeedControl = false;
            AlipayTradePrecreateResponse builder = _handler.BuildNew(app, storeInfo, AgentUi, TotalAmount,true, NeedControl);

            if(builder.Code == "10000")
            {
                result = _handler.CreateF2FQR(builder.QrCode);
                result = _handler.DeQR(result);
                status = AliPayResult.SUCCESS;
            }
            else
            {
                if (builder.Code == "20001")
                {
                    status = AliPayResult.AUTHERROR;
                    result = "授权错误！";
                }
                else
                {
                    result = "[Error Message]" + builder.Msg + "[Sub Msg]" + builder.SubMsg;
                    status = AliPayResult.FAILED;
                }
            }

          
            return result;
        }

        public string PartPay(EAliPayApplication app, EUserInfo AgentUi, EStoreInfo storeInfo, string TotalAmount, out AliPayResult status)
        {
            string result = "";

            /*
            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(AliPayConfig.serverUrl, AliPayConfig.appId, AliPayConfig.merchant_private_key, AliPayConfig.version,
                           AliPayConfig.sign_type, AliPayConfig.alipay_public_key, AliPayConfig.charset);
          */
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
            app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);


            _handler = new F2FPayHandler();

            AlipayTradePrecreateResponse builder = _handler.BuildNew(app, storeInfo, AgentUi, TotalAmount);

            if (builder.Code == "10000")
            {
                result = _handler.CreateF2FQR(builder.QrCode);
                result = _handler.DeQR(result);
                status = AliPayResult.SUCCESS;
            }
            else
            {
                if (builder.Code == "20001")
                {
                    status = AliPayResult.AUTHERROR;
                }
                else
                {
                    result = "[Error Message]" + builder.Msg + "[Sub Msg]" + builder.SubMsg;
                    status = AliPayResult.FAILED;
                }
            }


            return result;
        }

        public string PartPayQR(EAliPayApplication app, EUserInfo AgentUi, EStoreInfo storeInfo, string TotalAmount, out AliPayResult status)
        {
            string result = "";
        
            /*
            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(AliPayConfig.serverUrl, AliPayConfig.appId, AliPayConfig.merchant_private_key, AliPayConfig.version,
                           AliPayConfig.sign_type, AliPayConfig.alipay_public_key, AliPayConfig.charset);
          */
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
            app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);


            _handler = new F2FPayHandler();

            AlipayTradePrecreateResponse builder = _handler.BuildNew(app, storeInfo, AgentUi, TotalAmount);

            if (builder.Code == "10000")
            {
                result = _handler.CreateF2FQR(builder.QrCode,true);
             
                status = AliPayResult.SUCCESS;
            }
            else
            {
                if (builder.Code == "20001")
                {
                    status = AliPayResult.AUTHERROR;
                }
                else
                {
                    result = "[Error Message]" + builder.Msg + "[Sub Msg]" + builder.SubMsg;
                    status = AliPayResult.FAILED;
                }
            }


            return result;
        }
        public static List<Com.Alipay.Model.GoodsInfo> GetGoodsList(string TotalAmt)
        {

            List<Com.Alipay.Model.GoodsInfo> list = new List<Com.Alipay.Model.GoodsInfo>();
            Random r = new Random();
            int n = r.Next(1, 5);
            Com.Alipay.Model.GoodsInfo good = new Com.Alipay.Model.GoodsInfo();
            switch (n)
            {
                case 1:
                    good.goods_id = "冰种天然-碧玉尊翡翠手镯";
                    good.goods_name = "冰种天然-碧玉尊翡翠手镯";
                    break;
                case 2:
                    good.goods_id = "巴西天然钛晶HD-141";
                    good.goods_name = "巴西天然钛晶HD-141";
                    break;
                case 3:
                    good.goods_id = "野兽派 小算盘18K金钻石项链";
                    good.goods_name = "野兽派 小算盘18K金钻石项链";
                    break;
                case 4:
                    good.goods_id = "钻石裸钻GIA30-50-70";
                    good.goods_name = "钻石裸钻GIA30-50-70";
                    break;
                case 5:
                    good.goods_id = "飘花冰糯种玉镯子";
                    good.goods_name = "飘花冰糯种玉镯子";
                    break;
            }
           
            good.price = TotalAmt;
            good.quantity = "1";
            good.goods_category = "消费-服饰美容";
            list.Add(good);
            return list;
        }

        public AlipayTradeCloseResponse CleanWaitOrder(EAliPayApplication app,EOrderInfo order,EStoreInfo store=null)
        {
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
            app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);

            AlipayTradeCloseRequest request = new AlipayTradeCloseRequest();
            AlipayTradeCloseModel model = new AlipayTradeCloseModel();
            model.OutTradeNo = order.OrderNo;
            request.SetBizModel(model);
            AlipayTradeCloseResponse response = null;
            if (store!=null)
            {
                response = aliyapClient.Execute(request,null,store.AliPayAuthToke);
            }
            else
            {
                response = aliyapClient.Execute(request);
            }


           
            return response;
        }

        #region O2O
        /// <summary>
        /// O2O转账
        /// </summary>
        /// <param name="initedTrans">已初始化的Transfer对象</param>
        /// <returns></returns>
        public ETransferAmount O2OTransferHandler(ETransferAmount initedTransfer, EAliPayApplication app, EAliPayApplication subApp)
        {
            try
            {
                AlipayFundTransToaccountTransferResponse res = null;

                string TransferId = "";

                res =  DoTransferAmount(initedTransfer.TransferTarget, 
                                 subApp,
                                 initedTransfer.TargetAccount,
                                 initedTransfer.TransferAmount.ToString("0.00"), 
                                 PayTargetMode.AliPayAccount, out TransferId);
                initedTransfer.TransferId = TransferId;
                if (res.Code == "10000")
                {
                    initedTransfer.TransferStatus = TransferStatus.Success;
                }
                else
                {

                    initedTransfer.TransferStatus = TransferStatus.Failure;
                    initedTransfer.Log += string.Format("[Transfer to {2}] SubCode:{0};Submsg:{1}", res.SubCode, res.SubMsg, initedTransfer.TransferTarget.ToString());

                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return initedTransfer;
        }
        #endregion

       
    }
}
