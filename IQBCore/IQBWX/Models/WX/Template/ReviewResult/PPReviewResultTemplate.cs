using IQBCore.IQBPay.Models.O2O;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.ReviewResult
{
    public class PPReviewResultTemplate : BaseTemplate<PPReviewResultTemplate>
    {
        public object data;
        /*
         {{first.DATA}}
        账号名称：{{keyword1.DATA}}
        审核状态：{{keyword2.DATA}}
        审核时间：{{keyword3.DATA}}
        {{remark.DATA}}
        */
        public PPReviewResultTemplate GenerateData(string toUserOpenId, RO2OOrder o2oOrder)
        {
            string nextStep = "请注意您的商城订单，如果已被签收，请到系统确认签收!";

            string first = string.Format("订单审核通过！请根据以下提示继续操作！");

            string OrderStatus = "审核通过";

            if (o2oOrder.O2OOrderStatus == IQBPay.BaseEnum.O2OOrderStatus.OrderRefused)
            {
                first = string.Format("订单被驳回！点击详情，重新提交订单");
                nextStep = "【驳回理由】\n"+o2oOrder.RejectReason;
                OrderStatus = "审核驳回";
            }
           


            
            string remark = string.Format("所属中介:{0} \n商品名称:{1}\n金额:{2}\n{3}",
                                         o2oOrder.AgentName,
                                         o2oOrder.ItemName,
                                         o2oOrder.OrderAmount,
                                        nextStep

                                         );
            var data = new
            {
                
             
                first = new TemplateField() { value = first, color = "#1364EB" },
                keyword1 = new TemplateField() { value = o2oOrder.User },
                keyword2 = new TemplateField() { value = OrderStatus },
                keyword3 = new TemplateField() { value = o2oOrder.ReviewDateTime.ToString("yyyy-MM-dd HH:mm:ss") },

                remark = new TemplateField { value = remark, color = "#007ACC" },
            };
            if(o2oOrder.O2OOrderStatus == IQBPay.BaseEnum.O2OOrderStatus.OrderRefused)
            {
                data.first.color = "#EB1313";
            }
            
            string url = ConfigurationManager.AppSettings["Main_SiteUrl"] + "/O2OWap/OrderDetail?aoId={0}&O2ONo={1}";

            url = string.Format(url, o2oOrder.AgentOpenId, o2oOrder.O2ONo);

            PPReviewResultTemplate obj = base.InitObject(toUserOpenId, url, "ZDHFFrCe-8DefA96Ar8QTagfZ8tinOJbWio0wd1ivPw");
            obj.data = data;
            return obj;

        }
    }
}
