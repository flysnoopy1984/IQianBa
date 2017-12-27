using IQBCore.Common.Helper;
using IQBCore.IQBWX.Models.Json.WXMedia;
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

        [HttpPost]
        public ActionResult MediaListQuery()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            int pageSize = Convert.ToInt32(Request["PageSize"]);

            string access_token = this.getAccessToken();
            string posturl = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + access_token;

            JIMedia obj = new JIMedia();
            obj.type = "image";
            obj.offset = pageIndex.ToString();
            obj.count = pageSize.ToString();
            string data = JsonConvert.SerializeObject(obj);

            string responseResult = HttpHelper.RequestUrlSendMsg(posturl, HttpHelper.HttpMethod.Post, data);
            JOMedia result = JsonConvert.DeserializeObject<JOMedia>(responseResult);

            return Json(result);

        }

        public ActionResult MediaList()
        {
           
            return View();
        }
    }
}