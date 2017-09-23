using IQBWX.Common;
using IQBWX.Controllers;
using IQBWX.Models.User;
using IQBWX.Models.WX.Template;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxPayAPI;

namespace IQBWX.BLL.NT
{
    public abstract class Notification<T>
    {
        private IQBLog log = null;
        private string _accessToken;
        public Notification(string accessToken)
        {
            log = new IQBLog();
            if (accessToken == null)
                accessToken = JsApiPay.GetAccessToken();
            _accessToken = accessToken;
        }

        protected abstract T SetData();

        public void Push()
        {
            try
            {                       
                string strUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + _accessToken;
                string template;
            
                var data = SetData();
                template = JsonConvert.SerializeObject(data);
                log.log("Push Template File: " + template);
                string result = HttpHelper.RequestUrlSendMsg(strUrl, HttpHelper.HttpMethod.Post, template);
                log.log("Push Result: " + result);
            }
            catch (Exception ex)
            {
                log.log("Push Error: " + ex.Message);
                log.log("Push Inner Error: " + ex.StackTrace);
            }
        }

      
    }
}