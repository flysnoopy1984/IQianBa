using IQBWX.Models.Order;
using IQBWX.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.WX.Template
{
    public class OrderPaidSuccessTemplate:BaseTemplate<OrderPaidSuccessTemplate>
    {
        public object data;
        public OrderPaidSuccessTemplate GenerateData(EMemberInfo mi, EMemberInfo pmi,EOrderLine order, decimal receive)
        {
            string first = string.Format("恭喜您，您的直属下级会员{0}在{1}时，购买的【{2}】已经支付成功！", mi.nickname, order.CreateDateTime.ToString(),order.ItemName);

            string remark = "您获得了商品佣金，请去查看我的余额吧";
            var data = new
            {
                first = new TemplateField() { value = first, color = "#007ACC" },
                keyword1 = new TemplateField() { value = order.ItemName },
                keyword2 = new TemplateField() { value = receive.ToString()+ "元" },
                keyword3 = new TemplateField() { value = "已支付"},              
                remark = new TemplateField { value = remark, color = "#EB6B13" },
            };
            OrderPaidSuccessTemplate obj = base.InitObject(pmi.openId, "", "ACRhW3PTfBEhnsXYuzpOIWSD7mCaE41CSA8XGpoJnws");
            obj.data = data;
            return obj;

        }
    }
}