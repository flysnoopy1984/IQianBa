using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Simple;
using IQBCore.IQBWX.Models.WX.Template.InviteCode;
using IQBCore.IQBWX.Models.WX.Template.NewMemberReview;
using IQBWX.DataBase.IQBPay;
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

        public ActionResult TestQR()
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

        public ActionResult RateChangeNT()
        {
            string accessToken = this.getAccessToken(true);
            using (AliPayContent db = new AliPayContent())
            {
                string openId = "o3nwE0jrONff65oS-_W96ErKcaa0";
                string sql = string.Format(@"select MarketRate-Rate as OrigFeeRate,QRType from QRUser
where OpenId = '{0}'",openId);

                List<SFee> list = db.Database.SqlQuery<SFee>(sql).ToList();
                foreach(var obj in list)
                {
                    obj.AdjustedFeeRate = Convert.ToSingle((obj.OrigFeeRate - 0.01).ToString("0.00"));
                }

                PPInviteCodeNT notice = new PPInviteCodeNT(accessToken, list, openId);
                notice.Push();
            }
               

            return View();
        }
    }
}