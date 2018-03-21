using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.ConfirmSign
{
    /*
      {{first.DATA}}
      订单编号：{{keyword1.DATA}}
      商品信息：{{keyword2.DATA}}
      {{remark.DATA}}
     */
    public class PPConfirmSignTemplate : BaseTemplate<PPConfirmSignTemplate>
    {
        public object data;


        public PPConfirmSignTemplate GenerateData(string toUserOpenId, IQBCore.IQBPay.Models.Order.EOrderInfo ppOrder)
        {
            string first = string.Format("用户扫描了您的收款二维码");
            string remark = string.Format("支付宝流水号：{0} \n买家账号：{1} \n实际收款：{2}",
                                         ppOrder.AliPayOrderNo,
                                         ppOrder.BuyerAliPayLoginId,
                                         // ppOrder.Rate,
                                         ppOrder.RateAmount);
            var data = new
            {
                first = new TemplateField() { value = first, color = "#EB6B13" },
                keyword1 = new TemplateField() { value = ppOrder.TransDate.ToString() },
                keyword2 = new TemplateField() { value = "码商提供" },
                keyword3 = new TemplateField() { value = ppOrder.TotalAmount.ToString() },
                keyword4 = new TemplateField() { value = ppOrder.OrderNo.ToString() },
                keyword5 = new TemplateField() { value = "" },
                remark = new TemplateField { value = remark, color = "#007ACC" },
            };

            PPConfirmSignTemplate obj = base.InitObject(toUserOpenId, "", "94I1M_Tszb8y_orzdzeXjPrSc4A3giS4cEXhgv6btCg");
            obj.data = data;
            return obj;

        }
    }
}
