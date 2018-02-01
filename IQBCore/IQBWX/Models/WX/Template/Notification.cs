using IQBCore.Common.Helper;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace IQBCore.IQBWX.Models.WX.Template
{
    public abstract class Notification<T>
    {
        private IQBLog log = null;
        private string _accessToken;
        public Notification(string accessToken)
        {
            log = new IQBLog();
            //if (accessToken == null)
            //    accessToken = JsApiPay.GetAccessToken();
            _accessToken = accessToken;
        }

        protected abstract T SetData();

        public string Push()
        {
            try
            {                       
                string strUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + _accessToken;
                string template;
            
                var data = SetData();
                template = JsonConvert.SerializeObject(data);
               //  log.log("Push Template File: " + template);
                string result = HttpHelper.RequestUrlSendMsg(strUrl, HttpHelper.HttpMethod.Post,template, "application/json");
               // log.log("Push Result: " + result);

                return result;
            }
            catch (Exception ex)
            {
                log.log("Push Error: " + ex.Message);
                log.log("Push Inner Error: " + ex.StackTrace);
            }
            return "";
        }

      
    }
}