using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.InviteCode
{
    public class PPInviteCodeTemplate : BaseTemplate<PPInviteCodeTemplate>
    {
        public object data;

        public PPInviteCodeTemplate GenerateData(string toUserOpenId, int origNum, int curNum)
        {
            string first = string.Format("恭喜您！您的邀请码名额增加了");
            string remark = string.Format("系统每3天将清理没有业绩的成员，您可在【我的团队】->【总人数】看到邀请码名额变动情况。");
            var data = new
            {
                first = new TemplateField() { value = first, color = "#EB6B13" },
                keyword1 = new TemplateField() { value = origNum.ToString() },
                keyword2 = new TemplateField() { value = curNum.ToString() },
                keyword3 = new TemplateField() { value = DateTime.Now.ToString("yyyy-MM-dd") },
           
                remark = new TemplateField { value = remark, color = "#007ACC" },
            };

            PPInviteCodeTemplate obj = base.InitObject(toUserOpenId, "", "OibcI38z2dvaO7s7vaxoI2_o0B8y_UQrOmfnPSqE0fE");
            obj.data = data;
            return obj;

        }
    }
}
