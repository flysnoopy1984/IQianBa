using IQBCore.IQBPay.Models.O2O;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.SettleToUser
{
    /// <summary>
    /// i_5j4EheOGvq6bBDL_5SLKEW6LjJWB11iy61bKogVlk
    /// </summary>
    public class SettleToUserTemplate : BaseTemplate<SettleToUserTemplate>
    {
        public object data;
        /*
         {{first.DATA}}
        订单号：{{keyword1.DATA}}
        结算金额：{{keyword2.DATA}}
        结算时间：{{keyword3.DATA}}
        {{remark.DATA}}
        */
        public SettleToUserTemplate GenerateData(string toUserOpenId,EO2OOrder o2oOrder)
        {

            double amt;

            amt = o2oOrder.OrderAmount * ((100 - o2oOrder.MarketRate) / 100); 
            string first = string.Format("订单已被出库商/商户结算，请准备打钱给用户");

            

            string remark = string.Format( "用户手机号:{0}", o2oOrder.UserPhone);
            var data = new
            {


                first = new TemplateField() { value = first, color = "#1364EB" },
                keyword1 = new TemplateField() { value = o2oOrder.O2ONo },
                keyword2 = new TemplateField() { value = amt.ToString("0.00")+"元 (将转账给用户)" },
                keyword3 = new TemplateField() { value = "准备结算" },

                remark = new TemplateField { value = remark, color = "#FF7F27" },
            };
          
            string url = ConfigurationManager.AppSettings["Main_SiteUrl"] + "/O2OWap/UploadOrder?act=review&aoId={0}&OrderNo={1}";

            url = string.Format(url, toUserOpenId, o2oOrder.O2ONo);

            SettleToUserTemplate obj = base.InitObject(toUserOpenId, url, "i_5j4EheOGvq6bBDL_5SLKEW6LjJWB11iy61bKogVlk");
            obj.data = data;
            return obj;

        }
    }
}
