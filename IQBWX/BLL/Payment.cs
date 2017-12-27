using IQBCore.Common.Helper;
using IQBWX.Common;
using IQBWX.Models.Product;
using IQBWX.Models.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxPayAPI;

namespace IQBWX.BLL
{
    public class Payment
    {
        private IQBLog log =null;
        public Payment()
        {
            log = new IQBLog();
        }
       
        public WxPayOrder PostPay(HttpContextBase httpContext, EItemInfo item, string openId)
        {
           
            WxPayOrder aOrder;
            JsApiPay jsApiPay = new JsApiPay(httpContext);

            jsApiPay.openid = openId;
            jsApiPay.total_fee = item.SalesPrice;

            try
            {

                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(item.Description);
                WxPayData wxJsApiParam = jsApiPay.GetJsApiParameters2();

                aOrder = new WxPayOrder()
                {
                    appId = wxJsApiParam.GetValue("appId").ToString(),
                    nonceStr = wxJsApiParam.GetValue("nonceStr").ToString(),
                    package = wxJsApiParam.GetValue("package").ToString(),
                    paySign = wxJsApiParam.GetValue("paySign").ToString(),
                    signType = "MD5",
                    timeStamp = wxJsApiParam.GetValue("timeStamp").ToString(),

                };
            }
            catch
            {
                aOrder = new WxPayOrder()
                {
                    appId = "",
                    nonceStr = "",
                    package = "",
                    paySign = "",
                    timeStamp = "",
                    signType = "MD5",

                };
            }
            return aOrder;
        }
    }
}