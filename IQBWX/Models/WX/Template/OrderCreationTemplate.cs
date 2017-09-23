using IQBWX.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.WX.Template
{
    public class OrderCreationTemplate : BaseTemplate<OrderCreationTemplate>
    {

        public object data;

        public OrderCreationTemplate GenerateData(string toUserOpenId,EOrderLine order,string nickName,decimal receive)
        {
            string first = string.Format("你的直属下级会员{0}正在下单！", nickName);
            string remark = string.Format("待(他/她)付款后预计您将获得的红包为：{0}元", receive);
            var data = new
            {
                first = new TemplateField() { value = first, color= "#007ACC" },
                keyword1 = new TemplateField() { value = "全民秒贷微信店"},
                keyword2 = new TemplateField() { value = order.CreateDateTime.ToString() },
                keyword3 = new TemplateField() { value=order.ItemName },
                keyword4 = new TemplateField() { value = order.LineAmount.ToString()},
                remark = new TemplateField { value = remark, color = "#EB6B13" },
            };

            OrderCreationTemplate obj = base.InitObject(toUserOpenId, "", "nUagupqOkjdMbulDGhknYvZgsTUVlJeWgvDFFk-r_7U");
            obj.data = data;
            return obj;
            
        }
   
    }
}