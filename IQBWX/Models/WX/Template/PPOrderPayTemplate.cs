using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.WX.Template
{
    public class PPOrderPayTemplate:BaseTemplate<PPOrderPayTemplate>
    {
        public object data;

        public PPOrderPayTemplate GenerateData(string toUserOpenId, IQBCore.IQBPay.Models.Order.EOrderInfo ppOrder)
        {
            string first = string.Format("有用户扫描了您的收款二维码");
            string remark = string.Format("支付宝流水号：{0} \n买家账号：{1}\n二维码扣点率：{2}%\n实际收款：{3}",
                                         ppOrder.AliPayOrderNo,
                                         ppOrder.BuyerAliPayLoginId,
                                         ppOrder.Rate,
                                         ppOrder.RealTotalAmount);
            var data = new
            {
                first = new TemplateField() { value = first,color= "#EB6B13" },
                keyword1 = new TemplateField() { value = ppOrder.TransDate.ToString() },
                keyword2 = new TemplateField() { value = ppOrder.SellerName },
                keyword3 = new TemplateField() { value = ppOrder.TotalAmount.ToString() },
                keyword4 = new TemplateField() { value = ppOrder.OrderNo.ToString() },
                keyword5 = new TemplateField() { value ="" },
                remark = new TemplateField { value = remark, color = "#007ACC" },
            };

            PPOrderPayTemplate obj = base.InitObject(toUserOpenId, "", "m6qNTQKBPx7vrk2oRdlfalvwBF_4XFfQ0eIftyY0cKI");
            obj.data = data;
            return obj;

        }
    }
}