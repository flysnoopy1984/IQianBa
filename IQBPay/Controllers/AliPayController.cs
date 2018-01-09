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

namespace IQBPay.Controllers
{
    public class AliPayController : BaseController
    {
        private string privateKey = "MIIEowIBAAKCAQEAwHKeFBun6j3+wQwcgmAoCG7f/TWU1ST+iTKT/oImEiyNOEGrwel3D0TI8/qmeKWVS0u/lKraICRWmIuvd0p8IO5ab3jgYxBJb88NoSCHF8QbZ46BwyLFPc+SdbEvs0VAtyLE4iVzRcBmTHqpo7gt7Ga9MAmaV+x1WUNyiLgxgZqJYQWIyfXkfnGtjL3H6ox+9InwPHIcxZxEEAtdtLZCcrwPCC1zqRp6j191RAFUZ8KNU3zN+J1+QKC1ae4iyt2nBOGtwTVK8LwrMVYIpTffs1sMBNUj3XOqTfwYX7fUFwBrtvaEk4i+CtJopVLNlUxEqauqPEtbPgF1SuxdFIxYPwIDAQABAoIBAHiDThqpdu1pBS8+tluue2NMi1e1Rg5zrDGeSq8GMXEQFR81gKld2gDlwjGGtNi4WFVeigo/M3kNSG0ejDLXogO9P0SvHVTrzhEGSDKue+qWE9M1mmzoSTv70GuDGavZoj0MuN4lNZpocadS6QhtPdTcQXzjhpOor5PGeOLE9buCQz/6YbpgWBKUxWERFZellrgoWaEumDqVSAY4xmflbwL54UIoI/AHlVe3YiKZ2a8RSDpdKQHX4JpU/NHYTI0ZNM6NlXJ5FaAbCdMXgvBQzMz40qg27iF+pCA4jkgS3a5q0BM1+KaB05TlYNh/QHxWaL0Mtt1xgmBlO0n4ho+riwECgYEA9WhqNkzINpHXlGx5opzkUCxA3VCcN2QBkeDFdrdieBfHBIGCVnhmvb+tMq5+mSsEqOdCLmPLAmj3x8XNEt1CiXn34M011/Oh1AcwXgSjtwNySozNsa+qXS57gEoS3xp1n1pkK9bB8Xkebt2l7b2ibkSMLteeuIYl1q4WkLk77qcCgYEAyMEHsyuf8OAL04aGrJWlf4npv7lh9rCqWWLeBrkvt701sDoouwfo5m80i0eWhBbY5Nlh02LqBcJd1Iwk3WSjMA7XNj9CkUWoM4u/hz9YZMigMyPu9EWo7eqGypT9DIhQ67Z1VESlTS0LK2V3bDIkxAJhumgdhCp8iELjEauLVKkCgYBWoXB1EK/Qy7UdeRmLNPVH9AdF2TH8P7pqI72xRdVl7Ybc6Vb4bXJfY22hqYWZTl1Lvq9XLvU4OZPWmtXk5eSaIUtGuUpbnG6xKYSCfALLFVVgScpHAmsSj9kbFYsJ5Q5GnaMk8p/uPUJoAqiTf1D6ugn+czFdlEWBPl1K44jrmwKBgQCE+i/yg7wfHxlWVO7SVRHaKG1YXSDB+oXsTawKQhKUn9V3VR7zvKqOMS1Z8OKHvmaPOFsvXX7sr7Hdf7NPn0DlLX9q5H5gogZnlnMY0GHp6GcNWQkIbzgV2FrOx9/StFz9tc+EMTBZrbOPXFe9qH1oBLfddOfQSyBQVhX492uEeQKBgA/Umil0P/q/nECzL+71LMlpGM6ldRcOsnHKh+5/48h5VImiFn8YhOewboYUnkoBJBJXf5upj3GA+J2OSkojxgOwfBC+EWreXmGltYw/2bZ3CO08Fs80vXDbhMaPIUcjfsS4CCxx0qIJanhf/6522kT3avj5aA4jYE04by8qV4KI";

