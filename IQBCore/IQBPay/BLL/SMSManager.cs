using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.SMS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.BLL
{
    public class SMSManager
    {
        private const String host = "http://smsapi.api51.cn";
        private const String path = "/single_sms/";
        private const String method = "POST";
        private const String appcode = "d595292b73f8415691cd09b90ca04d17";

        public SMSResult_API51 PostSMS_API51(InSMS InSMS, ref ESMSLog smsLog)
        {
            try
            {
                smsLog.APPName = "API51";
                smsLog.UserPhone = InSMS.PhoneNumber;

                String querys = "";
                String bodys = "mobile={0}&params={1}&sign={2}&tpl_id={3}";
                String url = host + path;
                HttpWebRequest httpRequest = null;
                HttpWebResponse httpResponse = null;
                bodys = string.Format(bodys, InSMS.PhoneNumber, InSMS.Parameters, InSMS.Sign, InSMS.Tpl_id);

                smsLog.RequestMessage = bodys;

                if (0 < querys.Length)
                {
                    url = url + "?" + querys;
                }

                if (host.Contains("https://"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                }
                else
                {
                    httpRequest = (HttpWebRequest)WebRequest.Create(url);
                }
                httpRequest.Method = method;
                httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
                //根据API的要求，定义相对应的Content-Type
                httpRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                if (0 < bodys.Length)
                {
                    byte[] data = Encoding.UTF8.GetBytes(bodys);
                    using (Stream stream = httpRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                try
                {
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    httpResponse = (HttpWebResponse)ex.Response;
                }

             
                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                string json = reader.ReadToEnd();

                smsLog.ResponseMessage = bodys;
                smsLog.SendDateTime = DateTime.Now;
                
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(json);
                SMSResult_API51 result = serializer.Deserialize<SMSResult_API51>(new JsonTextReader(sr));
                return result;


            }
            catch (Exception ex)
            {
                smsLog.Exception = "SMSManager Post:" + ex.Message;
                throw ex;
            }
          
        }

      
           
    


        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public Boolean PostAliSMS()
        {
            return true;
        }
    }
}
