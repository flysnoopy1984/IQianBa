using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using System.Web;

using System.Xml;

namespace IQBWX.Models.WX
{

    public class WxPayOrder
    {
        public string appId { get; set; }
        public string timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string package { get; set; }

        public string signType { get; set; }
        public string paySign { get; set; }

    
    }

    public class WXUserInfo
    {
      
        public int subscribe { get; set; }

        [MaxLength(32)]
        public string openid { get; set; }

        [MaxLength(40)]
        public string nickname { get; set; }
        public int sex { get; set; }
        [MaxLength(20)]
        public string language { get; set; }
        [MaxLength(20)]
        public string city { get; set; }
        [MaxLength(20)]
        public string province { get; set; }
        [MaxLength(20)]
        public string country { get; set; }

        [MaxLength(256)]
        public string headimgurl { get; set; }
        
        public string unionid { get; set; }

       
    }

    public class AccessToken
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }

        public string errcode { get; set; }

        public string errmsg { get; set; }
    }

    /// <summary>
    /// 微信接口XmlModel
    /// XML解析
    /// </summary>
    public class WXMessage
    {
        /// <summary>
        /// 消息接收方微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 图片链接，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 事件类型，subscribe(订阅/扫描带参数二维码订阅)、unsubscribe(取消订阅)、CLICK(自定义菜单点击事件) 、SCAN（已关注的状态下扫描带参数二维码）
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可以用来换取二维码
        /// </summary>
        public string Ticket { get; set; }

        public string toText(string content)
        {
            string xml = @"<xml>
                                    <ToUserName><![CDATA[{0}]]></ToUserName>
                                    <FromUserName><![CDATA[{1}]]></FromUserName>
                                    <CreateTime>{2}</CreateTime>
                                    <MsgType><![CDATA[{3}]]></MsgType>
                                    <Content><![CDATA[{4}]]></Content>
                                    </xml>";
            xml = string.Format(xml, this.FromUserName, this.ToUserName, 12345678, "text", content);
            return xml;
        }

        public string toPicText(string picUrl)
        {            

            string xml = @"<xml>
                <ToUserName><![CDATA[{0}]]></ToUserName>
                <FromUserName><![CDATA[{1}]]></FromUserName>
                <CreateTime>12345678</CreateTime>
                <MsgType><![CDATA[news]]></MsgType>
                <ArticleCount>1</ArticleCount>
                <Articles>
                <item>
                <Title><![CDATA[我的二维码]]></Title> 
                <Description><![CDATA[点击详情获取酷炫二维码标记]]></Description>
                <PicUrl><![CDATA[{2}]]></PicUrl>
                <Url><![CDATA[{3}]]></Url>
                </item>
                </Articles>
                </xml>";
            xml = string.Format(xml, this.FromUserName, this.ToUserName, picUrl,"http://wx.iqianba.cn/html5/MemberQR");
            return xml;
        }

      

        public void LoadXml(string xml)
        {
          
            XmlNode node = null;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);

            XmlElement rootele = xmldoc.DocumentElement;
            node = rootele.SelectSingleNode("ToUserName");
            if(node!=null)
                this.ToUserName = node.InnerText;

            node = rootele.SelectSingleNode("FromUserName");
            if (node != null)
                this.FromUserName = node.InnerText;

            node = rootele.SelectSingleNode("MsgType");
            if (node != null)
                this.MsgType = node.InnerText;

            node = rootele.SelectSingleNode("Event");
            if (node != null)
                this.Event = node.InnerText.ToLower();

            node = rootele.SelectSingleNode("EventKey");
            if (node != null)
                this.EventKey = node.InnerText;

            node = rootele.SelectSingleNode("Ticket");
            if (node != null)
                this.Ticket = node.InnerText;
         
        }
    }

    public class WXQR
    {
        public string expire_seconds { get; set; }
        public string action_name { get; set; }
        public string action_info { get; set; }
        public string scene_id { get; set; }
        public string scene_str { get; set; }

    }

    public class WXQRResult
    {
        public string ticket { get; set; }
        public string url { get; set; }

       
    }

    public class WXTemplate
    {

    }




}