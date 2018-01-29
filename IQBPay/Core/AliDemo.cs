using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.Sys;
using IQBPay.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace IQBPay.Core
{
    public class AliDemo
    {
        private static string privateKey = BaseController.App.Merchant_Private_Key;



        public static string publicKey2 = BaseController.App.Merchant_Public_key;

      

        public static string AppID = BaseController.App.AppId;




        public static string callQRImg()
        {
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppID,
                privateKey, "json", "1.0", "RSA2", publicKey2, "UTF-8", false);

            AlipayMobilePublicQrcodeCreateRequest request = new AlipayMobilePublicQrcodeCreateRequest();

            string json = "";

            using (StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"Content\json\QR.json"))
            {
                json = sr.ReadToEnd();
            }
            request.BizContent = json;

            AlipayMobilePublicQrcodeCreateResponse response = aliyapClient.Execute(request);
            return response.Body;
        }

        public static string GetAuthToken()
        {
            //GBK
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppID,
                privateKey, "json", "1.0", "RSA2", publicKey2, "utf-8", false);

            AlipayOpenAuthTokenAppRequest request = new AlipayOpenAuthTokenAppRequest();
            AlipayOpenAuthTokenAppModel model = new AlipayOpenAuthTokenAppModel();
            model.GrantType = "authorization_code";
            model.Code = "1627f85321104f4eba8e2b23342faB53";
            /*
            request.setBizContent("{" +
            "    \"grant_type\":\"authorization_code\"," +
            "    \"code\":\"1cc19911172e4f8aaa509c8fb5d12F56\"" +
            "  }");
            */
            request.SetBizModel(model);
            AlipayOpenAuthTokenAppResponse response = aliyapClient.Execute(request);
            return response.Body;
        }

        public static string callAliPay_Wap(string amt)
        {
            EAliPayApplication app = BaseController.App;
            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
                       app.Merchant_Private_Key, "json", app.Version, app.SignType, app.Merchant_Public_key, "UTF-8", false);

            
            AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
            model.Body = "服务费";
            model.Subject = "服务费";
            model.TotalAmount = amt;
            model.OutTradeNo = StringHelper.GenerateOrderNo();
            model.ProductCode = "QUICK_WAP_PAY";
            model.GoodsType = "1";
            //model.EnablePayChannels = "pcreditpayInstallment";
            //model.ExtendParams = new ExtendParams();
            //model.ExtendParams.HbFqNum = "3";
            //model.ExtendParams.HbFqSellerPercent = "100";
            // model.EnablePayChannels = "pcredit";
            //  model.SellerId = "2088821092484390";
            // model.AuthToken = "201709BBd8a868e8d3ab4f4fb61d1f6f42d3dE39";

            AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
            

            request.SetBizModel(model);

            AlipayTradeWapPayResponse response = alipayClient.pageExecute(request);
           
            string ret = response.Body;
                return ret;
        }

        public static string callAliPay_PC(string amt)
        {
            EAliPayApplication app = BaseController.SubApp;
            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
                       app.Merchant_Private_Key, "json", app.Version, app.SignType, app.Merchant_Public_key, "UTF-8", false);


            AlipayTradePayModel model = new AlipayTradePayModel();
            string orderNo = StringHelper.GenerateOrderNo();

            model.Body = "原版书册";
            model.Subject = "原版书册";
            model.TotalAmount = amt;
            model.OutTradeNo = orderNo;
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";
           
            
            model.ExtendParams = new ExtendParams();
            model.ExtendParams.HbFqNum = "3";
            model.ExtendParams.HbFqSellerPercent = "100";
       //     model.ExtendParams.SysServiceProviderId = "";





            // model.EnablePayChannels = "pcredit";
            //  model.SellerId = "2088821092484390";
            // model.AuthToken = "201709BBd8a868e8d3ab4f4fb61d1f6f42d3dE39";

            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();


            request.SetBizModel(model);

            AlipayTradePagePayResponse response = alipayClient.pageExecute(request);

            string ret = response.Body;
            return ret;
        }

        public static string callQuery_AuthToke()
        {
            int i = 0;
            EAliPayApplication app = BaseController.App;
            IAopClient a2 = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppID,
                privateKey, "json", "1.0", "RSA2", publicKey2, "UTF-8", false);
            /*
                       if(AppID!=app.AppId)
                       {
                           i++;
                       }

                       if (privateKey != app.Merchant_Private_Key)
                       {
                           i++;
                       }

                       if ("1.0" != app.Version)
                       {
                           i++;
                       }

                       if ("RSA2" != app.SignType)
                       {
                           i++;
                       }

                       if (publicKey2 != app.Merchant_Public_key)
                       {
                           i++;
                       }

               */

            IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
                                app.Merchant_Private_Key, "json", app.Version, app.SignType, app.Merchant_Public_key, "UTF-8", false);


            AlipayOpenAuthTokenAppQueryModel model = new AlipayOpenAuthTokenAppQueryModel();
            model.AppAuthToken = "201711BB6664b2966d804ee9a24e82491628bX43";

            AlipayOpenAuthTokenAppQueryRequest request = new AlipayOpenAuthTokenAppQueryRequest();
            /*
            request.BizContent ="{" +
            "    \"app_auth_token\":\"201709BBd8a868e8d3ab4f4fb61d1f6f42d3dE39\"" +
            "  }";
            */
            request.SetBizModel(model);
            AlipayOpenAuthTokenAppQueryResponse response = alipayClient.Execute(request);

            return response.Body;
        }
    }
}