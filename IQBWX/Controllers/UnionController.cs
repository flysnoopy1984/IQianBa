using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBWX.Controllers
{
    public class UnionController : WXBaseController
    {
        // GET: Union
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 胖蚂蚁
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjPMY()
        {
            return View();
        }

        /// <summary>
        /// 卡卡收银台
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjKK()
        {
            return View();
        }

        /// <summary>
        /// 温特
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjWTP()
        {
            return View();
        }

        public ActionResult CC()
        {
            return View();
        }
    }
}