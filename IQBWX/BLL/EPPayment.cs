using IQBWX.Common;
using IQBWX.Models.JsonData;
using IQBWX.Models.WX;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;
using WxPayAPI;

namespace IQBWX.BLL
{
    /// <summary>
    /// 参考文章：
    /// http://blog.csdn.net/u014742815/article/details/52879713
    /// </summary>
    public class EPPayment
    {
        private IQBLog log;
        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        private string nonce_str { get; set; }
        public string sign { get; set; }
        public string mch_id { get; set; }
        public string mch_billno { get; set; }
        public string wxappid { get; set; }

        public string send_name { get; set; }
        /// <summary>
        /// 发放的用户
        /// </summary>
        public string re_openid { get; set; }

        /// <summary>
        /// 付款金额，单位分
        /// </summary>
        public int total_amount { get; set; }
        /// <summary>
        /// 红包发放总人数 
        /// </summary>
        public int total_num { get; set; }

        /// <summary>
        ///红包祝福语
        /// </summary>
        public string wishing { get; set; }
        /// <summary>
        /// 调用接口的机器Ip地址
        /// </summary>
        public string client_ip { get; set; }
        /// <summary>
        /// 活动名字
        /// </summary>
        public string act_name { get; set; }

        public string remark { get; set; }
        /// <summary>
        /// 发放红包使用场景，红包金额大于200时必传
        /// </summary>
        public string scene_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string risk_info { get; set; }
        /// <summary>
        /// 资金授权商户号,服务商替特约商户发放时使用
        /// </summary>
        public string consume_mch_id { get; set; }

        public EPPayment()
        {
            log = new IQBLog();
        }
        public string PostEPPay(string toOpenId,decimal totalAmount)
        {
            this.nonce_str = StringHelper.GetRnd(30, true, true, true, false, "");
            this.mch_id = WxPayConfig.MCHID;            
            this.mch_billno = WxPayConfig.MCHID + DateTime.Now.ToString("yyyyMMdd")+this.GettimeStamp().ToString();
            this.wxappid = WxPayConfig.APPID;
            this.send_name = "全民秒贷";
            this.re_openid = toOpenId;
            this.total_amount = Convert.ToInt32(totalAmount * 100);
            this.total_num = 1;
            this.wishing = "提款秒到账";
            this.client_ip = WxPayConfig.ServerIp;
            this.act_name = "全民秒贷提款红包";
            this.sign = this.GetSign();
           
            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
            string data = this.GetXmlData();

            X509Certificate2 cer = new X509Certificate2(ConfigurationManager.AppSettings["CertPath"], WxPayConfig.MCHID, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

            return HttpHelper.PostwithCert(url, cer, data);
        }

        public WXError GetResult(string responseXml)
        {
            WXError error;
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(responseXml);
            XmlNode eleErrCode = xml.SelectSingleNode("//err_code");
            XmlNode eleErrCodeDes = xml.SelectSingleNode("//err_code_des");
            
            if (eleErrCode.InnerText.ToUpper() == "SUCCESS")
                return null;
            else
            {
                error = new WXError
                {
                    WXErrorCode = eleErrCode.InnerText,
                    errorMsg = eleErrCodeDes.InnerText,
                };
            }
            return error;

        }

        private string GetXmlData()
        {
            string xml = 
         @"<xml>
            <sign><![CDATA[{0}]]></sign>
            <mch_billno><![CDATA[{1}]]></mch_billno>
            <mch_id><![CDATA[{2}]]></mch_id>
            <wxappid><![CDATA[{3}]]></wxappid>
            <send_name><![CDATA[{4}]]></send_name>
            <re_openid><![CDATA[{5}]]></re_openid>
            <total_amount><![CDATA[{6}]]></total_amount>
            <total_num><![CDATA[1]]></total_num>
            <wishing><![CDATA[{7}]]></wishing>
            <client_ip><![CDATA[{8}]]></client_ip>
            <act_name><![CDATA[{9}]]></act_name>
            <remark></remark>
            <scene_id></scene_id>
            <consume_mch_id></consume_mch_id>
            <nonce_str><![CDATA[{10}]]></nonce_str>
            <risk_info></risk_info>
            </xml>";
            xml = string.Format(xml, this.sign,
                                                   this.mch_billno,
                                                   this.mch_id,
                                                   this.wxappid,
                                                   this.send_name,
                                                   this.re_openid,
                                                   this.total_amount,                                                  
                                                   this.wishing,
                                                   this.client_ip,
                                                   this.act_name,
                                                   this.nonce_str);
            log.log("xml:" + xml);

            return xml;

        }

        private string GetSign()
        {
            string KEY = "y58ohva8wsmw6dtshg5ccvkp9khan39g";
            string stringA = @"act_name={10}&client_ip={9}&mch_billno={2}&mch_id={1}&nonce_str={0}&re_openid={5}&send_name={4}&total_amount={6}&total_num={7}&wishing={8}&wxappid={3}";
            stringA = string.Format(stringA, nonce_str, mch_id, mch_billno, wxappid, send_name, re_openid, total_amount, total_num, wishing, client_ip, act_name);
            log.log("stringA:" + stringA);
            string SignTemp = stringA + "&" + "key=" + KEY;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(SignTemp))).ToUpper().Replace("-","");

            return sign;
        }

        private long GettimeStamp()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = DateTime.Now.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 7));
            return timeStamp;
        }

      

    }
}