using IQBCore.IQBPay.Models.Simple;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.InviteCode
{
    /// <summary>
    /// OibcI38z2dvaO7s7vaxoI2_o0B8y_UQrOmfnPSqE0fE
    /// 
    /// 原来用作邀请码名额，现用于费率变更
    /// </summary>
    public class PPInviteCodeTemplate : BaseTemplate<PPInviteCodeTemplate>
    {
        public object data;

        public PPInviteCodeTemplate GenerateData(string toUserOpenId, List<SFee> FeeList)
        {
            string first = string.Format("费率调整");
            string remark = string.Format("请点击查看。如有问题请联系平台");
            string OrigStr = "【原费率】";
            string AfterStr = "【现费率】";
            foreach(SFee fee in FeeList)
            {
                if(fee.QRType == IQBPay.BaseEnum.QRReceiveType.Small)
                {
                    OrigStr += string.Format("花呗:{0}% ",fee.OrigFeeRate);
                    AfterStr += string.Format("花呗:{0}% ",fee.AdjustedFeeRate);
                }
                else if(fee.QRType == IQBPay.BaseEnum.QRReceiveType.CreditCard)
                {
                    OrigStr += string.Format("\n信用卡:{0}% ", fee.OrigFeeRate);
                    AfterStr += string.Format("\n信用卡:{0}% ", fee.AdjustedFeeRate);
                }
            }
            var data = new
            {
                first = new TemplateField() { value = first, color = "#EB6B13" },
                keyword1 = new TemplateField() { value = OrigStr },
                keyword2 = new TemplateField() { value = AfterStr },
                keyword3 = new TemplateField() { value = DateTime.Now.ToString("yyyy-MM-dd") },
           
                remark = new TemplateField { value = remark, color = "#007ACC" },
            };
            string host = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];

            if (string.IsNullOrEmpty(host))
                host = ConfigurationManager.AppSettings["Site_WX"];

            string url = host + "/PP/Agent_QR_ARList";

            PPInviteCodeTemplate obj = base.InitObject(toUserOpenId, url, "OibcI38z2dvaO7s7vaxoI2_o0B8y_UQrOmfnPSqE0fE");
            obj.data = data;
            return obj;

        }
    }
}
