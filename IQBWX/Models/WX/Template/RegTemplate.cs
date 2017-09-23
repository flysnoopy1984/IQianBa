using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.WX.Template
{
    public class RegTemplate:BaseTemplate<RegTemplate>
    {

        public object data;

        public RegTemplate GenerateData(string toUserOpenId,string nickName,DateTime RegDate)
        {
            var data = new
            {
                first = new TemplateField() { value = "恭喜您，通过您的二维码成功锁定一位下级会员！",color= "#007ACC" },
                keyword1 = new TemplateField() { value = nickName ,},
                keyword2 = new TemplateField() { value = RegDate.ToString() },
                remark = new TemplateField { value = "请再接再厉，记得提醒TA成为会员！", color = "#EB6B13" },
            };

            RegTemplate obj = base.InitObject(toUserOpenId, "", "3XbpeQncPdidDErnGYYa7YG79yLq0iyhI8oJ2YqSUTE");
            obj.data = data;
            return obj;
        }
    }
}