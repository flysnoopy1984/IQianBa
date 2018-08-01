using IQBCore.IQBPay.Models.O2O;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.ReviewRemind
{

    /// <summary>
    /// Hwfh8EsQtI-az0dorSBZEVi3iuvg3lh8dpCznayXHdg
    /// </summary>
    public class PPReviewRemindTemplate: BaseTemplate<PPReviewRemindTemplate>
    {
        public object data;

        /*
         {{first.DATA}}
         客户：{{keyword1.DATA}}
         订单金额：{{keyword2.DATA}}
         {{remark.DATA}}
         */
        public PPReviewRemindTemplate GenerateData(string toUserOpenId,RO2OOrder o2oOrder)
        {
            string first = string.Format("有订单需要审核！");
            string remark = string.Format("代理:{0} \n出库商:{1} \n创建时间:{2}",
                                         o2oOrder.AgentName,
                                         o2oOrder.WHName,
                                         o2oOrder.CreateDateTimeStr
                                         );
            var data = new
            {
                first = new TemplateField() { value = first, color = "#EB6B13" },
                keyword1 = new TemplateField() { value = o2oOrder.User},
                keyword2 = new TemplateField() { value = o2oOrder.OrderAmount.ToString() },
                
                remark = new TemplateField { value = remark, color = "#007ACC" },
            };
            string url = ConfigurationManager.AppSettings["Main_SiteUrl"]+ "/O2OWap/UploadOrder?act=review&aoId={1}&OrderNo={0}";

            url = string.Format(url, o2oOrder.O2ONo,toUserOpenId);

            PPReviewRemindTemplate obj = base.InitObject(toUserOpenId, url, "Hwfh8EsQtI-az0dorSBZEVi3iuvg3lh8dpCznayXHdg");
            obj.data = data;
            return obj;

        }
    }
}
