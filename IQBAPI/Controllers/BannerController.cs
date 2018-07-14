using IQBCore.Common.Helper;
using IQBCore.DataBase;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Model;
using IQBCore.OO.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBAPI.Controllers
{
    public class BannerController :BaseAPIController
    {
      //  private BannerDBSrv _SrvBanner = new BannerDBSrv();  


        [HttpPost]
        public NResult<EBanner> GetLatest()
        {
            NResult<EBanner> result = base.GetLatestData<EBanner>(10);
           // NLogHelper.InfoTxt("ttt");


            return result;
        }

        [HttpPost]
        public OutAPIResult InsertBanner()
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (OOContent db = new OOContent())
                {
                    EBanner obj = new EBanner();
                    obj.BannerImg = "/Content/Images/AppBanner/Banner1.png";
                    obj.Title = "最新公告";
                    obj.CreatedTime = DateTime.Now;
                    obj.Position = 1;
                    db.DBBanner.Add(obj);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return result;

        }
    }
}