        private string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwHKeFBun6j3+wQwcgmAoCG7f/TWU1ST+iTKT/oImEiyNOEGrwel3D0TI8/qmeKWVS0u/lKraICRWmIuvd0p8IO5ab3jgYxBJb88NoSCHF8QbZ46BwyLFPc+SdbEvs0VAtyLE4iVzRcBmTHqpo7gt7Ga9MAmaV+x1WUNyiLgxgZqJYQWIyfXkfnGtjL3H6ox+9InwPHIcxZxEEAtdtLZCcrwPCC1zqRp6j191RAFUZ8KNU3zN+J1+QKC1ae4iyt2nBOGtwTVK8LwrMVYIpTffs1sMBNUj3XOqTfwYX7fUFwBrtvaEk4i+CtJopVLNlUxEqauqPEtbPgF1SuxdFIxYPwIDAQAB";

        public string publicKey2 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAshtxB8HjXoFchAfohW4kdnF/b74hA15vVrB+JeBDG7QuRssyR8NG17Nho4VgSQ1eqkVzOpKmNcmGWHEhC7sZosFzFZyOlofqpEte75KXxvyOI/fkoHe5CtQlPuXJln7hivY0er1b/vOXv4cCeMdZUYUPaLdJqCBHZjlay6vmxuw6Y4I62i58eHoP2oVypUj/v0S560OyXv8lk0MGhe1bWfM1RsZcRKCA1lV2sor4PC7KOMTSoIO9k1GWj0FyLWuypif8oX9d9z38FscEGQycu71gY64eXq0Fq4CPf85Y4A5YFxRbstiutIjn8J9F0d8gaVHgKau4EkAdHHRWp+NkJwIDAQAB";

        public string pk1_rs1 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA3aaDFxkj4IfzV42j8lwdJCZgTPTfrwDTiFfxhQXwFk/9sstsNdSkrYzKAmMBgl95d7R9bA8ASc0A8JADgR1ye+gsky9K/l8DEI8ZbgSWgCdEkTHbuZtzLo0SN9Q+U6k/g3QWTV27+0WHXHNwECFhdk23V0s2MeF+HrYgPn0WSkpYwz58hCDV9Eh71sj05tcgWfitcEkMLSazXmDqRsv8LZjtzpXO9Chwssfi9iCWa3hfsuzfmXusk8TRwtRyUtD9hIq4Fxr2+QJ2AvMlyK7/Sgtnsgl+lIv869jVyaNydlwSv8js1TM8nXPVemTWvj7fQUnWhU0YRHVa0XcdeyvaBwIDAQAB";

        public string AppID = ConfigurationManager.AppSettings["APPID"];

        public object EORderInfo { get; private set; }

        public string callF2FPay(EStoreInfo store,string TotalAmt,string orderNo)
        {

            string result = "";

            EAliPayApplication app = BaseController.App;
            IAopClient aliyapClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
            app.Merchant_Private_Key, "json", "1.0", "RSA2", app.Merchant_Public_key, "GBK", false);

            AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();
            AlipayTradePrecreateModel model = new AlipayTradePrecreateModel();
          
            model.SellerId = store.AliPayAccount;
            model.OutTradeNo = orderNo;
            model.TotalAmount = TotalAmt;
            model.Subject = "找熟人-原原 收银台";
            model.Body =  app.AppName + "-商品";
           // List<GoodsDetail> gList = new List<GoodsDetail>();
           // GoodsDetail good = new GoodsDetail();
           // good.GoodsName = "找熟人服务包";
           // good.GoodsId = "找熟人服务包";
           //// good.GoodsCategory = "服装";
           // good.Price = TotalAmt;
           // good.Quantity = 1;

           // model.GoodsDetail= gList;

            request.SetBizModel(model);

            AlipayTradePrecreateResponse  response = aliyapClient.Execute(request, null, store.AliPayAuthToke);
            return response.Body;
            /*
            F2FPayHandler handler = new F2FPayHandler();
            EUserInfo ui = new EUserInfo();
            ui.Name = "Test";
            AlipayTradePrecreateContentBuilder builder = handler.BuildPrecreateContent(BaseController.App,ui, sellerid, TotalAmt);

            AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder);


            switch (precreateResult.Status)
            {
                case ResultEnum.SUCCESS:
                    result = handler.CreateQR(precreateResult);
                    result = handler.DeQR(result);

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
            */
            return result;
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

        

