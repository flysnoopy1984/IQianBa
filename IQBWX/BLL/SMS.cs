using IQBWX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.BLL
{
    public class SMS
    {
        public string TestSMS()
        {
            string url = string.Format("http://http.asp.sh.cn/GetInfo.do?Username={0}&Password={1}", "dingyunlong", "DING0208");
            return HttpHelper.HttpGet(url);
        }
       
    }
}