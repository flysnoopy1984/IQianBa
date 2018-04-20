using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
using IQBCore.Model;
using IQBWX.DataBase.IQBPay;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;

namespace IQBWX.Controllers
{
    public class O2OWapController : WXBaseController
    {
        // GET: O2O
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// O2O代理入口
        /// </summary>
        /// <returns></returns>
        public ActionResult EntryAgent()
        {
            if (UserSession.O2OUserRole< O2OUserRole.Agent && UserSession.UserRole!= UserRole.Administrator)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            if(string.IsNullOrEmpty(UserSession.AgentPhone))
            {
                return RedirectToAction("AgentInfoCheck");
            }
            EQRUser qrUser = null;
            using (AliPayContent db = new AliPayContent())
            {
                string PaySite = ConfigurationManager.AppSettings["Site_IQBPay"];
                qrUser = db.DBQRUser.Where(a => a.OpenId == UserSession.OpenId && a.QRType == QRType.O2O).FirstOrDefault();

                if (qrUser == null)
                    qrUser = new EQRUser();
                else
                {
                    qrUser.FilePath = PaySite + qrUser.FilePath;
                    qrUser.OrigQRFilePath = PaySite + qrUser.OrigQRFilePath;
                }
                   
            }
          //  ViewBag.TargetUrl = qrUser.TargetUrl;

            string ppUrl = ConfigurationManager.AppSettings["Site_IQBPay"];
            ViewBag.ppUrl = ppUrl;
            //ViewBag.ConfirmJquery = ppUrl+ "scripts/Component/jquery-confirm.js";
            //ViewBag.ConfirmCss = ppUrl+ "Content/Component/jquery-confirm.min.css";
            //ViewBag.ppUrl = ppUrl;

            ViewBag.IsAdmin = UserSession.UserRole == UserRole.Administrator;
  //          ViewBag.ReChargePage = ConfigurationManager.AppSettings["Site_IQBPay"] + "O2OWap/WHReCharge";
            ViewBag.AgentPhone = UserSession.AgentPhone;

            // InitProfilePage();
            return View(qrUser);
        }

        public ActionResult AgentInfoCheck()
        {
            if (UserSession.O2OUserRole < O2OUserRole.Agent && UserSession.UserRole != UserRole.Administrator)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
            if (!string.IsNullOrEmpty(UserSession.AgentPhone))
            {
                string url = ConfigurationManager.AppSettings["Site_WX"] +"/O2OWap/AgentEntry";
                return RedirectToAction("ErrorMessage", "Home", new { code = 2000, ErrorMsg="已校验用户！" , backUrl = url });
            }


            return View();
        }

        public ActionResult AgentFeeRate()
        {
            if (UserSession.O2OUserRole < O2OUserRole.Agent && UserSession.UserRole != UserRole.Administrator)
            {
                return RedirectToAction("ErrorMessage", "Home", new { code = 2002 });
            }
        
            return View();
        }

      

            //        [HttpPost]
            //        public ActionResult AgentFeeRateQuery()
            //        {
            //            NResult<RO2OAgentFeeRate> result = new NResult<RO2OAgentFeeRate>();
            //            try
            //            {
            //                using (AliPayContent db = new AliPayContent())
            //                {
            //                    string sql = @"select r.Id,r.MarketRate,i.ShipFee+{1} as FeeRate,m.Name as MallName
            //from O2OAgentFeeRate as r
            //join
            //(

            //select max(i.ShipFeeRate) as ShipFee,i.MallId
            //from O2OItemInfo as i
            //where i.RecordStatus = 0
            //group by i.MallId

            //) as i on i.MallId = r.MallId
            //join O2OMall as m on m.ID = r.MallId
            //where r.OpenId = '{0}'";

            //                    sql = string.Format(sql, UserSession.OpenId,GlobalConfig.AgentFeeBasedShipFee);
            //                    result.resultList = db.Database.SqlQuery<RO2OAgentFeeRate>(sql).ToList();
            //                    if (result.resultList == null) result.resultList = new List<RO2OAgentFeeRate>();
            //                }
            //            } 
            //            catch(Exception ex)
            //            {
            //                result.IsSuccess = false;
            //                result.ErrorMsg = ex.Message;
            //            }

            //            return Json(result);
            //        }

            [HttpPost]
        public ActionResult AgentFeeRateQuery()
        {
            NResult<RO2OAgentFeeRate> result = new NResult<RO2OAgentFeeRate>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select ISNULL(agent.Id,0) as Id,i.Id as ItemId,i.MallCode,i.Name as ItemName,i.Amount,i.ShipFeeRate+1 as FeeRate,i.PayMethod,i.IsLightReceive,
m.Name as MallName,isnull(agent.MarketRate,0) as MarketRate
from O2OItemInfo as i
left join O2OAgentFeeRate as agent on agent.ItemId = i.Id and agent.OpenId = '{0}'
join O2OMall as m on m.Code = i.MallCode
where i.RecordStatus =0
order by i.PayMethod,i.Amount
";
                    sql = string.Format(sql, UserSession.OpenId, GlobalConfig.AgentFeeBasedShipFee);
                    result.resultList = db.Database.SqlQuery<RO2OAgentFeeRate>(sql).ToList();
                    if (result.resultList == null) result.resultList = new List<RO2OAgentFeeRate>();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }

        public ActionResult AgentFeeRateSave(List<EO2OAgentFeeRate> newList, List<EO2OAgentFeeRate> updateList)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                if(string.IsNullOrEmpty(UserSession.OpenId))
                {
                    result.IntMsg = -1;
                    result.IsSuccess = false;
                    result.ErrorMsg = "没有获取代理信息，请在微信公众号登录";
                    return Json(result);
                }
                using (AliPayContent db = new AliPayContent())
                {
                    
                    if(newList!=null)
                    {
                        foreach (EO2OAgentFeeRate obj in newList)
                        {
                            obj.OpenId = UserSession.OpenId;
                            obj.DiffFeeRate = 0;
                            EO2OAgentFeeRate upObj = db.DBO2OAgentFeeRate.Where(a => a.OpenId == obj.OpenId && a.ItemId == obj.ItemId).FirstOrDefault();
                            if(upObj == null)
                                db.DBO2OAgentFeeRate.Add(obj);
                          

                        }
                        db.SaveChanges();
                    }
                  
                    if(updateList!=null)
                    {
                        foreach (EO2OAgentFeeRate obj in updateList)
                        {

                            db.DBO2OAgentFeeRate.Where(a => a.Id == obj.Id).Update(a => new EO2OAgentFeeRate
                            {
                                MarketRate = obj.MarketRate
                            });

                        }
                    }

                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }

       

    }
}