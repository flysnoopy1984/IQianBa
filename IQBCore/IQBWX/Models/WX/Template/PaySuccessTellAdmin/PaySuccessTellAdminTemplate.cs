using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.PaySuccessTellAdmin
{

 /*
{{first.DATA}}
付款金额：{{keyword1.DATA
}}
交易单号：{{keyword2.DATA}}
{{remark.DATA}}
*/
    /// <summary>
    /// 用户通知管理员下单成功
    /// mCRMZ-m4KIJ787073hYRscLXpHV4V4bflF1Ar8nojdg    
    /// </summary>
    public class PaySuccessTellAdminTemplate:BaseTemplate<PaySuccessTellAdminTemplate>
    {
        public object data;

        public PaySuccessTellAdminTemplate GenerateData(string toUserOpenId, IQBCore.IQBPay.Models.Order.EOrderInfo ppOrder)
        {
            string first = string.Format("用户支付成功");
            string remark = string.Format("买家手机号：{0} \n代理名：{1}",                                     
                                         ppOrder.BuyerMobilePhone,
                                         ppOrder.AgentName
                                         
                                      );
            var data = new
            {
                first = new TemplateField() { value = first, color = "#EB6B13" },
                keyword1 = new TemplateField() { value = ppOrder.TotalAmount.ToString() },
                keyword2 = new TemplateField() { value = ppOrder.OrderNo },
               
                remark = new TemplateField { value = remark, color = "#007ACC" },
            };

            string host = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];

            if (string.IsNullOrEmpty(host))
                host = ConfigurationManager.AppSettings["Site_WX"];

            string url = string.Format(host + "/PPAdmin/ReviewPaySuccess?OrderNo={0}",
                ppOrder.OrderNo);


            PaySuccessTellAdminTemplate obj = base.InitObject(toUserOpenId,url, "mCRMZ-m4KIJ787073hYRscLXpHV4V4bflF1Ar8nojdg");
            obj.data = data;
            return obj;

        }
    }
}
