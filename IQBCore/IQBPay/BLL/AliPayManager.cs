using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.System;
using IQBCore.IQBPay.Models.User;
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
            
            Aop.Api.Domain.AlipayTradeOrderSettleModel model = new AlipayTradeOrderSettleModel();

            model.OutRequestNo = StringHelper.GenerateSubAccountTransNo();
            model.TradeNo = order.AliPayOrderNo;

            List<OpenApiRoyaltyDetailInfoPojo> paramList = new List<OpenApiRoyaltyDetailInfoPojo>();

            OpenApiRoyaltyDetailInfoPojo p = new OpenApiRoyaltyDetailInfoPojo();
            p.TransOut = store.AliPayAccount;
            p.TransIn = receiveStore.AliPayAccount;
            p.Amount = Convert.ToInt64(order.SellerCommission);
            if (p.Amount <= 0)
                p.Amount = 1;
            p.AmountPercentage = 100;


            paramList.Add(p);

            model.RoyaltyParameters = paramList;
            request.SetBizModel(model);

            AlipayTradeOrderSettleResponse response = aliyapClient.Execute(request, store.AliPayAuthToke);
            return response;
        }

        public AlipayFundTransToaccountTransferResponse TransferAmount(EAliPayApplication app,EUserInfo ui,string Amount)
        {
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
             app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);

            AlipayFundTransToaccountTransferRequest request = new AlipayFundTransToaccountTransferRequest();

            AlipayFundTransToaccountTransferModel model = new AlipayFundTransToaccountTransferModel();
            model.Amount = Amount;
            model.OutBizNo = StringHelper.GenerateSubAccountTransNo();
            model.PayeeType = "ALIPAY_LOGONID";
            model.PayeeAccount = ui.AliPayAccount;
            model.PayerShowName = "爱钱吧平台支付";
            model.Remark = string.Format("爱钱吧提现");

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

        public EOrderInfo InitOrder(EQRUser qrUser,EStoreInfo store, float TotalAmount)
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
                RateAmount = TotalAmount * (qrUser.Rate / 100),
                TransDate = DateTime.Now,
                SellerAliPayId = store.AliPayAccount,
                SellerStoreId = store.ID,
               
                SellerName = store.Name,
                SellerChannel = store.Channel,
                SellerRate = store.Rate,
                SellerCommission = TotalAmount*(store.Rate)/100,
                

                OrderType = BaseEnum.OrderType.Normal,

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
    }
}
