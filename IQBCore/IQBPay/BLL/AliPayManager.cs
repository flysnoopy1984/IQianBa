using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.BLL
{
    public class AliPayManager
    {
        public string PayF2F(EAliPayApplication app, EQRUser qrUser, EStoreInfo storeInfo, float TotalAmount, out ResultEnum status)
        {
            string result = "";
            /*
            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(AliPayConfig.serverUrl, AliPayConfig.appId, AliPayConfig.merchant_private_key, AliPayConfig.version,
                           AliPayConfig.sign_type, AliPayConfig.alipay_public_key, AliPayConfig.charset);
                           */
            IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(app.ServerUrl, app.AppId, app.Merchant_Private_Key, app.Version,
                                       app.SignType, app.Merchant_Public_key, app.Charset);

            F2FPayHandler handler = new F2FPayHandler();

            AlipayTradePrecreateContentBuilder builder = handler.BuildPrecreateContent(app,storeInfo.AliPayAccount, TotalAmount.ToString());

            AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder);

            status = precreateResult.Status;

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
            return result;
        }
    }
}
