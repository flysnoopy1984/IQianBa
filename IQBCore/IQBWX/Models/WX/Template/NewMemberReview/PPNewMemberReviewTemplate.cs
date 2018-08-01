using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.NewMemberReview
{
    /*
     Gyow2Z8L4u9Kk0syOmULIo7MziwhbVUZhgwjvRbLKXo
    {{first.DATA}}
    会员ID：{{keyword1.DATA}}
    时间：{{keyword2.DATA}}
    {{remark.DATA}}     
    */
    public class PPNewMemberReviewTemplate : BaseTemplate<PPNewMemberReviewTemplate>
    {
        public object data;

        public PPNewMemberReviewTemplate GenerateData(string toUserOpenId,string newOpenId,string UserName,string JointDateTime)
        {
            string first = string.Format("有会员扫描了您的邀请码！等待您的审核");
            string remark = string.Format("请点击审核");
            var data = new
            {
                first = new TemplateField() { value = first, color = "#EB6B13" },
                keyword1 = new TemplateField() { value = UserName },
                keyword2 = new TemplateField() { value = JointDateTime },
                remark = new TemplateField { value = remark, color = "#007ACC" },
            };
            string host = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];

            if (string.IsNullOrEmpty(host))
                host = ConfigurationManager.AppSettings["Site_WX"];

            string url =string.Format(host + "/PP/NewMemberReview?nOpenId={0}",
                newOpenId);

            PPNewMemberReviewTemplate obj = base.InitObject(toUserOpenId, url, "Gyow2Z8L4u9Kk0syOmULIo7MziwhbVUZhgwjvRbLKXo");
            obj.data = data;
            return obj;
          
        }
    }
}