        public ActionResult PayNotify()
        {
            string orderNo = Request["out_trade_no"];
            AliPayManager payManager = new AliPayManager();
            ETransferAmount tranfer = null;
            EAgentCommission agentComm = null;
            int TransferError = 0 ;

            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    
                    EOrderInfo order = db.DBOrder.Where(o => o.OrderNo == orderNo).FirstOrDefault();

                    if (order == null)
                    {
                        //order = payManager.InitUnKnowOrderForAliPayNotice(Request);
                        //db.DBOrder.Add(order);
                        //db.SaveChanges();
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
                  //  order.AliPayTransDate = Convert.ToDateTime(Request["gmt_create"]);
                    if (order.AliPayTradeStatus == "TRADE_SUCCESS")
                    {
                      //  Log.log("PayNotify 1");

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
                        //店铺佣金
                        if (!store.IsReceiveAccount)
                        {
                          //  Log.log("PayNotify 开始分账");
                            //分账
                            
                            EStoreInfo subStore =null;
                            try
                            {
                                subStore = BaseController.SubAccount;
                            }
                            catch (Exception ex)
                            {
                                order.LogRemark += ex.Message;
                                order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Exception;
                                Log.log("subStore 获取错误");
                            }
                            if (subStore!=null)
                            {
                                try
                                {
                                    AlipayTradeOrderSettleResponse res = payManager.DoSubAccount(BaseController.App, order, store, subStore);
                                    if (res.Code == "10000")
                                        order.LogRemark += string.Format("[SubAccount] Code:{0};msg:{1}; ", res.Code, res.Msg);
                                    else
                                    {
                                        order.LogRemark += string.Format("[SubAccount] SubCode:{0};Submsg:{1}; ", res.SubCode, res.SubMsg);
                                        order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Exception;
                                        store.RecordStatus = RecordStatus.Blocked;
                                    }
                                }
                                catch(Exception ex)
                                {
                                    order.LogRemark += ex.Message;
                                    store.RecordStatus = RecordStatus.Blocked;
                                }
                               
                            }
                         //  Log.log("PayNotify 分账结束");
                        }
                        string accessToken = this.getAccessToken(true);
                        //代理打款
                        EUserInfo agentUI = db.DBUserInfo.Where(u => u.OpenId == order.AgentOpenId).FirstOrDefault();
                  
                      //  Log.log("PayNotify 开始转账给代理");
                        tranfer = payManager.TransferHandler(TransferTarget.Agent, BaseController.SubApp, BaseController.SubApp, agentUI,ref order, accessToken,BaseController.GlobalConfig);
                        db.DBTransferAmount.Add(tranfer);
                        if(tranfer.TransferStatus != TransferStatus.Success)
                            TransferError++;
                      //  Log.log("PayNotify 转账给代理结束");

                        //上级代理佣金
                        if (!string.IsNullOrEmpty(order.ParentOpenId) && order.ParentCommissionAmount > 0)
                        {

                            agentComm = db.DBAgentCommission.Where(c => c.OrderNo == order.OrderNo && c.ParentOpenId == order.ParentOpenId && c.AgentCommissionStatus == AgentCommissionStatus.Open).FirstOrDefault();

                            EUserInfo parentUi = new EUserInfo();
                            parentUi.AliPayAccount = agentComm.ParentAliPayAccount;

                            //用户转账函数赋值
                            parentUi.OpenId = agentComm.ParentOpenId;
                            parentUi.Name = agentComm.ParentName;

                            tranfer = payManager.TransferHandler(TransferTarget.ParentAgent, BaseController.SubApp, BaseController.SubApp, parentUi, ref order, null,BaseController.GlobalConfig);
                            db.DBTransferAmount.Add(tranfer);
                            
                            if(tranfer.TransferStatus == TransferStatus.Success)
                                agentComm.AgentCommissionStatus = AgentCommissionStatus.Closed;
                            else
                                TransferError++;
                        }
                        //3级
                        if (!string.IsNullOrEmpty(order.L3OpenId) && order.L3CommissionAmount>0)
                        {
                            agentComm = db.DBAgentCommission.Where(c => c.OrderNo == order.OrderNo && c.ParentOpenId == order.L3OpenId && c.AgentCommissionStatus == AgentCommissionStatus.Open).FirstOrDefault();

                            EUserInfo parentUi = new EUserInfo();
                            parentUi.AliPayAccount = agentComm.ParentAliPayAccount;

                            //用户转账函数赋值
                            parentUi.OpenId = agentComm.ParentOpenId;
                            parentUi.Name = agentComm.ParentName;

                            tranfer = payManager.TransferHandler(TransferTarget.L3Agent, BaseController.SubApp, BaseController.SubApp, parentUi, ref order, null, BaseController.GlobalConfig);
                            db.DBTransferAmount.Add(tranfer);

                            if (tranfer.TransferStatus == TransferStatus.Success)
                                agentComm.AgentCommissionStatus = AgentCommissionStatus.Closed;
                            else
                                TransferError++;
                        }

                            //用户打款
                       //  Log.log("PayNotify 开始用户打款");
                        tranfer = payManager.TransferHandler(TransferTarget.User, BaseController.App, BaseController.SubApp,null, ref order, null, BaseController.GlobalConfig);
                        db.DBTransferAmount.Add(tranfer);

                        if(tranfer.TransferStatus != TransferStatus.Success)
                            TransferError++;
                        if(TransferError>0)
                            order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Exception;
                        else
                            order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Closed;

                      //  Log.log("PayNotify 结束用户打款");

                 
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
                    
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                Log.log("PayNotify Error:" + ex.Message);
            }

            return View();
        }

        public ActionResult AuthHanYi()
        {
            string authCode = Request["app_auth_code"];
            string appId = Request["app_id"];
            string Id = Request["Id"];
            string StoreId = Request["StoreId"];
            long qrId;
            EQRInfo qr = null;
            EStoreInfo store = null;
            EStoreInfo SelfStore = null;
            Log.log("Auth Code:" + authCode);
            EAliPayApplication app = null;
            AlipayOpenAuthTokenAppResponse response = null;

            if (!string.IsNullOrEmpty(authCode))
            {
                if (string.IsNullOrEmpty(Id) || !long.TryParse(Id, out qrId))
                {
                    Log.log("Auth No Id");
                    return Content("【传入的值不正确】无法授权，请联系平台");
                }
                app = BaseController.SubApp;
                if (app == null)
                {
                    Log.log("Auth No app");
                    return Content("【没有APP】无法授权，请联系平台");
                }

                try
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        qr = db.QR_GetById(qrId, IQBCore.IQBPay.BaseEnum.QRType.StoreAuth);

                        if (qr == null)
                        {
                            Log.log("Auth No QR");
                            return Content("【授权码不存在】无法授权，请联系平台！");
                        }
                        else if (qr.RecordStatus == IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked)
                            return Content("【授权码已被使用】无法授权，请联系平台！");


                        IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
                        app.Merchant_Private_Key, "json", app.Version, app.SignType, app.Merchant_Public_key, "UTF-8", false);

                        AlipayOpenAuthTokenAppRequest request = new AlipayOpenAuthTokenAppRequest();

                        AlipayOpenAuthTokenAppModel model = new AlipayOpenAuthTokenAppModel();
                        model.GrantType = "authorization_code";
                        model.Code = authCode;

                        request.SetBizModel(model);

                        response = alipayClient.Execute(request);
                        if (response.Code == "10000")
                        {
                            if (!string.IsNullOrEmpty(StoreId))
                                SelfStore = db.DBStoreInfo.Where(s => s.ID == Convert.ToInt32(StoreId)).FirstOrDefault();


                            store = db.Store_GetByAliPayUserId(response.UserId);
                            if (store == null)
                            {
                                if (SelfStore != null)
                                {
                                    SelfStore.AliPayAccount = response.UserId;
                                    SelfStore.AliPayAuthAppId = response.AuthAppId;
                                    SelfStore.AliPayAuthToke = response.AppAuthToken;

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
                                        Name = qr.Name,
                                        Remark = qr.Remark,
                                        RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Normal,
                                        QRId = qr.ID,
                                        Rate = qr.Rate,
                                        FromIQBAPP = app.AppId,
                                    };
                                    store.InitCreate();
                                    store.InitModify();
                                    db.DBStoreInfo.Add(store);
                                }


                            }
                            else
                            {
                                if (SelfStore != null)
                                {
                                    SelfStore.AliPayAccount = response.UserId;
                                    SelfStore.AliPayAuthAppId = response.AuthAppId;
                                    SelfStore.AliPayAuthToke = response.AppAuthToken;

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
                                }
                            }
                            qr.InitModify();
                            qr.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked;
                            Log.log("Auth QR Status:" + qr.RecordStatus);
                            db.SaveChanges();
                        }
                        else
                        {
                            Log.log("response Code" + response.Code);
                            return Content("授权失败：" + response.Msg);
                        }

                    }
                    string url = ConfigurationManager.AppSettings["IQBWX_SiteUrl"] + "/PP/Auth_Store?Rate=" + store.Rate;
                    return Redirect(url);
                }
                catch (Exception ex)
                {
                    Log.log("Auth Response Error:" + ex.Message);
                    Log.log("Auth Response Inner Error:" + ex.InnerException);
                    return View();

                }

            }
            else
            {
                return Content("No Auth Code");
            }


        }

        public ActionResult Auth()
        {
            string authCode = Request["app_auth_code"];
            string appId = Request["app_id"];
            string Id = Request["Id"];
            string StoreId = Request["StoreId"];
            long qrId;
            EQRInfo qr = null;
            EStoreInfo store = null;
            EStoreInfo SelfStore = null;
            Log.log("Auth Code:"+authCode);
            EAliPayApplication app = null;
            AlipayOpenAuthTokenAppResponse response = null;

            if (!string.IsNullOrEmpty(authCode))
            {
                if (string.IsNullOrEmpty(Id) || !long.TryParse(Id, out qrId))
                {
                    Log.log("Auth No Id");
                    return Content("【传入的值不正确】无法授权，请联系平台");
                }
                app = BaseController.App;
                if (app == null)
                {
                    Log.log("Auth No app");
                    return Content("【没有APP】无法授权，请联系平台");
                }
               
                try
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        qr = db.QR_GetById(qrId, IQBCore.IQBPay.BaseEnum.QRType.StoreAuth);

                        if (qr == null)
                        {
                            Log.log("Auth No QR");
                            return Content("【授权码不存在】无法授权，请联系平台！");
                        }
                        else if (qr.RecordStatus == IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked)
                            return Content("【授权码已被使用】无法授权，请联系平台！");
                  

                        IAopClient alipayClient = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app.AppId,
                        app.Merchant_Private_Key, "json", app.Version, app.SignType, app.Merchant_Public_key, "UTF-8", false);

                        AlipayOpenAuthTokenAppRequest request = new AlipayOpenAuthTokenAppRequest();

                        AlipayOpenAuthTokenAppModel model = new AlipayOpenAuthTokenAppModel();
                        model.GrantType = "authorization_code";
                        model.Code = authCode;

                        request.SetBizModel(model);

                        response = alipayClient.Execute(request);
                        if (response.Code == "10000")
                        {
                            if (!string.IsNullOrEmpty(StoreId))
                                SelfStore = db.DBStoreInfo.Where(s => s.ID == Convert.ToInt32(StoreId)).FirstOrDefault();
                      
                            
                            store = db.Store_GetByAliPayUserId(response.UserId);
                            if (store == null)
                            {
                                if(SelfStore!=null)
                                {
                                    SelfStore.AliPayAccount = response.UserId;
                                    SelfStore.AliPayAuthAppId = response.AuthAppId;
                                    SelfStore.AliPayAuthToke = response.AppAuthToken;

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
                                        Name = qr.Name,
                                        Remark = qr.Remark,
                                        RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Normal,
                                        QRId = qr.ID,
                                        Rate = qr.Rate,
                                        FromIQBAPP = app.AppId,
                                    };
                                    store.InitCreate();
                                    store.InitModify();
                                    db.DBStoreInfo.Add(store);
                                }

                               
                            }
                            else
                            {
                                if (SelfStore != null)
                                {
                                    SelfStore.AliPayAccount = response.UserId;
                                    SelfStore.AliPayAuthAppId = response.AuthAppId;
                                    SelfStore.AliPayAuthToke = response.AppAuthToken;

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
                                }
                              
                            }
                            qr.InitModify();
                            qr.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked;
                            Log.log("Auth QR Status:"+qr.RecordStatus);
                            db.SaveChanges();
                        }
                        else
                        {
                            Log.log("response Code"+ response.Code);
                            return Content("授权失败："+response.Msg);
                        }

                    }
                    string url = ConfigurationManager.AppSettings["IQBWX_SiteUrl"]+"/PP/Auth_Store?Rate="+store.Rate;
                    return Redirect(url);
                }
                catch (Exception ex)
                {
                    Log.log("Auth Response Error:" + ex.Message);
                    Log.log("Auth Response Inner Error:" + ex.InnerException);
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

        public ActionResult TestF2F(string No)
        {
            using (AliPayContent db = new AliPayContent())
            {
                EStoreInfo store = db.DBStoreInfo.Where(s => s.ID == 7).FirstOrDefault();
                return Content(callF2FPay(store, "10.00",No));
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
            string accessToken = this.getAccessToken(true);
            IQBCore.IQBPay.Models.Order.EOrderInfo _ppOrder;
            PPOrderPayNT notice = null;
            using (AliPayContent db = new AliPayContent())
            {
                _ppOrder = db.DBOrder.FirstOrDefault();
            }
            try
            {
                if (!string.IsNullOrEmpty(accessToken))
                {
                    notice = new PPOrderPayNT(accessToken, "o3nwE0qI_cOkirmh_qbGGG-5G6B0", _ppOrder);
                    notice.Push();
                }

            }
            catch
            {

            }
            return Content(notice.Push());
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

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">QRUserId</param>
        /// <param name="Amount"></param>
        /// <returns></returns>   
        public ActionResult F2FPay(string qrUserId, string Amount,string AliPayAccount,int PayType)
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
                    ErrorUrl += "代理二维码ID获取错误";
                    return Redirect(ErrorUrl);
                }
                using (AliPayContent db = new AliPayContent())
                {
                    //校验代理二维码
                    qrUser = db.DBQRUser.Where(q => q.ID == Id).FirstOrDefault();
                    if (qrUser == null)
                    {
                        ErrorUrl += "未找到对应的收款二维码";
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
                    if (ui.UserStatus == IQBCore.IQBPay.BaseEnum.UserStatus.JustRegister)
                    {
                        ErrorUrl += "代理被禁用";
                        return Redirect(ErrorUrl);
                    }
                    if(string.IsNullOrEmpty(ui.AliPayAccount))
                    {
                        ErrorUrl += "代理没有设置支付宝账户";
                        return Redirect(ErrorUrl);
                    }
                   

                    EStoreInfo store = null;
                    string selectStoreSql = @"select top 1 * from StoreInfo 
                                    where RecordStatus = 0 and RemainAmount> 0 and Channel=1
                                    order by NEWID() ";
                    if (PayType == 1)
                    {
                        

                        store = db.Database.SqlQuery<EStoreInfo>(selectStoreSql).FirstOrDefault();
                        //  store = db.Database.SqlQuery("")
                    }
                    else if(PayType ==0)
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
                    if(store == null)
                    {
                        ErrorUrl += "今日所有可用花呗已用完，请明天12点再尝试";
                        return Redirect(ErrorUrl);
                    }
                    //获取并校验商户 
                    ResultEnum status;

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
                                ErrorUrl += "当前收款账户转账失败,请更换！";
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

                    //  string Res = payMag.PayF2F(app, ui, store, Convert.ToSingle(Amount),out status);
                    string Res = payMag.PayF2FNew(app, ui, store, Amount, out status);
                   // base.Log.log("支付PayF2F：" + Res);
                    if (status == ResultEnum.SUCCESS)
                    {
                        //创建初始化订单
                        EOrderInfo order = payMag.InitOrder(qrUser, store,Convert.ToSingle(Amount), AliPayAccount);

                        if (!string.IsNullOrEmpty(qrUser.ParentOpenId))
                        {
                            EAgentCommission agentComm = payMag.InitAgentCommission(order, qrUser);   

                            RUserInfo parentUi = db.DBUserInfo.Where(u => u.OpenId == qrUser.ParentOpenId).Select(a => new RUserInfo()
                            {
                                OpenId = a.OpenId,
                                ParentAgentOpenId =a.parentOpenId,
                                AliPayAccount = a.AliPayAccount,
                                NeedFollowUp  = a.NeedFollowUp,
                                
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
                                if(L3Parent!=null)
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
                        ErrorUrl += "【支付失败】" + Res;
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
                res = payManager.DoSubAccount(BaseController.App, order, store, BaseController.SubAccount);
              
            }
            return Content(res.Body);
               
        }

        public ActionResult TransferToMainAccount()
        {
            AliPayManager payManager = new AliPayManager();
            string tid; 
            AlipayFundTransToaccountTransferResponse response =  payManager.DoTransferAmount(TransferTarget.Internal,BaseController.SubApp,"m18221882506@163.com",100.ToString("0.00"),PayTargetMode.AliPayAccount,out tid);

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



    }
}