using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Common.Helper
{
    public class HttpHelper
    {

        public enum HttpMethod
        {
            Post,
            Get
        }

        public static string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static T Get<T>(string url, bool needLog = false)
        {
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "get";
                request.Timeout = 2000;
                HttpWebResponse response;

                using (response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);

                    result = sr.ReadToEnd();
                    if (needLog)
                    {
                        IQBLog log = new IQBLog();
                        log.log("HttpHelper Get Method:" + result);
                    }


                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static string RequestUrlSendMsg(string url, HttpMethod method, string JSONData, String ContentType = "text/html",string charset="utf-8")
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = method.ToString();
            request.ContentType = ContentType;
            
            request.Headers.Add("charset", charset);
            request.Headers.Add("CharacterEncoding", charset);
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);
            //声明一个HttpWebRequest请求  
            request.Timeout = 90000;
            //设置连接超时时间  
            request.Headers.Set("Pragma", "no-cache");
            request.Headers.Set("Cache-Control", "no-cache");
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }

        public static string PostwithCert(string url, X509Certificate2 cer, string data)
        {
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.ClientCertificates.Add(cer);
            webrequest.Method = "post";

            byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
            webrequest.ContentLength = postdatabyte.Length;
            Stream stream;
            stream = webrequest.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();
            return responseContent;
        }
    }
}
