using IQBWX.DataBase;
using IQBWX.Models.Results;
using IQBWX.Models.Transcation;
using IQBWX.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebManager.Controllers
{
    public class ReportController : Controller
    {

        public ActionResult UserSummary()
        {
            return View();
        }
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSummery()
        {
            int st = Convert.ToInt32(this.Request["summeryType"]);
            var date = this.Request["searchDate"];

            UserSummery userSummery = new UserSummery();
            using (UserContent db = new UserContent())
            {
                string sqlCount = string.Format(@"select count(1) from MemberInfo where convert(char(10),[RegisterDateTime],120) = '{0}'", date);
                userSummery.DayMemberAdded = db.Database.SqlQuery<int>(sqlCount).FirstOrDefault<int>();
                userSummery.TotalMember = db.MemberInfo.Count();

                sqlCount = string.Format(@"select count(1) from UserInfo where convert(char(10),[SubscribeDateTime],120) = '{0}'", date);
                userSummery.DayUserSub = db.Database.SqlQuery<int>(sqlCount).FirstOrDefault<int>();
                userSummery.TotalUser = db.UserInfo.Count();
            }
            JsonResult jr = Json(userSummery);
            return jr;          
        }

        public ActionResult ST()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DetailList()
        {
            int st = Convert.ToInt32(this.Request["summeryType"]);
            var date = this.Request["searchDate"];
            List<EMemberInfo>  mlist = null;
            List<EUserInfo> ulist = null;
            List<EARUserTrans> arList = null;
            string sql;
            UserContent udb = new UserContent();
            TransContent tdb = new TransContent();
          
               switch(st)
                {
                    case 1:
                    if (string.IsNullOrEmpty(date))
                        sql = @"select * from UserInfo ui";
                    else
                        sql = string.Format(@"select * from UserInfo ui where convert(char(10),ui.SubscribeDateTime,120) = '{0}' ", date);
                    ulist = udb.UserInfo.SqlQuery(sql).ToList();
                    return Json(ulist);
                    case 2:
                    if (string.IsNullOrEmpty(date))
                        sql = @"select * from MemberInfo";
                    else
                        sql = string.Format(@"select * from MemberInfo mi where convert(char(10),mi.RegisterDateTime,120) = '{0}' ", date);
                    mlist = udb.MemberInfo.SqlQuery(sql).ToList();
                    return Json(mlist);
                    case 3:
                    if (string.IsNullOrEmpty(date))
                        sql = @"select * from ARUserTrans";
                    else
                        sql = string.Format(@"select * from ARUserTrans trans where convert(char(10),trans.TransDateTime,120) = '{0}' ", date);
                    arList = tdb.ARTransDbSet.SqlQuery(sql).ToList();
                    return Json(arList);                       
                }
            

          
            return null;
        }
    }
}