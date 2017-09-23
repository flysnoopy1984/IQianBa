using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using IQBPay.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class AliPayController : Controller
    {
        private string privateKey = "MIIEowIBAAKCAQEAwHKeFBun6j3+wQwcgmAoCG7f/TWU1ST+iTKT/oImEiyNOEGrwel3D0TI8/qmeKWVS0u/lKraICRWmIuvd0p8IO5ab3jgYxBJb88NoSCHF8QbZ46BwyLFPc+SdbEvs0VAtyLE4iVzRcBmTHqpo7gt7Ga9MAmaV+x1WUNyiLgxgZqJYQWIyfXkfnGtjL3H6ox+9InwPHIcxZxEEAtdtLZCcrwPCC1zqRp6j191RAFUZ8KNU3zN+J1+QKC1ae4iyt2nBOGtwTVK8LwrMVYIpTffs1sMBNUj3XOqTfwYX7fUFwBrtvaEk4i+CtJopVLNlUxEqauqPEtbPgF1SuxdFIxYPwIDAQABAoIBAHiDThqpdu1pBS8+tluue2NMi1e1Rg5zrDGeSq8GMXEQFR81gKld2gDlwjGGtNi4WFVeigo/M3kNSG0ejDLXogO9P0SvHVTrzhEGSDKue+qWE9M1mmzoSTv70GuDGavZoj0MuN4lNZpocadS6QhtPdTcQXzjhpOor5PGeOLE9buCQz/6YbpgWBKUxWERFZellrgoWaEumDqVSAY4xmflbwL54UIoI/AHlVe3YiKZ2a8RSDpdKQHX4JpU/NHYTI0ZNM6NlXJ5FaAbCdMXgvBQzMz40qg27iF+pCA4jkgS3a5q0BM1+KaB05TlYNh/QHxWaL0Mtt1xgmBlO0n4ho+riwECgYEA9WhqNkzINpHXlGx5opzkUCxA3VCcN2QBkeDFdrdieBfHBIGCVnhmvb+tMq5+mSsEqOdCLmPLAmj3x8XNEt1CiXn34M011/Oh1AcwXgSjtwNySozNsa+qXS57gEoS3xp1n1pkK9bB8Xkebt2l7b2ibkSMLteeuIYl1q4WkLk77qcCgYEAyMEHsyuf8OAL04aGrJWlf4npv7lh9rCqWWLeBrkvt701sDoouwfo5m80i0eWhBbY5Nlh02LqBcJd1Iwk3WSjMA7XNj9CkUWoM4u/hz9YZMigMyPu9EWo7eqGypT9DIhQ67Z1VESlTS0LK2V3bDIkxAJhumgdhCp8iELjEauLVKkCgYBWoXB1EK/Qy7UdeRmLNPVH9AdF2TH8P7pqI72xRdVl7Ybc6Vb4bXJfY22hqYWZTl1Lvq9XLvU4OZPWmtXk5eSaIUtGuUpbnG6xKYSCfALLFVVgScpHAmsSj9kbFYsJ5Q5GnaMk8p/uPUJoAqiTf1D6ugn+czFdlEWBPl1K44jrmwKBgQCE+i/yg7wfHxlWVO7SVRHaKG1YXSDB+oXsTawKQhKUn9V3VR7zvKqOMS1Z8OKHvmaPOFsvXX7sr7Hdf7NPn0DlLX9q5H5gogZnlnMY0GHp6GcNWQkIbzgV2FrOx9/StFz9tc+EMTBZrbOPXFe9qH1oBLfddOfQSyBQVhX492uEeQKBgA/Umil0P/q/nECzL+71LMlpGM6ldRcOsnHKh+5/48h5VImiFn8YhOewboYUnkoBJBJXf5upj3GA+J2OSkojxgOwfBC+EWreXmGltYw/2bZ3CO08Fs80vXDbhMaPIUcjfsS4CCxx0qIJanhf/6522kT3avj5aA4jYE04by8qV4KI";

        private string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwHKeFBun6j3+wQwcgmAoCG7f/TWU1ST+iTKT/oImEiyNOEGrwel3D0TI8/qmeKWVS0u/lKraICRWmIuvd0p8IO5ab3jgYxBJb88NoSCHF8QbZ46BwyLFPc+SdbEvs0VAtyLE4iVzRcBmTHqpo7gt7Ga9MAmaV+x1WUNyiLgxgZqJYQWIyfXkfnGtjL3H6ox+9InwPHIcxZxEEAtdtLZCcrwPCC1zqRp6j191RAFUZ8KNU3zN+J1+QKC1ae4iyt2nBOGtwTVK8LwrMVYIpTffs1sMBNUj3XOqTfwYX7fUFwBrtvaEk4i+CtJopVLNlUxEqauqPEtbPgF1SuxdFIxYPwIDAQAB";

        public string publicKey2 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAshtxB8HjXoFchAfohW4kdnF/b74hA15vVrB+JeBDG7QuRssyR8NG17Nho4VgSQ1eqkVzOpKmNcmGWHEhC7sZosFzFZyOlofqpEte75KXxvyOI/fkoHe5CtQlPuXJln7hivY0er1b/vOXv4cCeMdZUYUPaLdJqCBHZjlay6vmxuw6Y4I62i58eHoP2oVypUj/v0S560OyXv8lk0MGhe1bWfM1RsZcRKCA1lV2sor4PC7KOMTSoIO9k1GWj0FyLWuypif8oX9d9z38FscEGQycu71gY64eXq0Fq4CPf85Y4A5YFxRbstiutIjn8J9F0d8gaVHgKau4EkAdHHRWp+NkJwIDAQAB";

        public string pk1_rs1 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA3aaDFxkj4IfzV42j8lwdJCZgTPTfrwDTiFfxhQXwFk/9sstsNdSkrYzKAmMBgl95d7R9bA8ASc0A8JADgR1ye+gsky9K/l8DEI8ZbgSWgCdEkTHbuZtzLo0SN9Q+U6k/g3QWTV27+0WHXHNwECFhdk23V0s2MeF+HrYgPn0WSkpYwz58hCDV9Eh71sj05tcgWfitcEkMLSazXmDqRsv8LZjtzpXO9Chwssfi9iCWa3hfsuzfmXusk8TRwtRyUtD9hIq4Fxr2+QJ2AvMlyK7/Sgtnsgl+lIv869jVyaNydlwSv8js1TM8nXPVemTWvj7fQUnWhU0YRHVa0XcdeyvaBwIDAQAB";

        public string AppID = ConfigurationManager.AppSettings["APPID"];


        public string callQRImg()
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

        public string callQuery_AuthToken()
        {
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppID,
                privateKey, "json", "1.0", "RSA2", publicKey2, "UTF-8", false);



            AlipayOpenAuthTokenAppQueryRequest request = new AlipayOpenAuthTokenAppQueryRequest();
            request.BizContent ="{" +
            "    \"app_auth_token\":\"201709BBd8a868e8d3ab4f4fb61d1f6f42d3dE39\"" +
            "  }";
            AlipayOpenAuthTokenAppQueryResponse response = aliyapClient.Execute(request);
          
            return response.Body;
        }


        private string CallAliPay()
        {
          
            // AlipayClient alipayClient = new DefaultAlipayClien
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AppID,
                privateKey,"json", "1.0", "RSA2", publicKey2, "UTF-8", false);

            AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
            model.Body = "至尊宝";
            model.Subject = "爱钱吧币";
            model.TotalAmount = "0.01";
            model.OutTradeNo = "IQB20170921";
            model.ProductCode = "QUICK_WAP_PAY";
          //  model.SellerId = "2088821092484390";
            model.AuthToken = "201709BBd8a868e8d3ab4f4fb61d1f6f42d3dE39";

            AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
          
            /*
            request.BizContent = "{" +
"    \"body\":\"至尊宝被\"," +
"    \"subject\":\"爱钱吧币\"," +
"    \"out_trade_no\":\"IQB20170921\"," +
"    \"timeout_express\":\"90m\"," +
"    \"total_amount\":1.00," +
"    \"product_code\":\"QUICK_WAP_WAY\"" +
"  }";

*/
            request.SetBizModel(model);

            AlipayTradeWapPayResponse response = aliyapClient.Execute(request, null, "201709BBd8a868e8d3ab4f4fb61d1f6f42d3dE39");
            
            string ret = response.Body;
            return ret;
        }


        public string callF2FPay()
        {

            string result="";  
           
            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(AliPayConfig.serverUrl, AliPayConfig.appId, AliPayConfig.merchant_private_key, AliPayConfig.version,
                            AliPayConfig.sign_type, AliPayConfig.alipay_public_key, AliPayConfig.charset);

            F2FPayHandler handler = new F2FPayHandler();
            AlipayTradePrecreateContentBuilder builder = handler.BuildPrecreateContent();

            AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder);

            switch (precreateResult.Status)
            {
                case ResultEnum.SUCCESS:
                    result = handler.CreateQR(precreateResult);
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


        // GET: AliPay
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Auth()
        {

            return View();
        }

        public ActionResult QueryToken()
        {
            return Content(callQuery_AuthToken());
        }

        public ActionResult Pay()
        {
            return Content(CallAliPay());
        }

        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult QRImg()
        {
           return Content( callQRImg());
        }

        public ActionResult F2FPay()
        {
            return Content(callF2FPay());
        }

        public ActionResult AuthToISV()
        {
           
        }
    }
}