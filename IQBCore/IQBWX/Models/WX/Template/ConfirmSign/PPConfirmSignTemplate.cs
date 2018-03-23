using IQBCore.IQBPay.Models.O2O;
using System;
using System.Collections.Generic;
using System.Configuration;
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


        public PPConfirmSignTemplate GenerateData(string toUserOpenId, RO2OOrder o2oOrder)
        {
            string first = string.Format("有订单已签收，请及时发货！");
            string remark = string.Format("订单金额:{0}\n订单创建时间:{1}", 
                                         o2oOrder.OrderAmount,
                                         o2oOrder.CreateDateTimeStr
                                         );
            var data = new
            {
                first = new TemplateField() { value = first, color = "#EB6B13" },
                keyword1 = new TemplateField() { value = o2oOrder.MallOrderNo },
                keyword2 = new TemplateField() { value = o2oOrder.ItemName },

                remark = new TemplateField { value = remark, color = "#007ACC" },
            };
          //  string url = ConfigurationManager.AppSettings["Main_SiteUrl"] + "/O2OWap/UploadOrder?aoId={0}&OrderNo={1}&OrderStatus={2}";

         //   url = string.Format(url, o2oOrder.AgentOpenId, o2oOrder.O2ONo, Convert.ToInt32(o2oOrder.O2OOrderStatus));

            PPConfirmSignTemplate obj = base.InitObject(toUserOpenId, "", "Hwfh8EsQtI-az0dorSBZEVi3iuvg3lh8dpCznayXHdg");
            obj.data = data;
            return obj;

        }
    }
}
