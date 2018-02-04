using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.Report;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OverView()
        {
            return View();
        }

        public ActionResult AgentOverView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgentOverViewQuery(InAgentOverview InParamter)
        {
            //string AgentName = Request["AgentName"];
            //string ParentName = Request["ParentName"];
            //string BeforeDay  = Request["BeforeDay"];
            //int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            //int pageSize = Convert.ToInt32(Request["pageSize"]);

            List<RPAgentPerformance> result = new List<RPAgentPerformance>();
 
            using (AliPayContent db = new AliPayContent())
            {
                string sql = @"select ui.Id, ui.OpenId as OpneId,CONVERT(varchar(100), ui.RegisterDate, 111) as RegisterDate, ui.Name as AgentName,pui.OpenId as ParentOpenId,pui.Name as ParentName,o.OrderTotalAmount,o.OrderComplatedNum from UserInfo ui
                left join 
                (
                select o.AgentOpenId,sum(TotalAmount) as OrderTotalAmount,count(*) as OrderComplatedNum  from OrderInfo as o
                where o.OrderStatus =2 and o.TransDate between cast('{0}' as datetime) and cast('{1}' as datetime)
	                group by AgentOpenId
	
                ) 
                as o on o.AgentOpenId = ui.OpenId
                left join UserInfo as pui on pui.OpenId = ui.parentOpenId
                where ui.UserStatus = 1";

                string toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                string FromDate = "";
                if(InParamter.BeforeDay == "99")
                {
                    FromDate = DateTime.Parse("2000-01-01").ToString("yyyy-MM-dd");
                }
                else
                {
                    int d = -Convert.ToInt32(InParamter.BeforeDay);

                    FromDate = DateTime.Now.AddDays(d).ToString("yyyy-MM-dd");
                }

                sql = string.Format(sql, FromDate, toDate);

                if (!string.IsNullOrEmpty(InParamter.AgentName))
                {
                    sql += string.Format(" and ui.Name like '%{0}%'", InParamter.AgentName);
                }
                if (!string.IsNullOrEmpty(InParamter.ParentName))
                {
                    sql += string.Format(" and pui.Name like '%{0}%'", InParamter.ParentName);
                }
                sql += " order by o.OrderTotalAmount desc,RegisterDate desc ";

                var list = db.Database.SqlQuery<RPAgentPerformance>(sql);

                if (InParamter.pageIndex == 0)
                {
                    result = list.Take(InParamter.pageSize).ToList();
                }
                else
                {
                    result = list.Skip(InParamter.pageIndex * InParamter.pageSize).Take(InParamter.pageSize).ToList();
                }

            }
            return Json(result);
        }
    }
}