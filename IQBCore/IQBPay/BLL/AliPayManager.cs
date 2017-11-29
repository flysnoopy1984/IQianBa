﻿using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.System;
using IQBCore.IQBPay.Models.User;
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

        public AlipayTradeOrderSettleResponse DoSubAccount(EAliPayApplication app,EOrderInfo order,EStoreInfo store,EStoreInfo receiveStore)
        {
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
              app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);


            AlipayTradeOrderSettleRequest request = new AlipayTradeOrderSettleRequest();
           
            string commission = (order.TotalAmount - order.SellerCommission).ToString("0.00");

            request.BizContent = "{" +
            "\"out_request_no\":\"" + StringHelper.GenerateSubAccountTransNo() + "\"," +
            "\"trade_no\":\""+order.AliPayOrderNo+"\"," +
            "      \"royalty_parameters\":[{" +
            "        \"trans_out\":\""+store.AliPayAccount+"\"," +
            "\"trans_in\":\""+ receiveStore .AliPayAccount+ "\"," +
            "\"amount\":"+ commission + "," +
            "\"desc\":\"分账\"" +
            "        }]," +
            "\"operator_id\":\"\"" +
            "  }";

            // model.RoyaltyParameters = paramList;
            // request.SetBizModel(model);

            AlipayTradeOrderSettleResponse response = aliyapClient.Execute(request, store.AliPayAuthToke);
            return response;
        }

        public AlipayFundTransToaccountTransferResponse TransferAmount(EAliPayApplication app,string toAliPayAccount,string Amount,out string TransferId)
        {
          
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
             app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);

            AlipayFundTransToaccountTransferRequest request = new AlipayFundTransToaccountTransferRequest();

            TransferId = StringHelper.GenerateSubAccountTransNo();
            AlipayFundTransToaccountTransferModel model = new AlipayFundTransToaccountTransferModel();
            model.Amount = Amount;
            model.OutBizNo = TransferId;
            model.PayeeType = "ALIPAY_LOGONID";
            model.PayeeAccount = toAliPayAccount;
            model.PayerShowName = "玉杰平台支付";
            model.Remark = string.Format("玉杰服务提现");

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
            order.AliPayTransDate =Convert.ToDateTime(Request["gmt_create"]);
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
                CommissionAmount = (qrUser.ParentCommissionRate/100)*order.TotalAmount,
                Level = 2,
                CommissionRate = qrUser.ParentCommissionRate,
                ChildName = qrUser.UserName,
                ParentName = qrUser.ParentName,
                TransDate = DateTime.Now,
                TransDateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),

            };
            return comm;
        }
        public EOrderInfo InitOrder(EQRUser qrUser,EStoreInfo store, float TotalAmount,string AliPayAccount = "")
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
                RateAmount = (float)Math.Round(TotalAmount * (qrUser.Rate / 100), 2, MidpointRounding.ToEven),
                TransDate = DateTime.Now,
                TransDateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                SellerAliPayId = store.AliPayAccount,
                SellerStoreId = store.ID,

                SellerName = store.Name,
                SellerChannel = store.Channel,
                SellerRate = store.Rate,
                SellerCommission = (float)Math.Round(TotalAmount * (store.Rate) / 100, 2, MidpointRounding.ToEven),
                OrderType = BaseEnum.OrderType.Normal,

                BuyerAliPayAccount = AliPayAccount,

                ReceiveNo = StringHelper.GenerateReceiveNo(),
                

            };
            order.RealTotalAmount = order.TotalAmount - order.RateAmount;

         

            return order;
           
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

            list.Add(good);
            return list;
        }
    }
}
