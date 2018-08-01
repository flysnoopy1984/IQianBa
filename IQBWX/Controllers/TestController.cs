using IQBCore.IQBWX.Models.WX.Template.NewMemberReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBWX.Controllers
{
    public class TestController : WXBaseController
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestSMS()
        {
            return View();
        }

        public ActionResult NewMemberNT()
        {
            string accessToken = this.getAccessToken(true);

            PPNewMemberReviewNT obj = new PPNewMemberReviewNT(accessToken, "o3nwE0qI_cOkirmh_qbGGG-5G6B0", "o3nwE0jrONff65oS-_W96ErKcaa0", "NewTest",DateTime.Now.ToString());
            obj.Push();

            return View();
        }
    }
}