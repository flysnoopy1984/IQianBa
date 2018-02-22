using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBWX.BaseEnum;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class O2OWapController : BaseController
    {
        // GET: O2OWap
        public ActionResult Index()
        {
            string reqQrO2OId = Request.QueryString["qrUserId"];

            //string wxSite = ConfigurationManager.AppSettings["IQBWX_SiteUrl"];
            //string ErrorUrl = wxSite + "Home/ErrorMessage?code={0}&ErrorMsg=";

            //if (BaseController.GlobalConfig.WebStatus == PayWebStatus.Stop)
            //{
            //    ErrorUrl = string.Format(ErrorUrl,9999);
            //    return Redirect(ErrorUrl);
            //}
            //if (BaseController.GlobalConfig.O2OEntry == PayWebStatus.Stop)
            //{
            //    ErrorUrl += "O2O通道维护中，请稍后进入！";
            //    return Redirect(ErrorUrl);

            //}
            using (AliPayContent db = new AliPayContent())
            {
               
            }


                return View();
        }

        public ActionResult Demo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QueryItemList()
        {
            int MallId =Convert.ToInt32(Request["MallId"]);
            int PGId = Convert.ToInt32(Request["PGId"]);

            List<RO2OItemInfo> result = new List<RO2OItemInfo>();
            using (AliPayContent db = new AliPayContent())
            {
                var list = db.DBO2OItemInfo.Select(o => new RO2OItemInfo
                {
                    Id = o.Id,
                    Name = o.Name,
                    Amount = o.Amount,
                    MallId = o.MallId,
                    ImgUrl = o.ImgUrl,
                    Qty = o.Qty,
                    O2ORuleId = o.O2ORuleId,
                    RealAddress = o.RealAddress,
                    RecordStatus=o.RecordStatus,

                }).Where(o=>o.RecordStatus == RecordStatus.Normal);
                if (PGId > 0)
                    list = list.Where(o=>o.PriceGroupId == PGId);
                if(MallId>0)
                    list = list.Where(o => o.MallId == MallId);
                result = list.ToList();


            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult QueryPriceGrouplist()
        {
            List<EO2OPriceGroup> result = new List<EO2OPriceGroup>();
            string reqMallId = Request["MallId"];
            int MallId = 0;
            if(!string.IsNullOrEmpty(reqMallId))
                MallId = Convert.ToInt32(reqMallId);
            using (AliPayContent db = new AliPayContent())
            {
                var list = db.DBO2OPriceGroup;
                if (MallId > 0)
                {
                    string sql = @"select * from O2OPriceGroup
                            where Id in 
                            (
                                select PriceGroupId from O2OItemInfo where MallId = '{0}'
                            )";
                    sql = string.Format(sql, MallId);
                    result = db.Database.SqlQuery<EO2OPriceGroup>(sql).ToList();
                }
                else
                    result = db.DBO2OPriceGroup.ToList();
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult QueryMallList()
        {
            List<RO2OMall> result = new List<RO2OMall>();
            using (AliPayContent db = new AliPayContent())
            {
                string sql = @"select * from O2OMall
                            where Id in 
                            (
                              select MallId from O2OItemInfo where O2OItemInfo.RecordStatus = 0
                            ) and RecordStatus = 0
                            ";
                result = db.Database.SqlQuery<RO2OMall>(sql).ToList();
            }
            return Json(result);
        }

        #region O2OOrder


        public ActionResult CheckBuyerPhone()
        {
            string rPhone = Request["Phone"];
            OutAPIResult result = new OutAPIResult();
            int n = 0;
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    n = db.DBO2OBuyer.Where(o => o.Phone == rPhone).Count();
                    result.IntMsg = n;
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
       
            return Json(result);
        }

        public void InitO2OOrder()
        {
            string rPhone = Request["Phone"];
            string rReceiveAccount = Request["ReceiveAccount"];
          //  string rItemId = 

        }
        #endregion

        #region Data
        public EO2OOrder GetO2OOrder()
        {
            EO2OOrder order = new EO2OOrder();
            return order;
        }
        #endregion


    }
}