using IQBCore.Common.Helper;
using IQBCore.IQBWX.Models.Json.WXMedia;
using IQBCore.IQBWX.Models.Json.WXMedia.News;
using IQBCore.IQBWX.Models.Json.WXMedia.News;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBWX.Controllers
{
    public class WXHelperController : WXBaseController
    {
        // GET: WXHelper
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendMessage()
        {
            return View();
        }

        public ActionResult Send()
        {

            return View();
        }

        public JOMedia_News GetNews(string access_token)
        {
            
            string posturl = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + access_token;

            JIMedia obj = new JIMedia();
            obj.type = "news";
            obj.offset = "0";
            obj.count = "10";

            string data = JsonConvert.SerializeObject(obj);

            string responseResult = HttpHelper.RequestUrlSendMsg(posturl, HttpHelper.HttpMethod.Post, data);

            JOMedia_News result = JsonConvert.DeserializeObject<JOMedia_News>(responseResult);
            return result;

        }

        [HttpPost]
        public ActionResult MediaListQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);
            string type = Request["type"];

            string access_token = this.getAccessToken();
            string posturl = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + access_token;

            JIMedia obj = new JIMedia();
            obj.type = type;
            obj.offset = pageIndex.ToString();
            obj.count = pageSize.ToString();
            string data = JsonConvert.SerializeObject(obj);

            string responseResult = HttpHelper.RequestUrlSendMsg(posturl, HttpHelper.HttpMethod.Post, data);
            if (type == "news")
            {
                JOMedia_News result =JsonConvert.DeserializeObject<JOMedia_News>(responseResult);
         
                return Json(result);
            }
            else
            {
                JOMedia result = JsonConvert.DeserializeObject<JOMedia>(responseResult);
                return Json(result);
            }
        }

        public ActionResult MediaList()
        {
           
            return View();
        }
    }
}