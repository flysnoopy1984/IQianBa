using IQBCore.IQBWX.BaseEnum;
using IQBWX.Common;
using IQBWX.DataBase;
using IQBWX.Models.Crawler;
using IQBWX.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBWX.Controllers
{
    public class InfoController : WXBaseController
    {
        /*
         *http://hjc025.xiaoyun.com/data/attachment/portal/201704/02/192713w1cv7x2dz6apja5j.png
        * https://www.hjc025.com/data/attachment/portal/201704/24/142341krb2yrr44y27kb2y.png
        */

        [HttpPost]
        public ActionResult DetailData()
        {
            RInfoDetail detail = null;
            int id = Convert.ToInt32(Request["id"]);
            if (id > 0)
            {
                using (Html5Content db = new Html5Content())
                {
                    detail = db.GetDetailResult(id);
                    detail.ArticleContent = detail.ArticleContent.Replace("data/attachment/portal", "https://www.hjc025.com/data/attachment/portal");
                    detail.ArticleContent = detail.ArticleContent.Replace("<img", "<img style='width:625px'");
                }
            }
            if (detail == null)
                detail = new RInfoDetail();
            return Json(detail);
        }
        public ActionResult Detail()
        {
               
            return View();
        }


        public ActionResult Summery()
        {
            return View();
            if (base.CheckIsMember())
            {
                return View();
            }
            else
            {
                return RedirectToRoute(new { Controller = "Home", action = "ErrorMessage", code = Errorcode.NotMember });             
            }
        }
        [HttpPost]
        public ActionResult SummeryData()
        {
            int pageIndex = Convert.ToInt32(Request["Page"]);
            List<EInfoSummery> list = null;
            using (Html5Content db = new Html5Content())
            {
                list = db.SummeryPagination(pageIndex);
            }
            return Json(list);
        }

        // GET: Info
        public ActionResult Index()
        {
            return View();
        }
    }
}