using IQBPay.Core;
using IQBCore.IQBPay.BaseEnum;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQBCore.Common.Constant;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BLL;
using Com.Alipay.Model;

namespace IQBPay.Controllers
{
    public class StoreController : BaseController
    {
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        [IQBPayAuthorize_Admin]
        public ActionResult StoreTransSum()
        {
            return View();
        }

        [IQBPayAuthorize_Admin]
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Get(int Id)
        {

            EStoreInfo result = null;

            if (Id == -1)
            {
                result.RunResult = "没有获取Id";
                return Json(result);
            }
            else
            {
                using (AliPayContent db = new AliPayContent())
                {
                 
                    result = db.DBStoreInfo.Where(a => a.ID == Id).FirstOrDefault();
                    result.RunResult = "OK";
                }
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult QueryKeyValue()
        {
            List<HashStore> result =null;

            using (AliPayContent db = new AliPayContent())
            {
                result = db.Database.SqlQuery<HashStore>("select Id,Name from storeinfo").ToList();
                if(result == null)
                    result = new List<HashStore>();
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult QuerySum(InStoreSum parameter)
        {
            string sql = @"select sum(o.TotalAmount) as TotalAmount,o.SellerStoreId,s.Name as StoreName,s.Rate,s.DayIncome from orderinfo as o
                                inner join storeinfo as s on s.ID = o.SellerStoreId
                                where 1=1";

          

            List<RStore_OrderSum> result = new List<RStore_OrderSum>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    if(!string.IsNullOrEmpty(parameter.StoreId) && parameter.StoreId != "-1")
                    {
                        sql += " and o.SellerStoreId=" + parameter.StoreId;
                    }
                    if(parameter.DataType == ConditionDataType.Today)
                    {
                        sql += " and Date(o.TransDate) = '" + DateTime.Today.ToString("yyyy-MM-dd")+"'";
                    }
                    if (parameter.DataType == ConditionDataType.Week)
                    {
                        DateTime startDate = UtilityHelper.GetTimeStartByType("Week", DateTime.Now);
                        DateTime endDate = UtilityHelper.GetTimeEndByType("Week", DateTime.Now);

                        sql += " and o.TransDate between  '"+startDate.ToString("yyyy-MM-dd")+ "' and  '" + endDate.ToString("yyyy-MM-dd") + "'";
                    }
                    if (parameter.DataType == ConditionDataType.Month)
                    {

                        DateTime startDate = UtilityHelper.GetTimeStartByType("Month", DateTime.Now);
                        DateTime endDate = UtilityHelper.GetTimeEndByType("Month", DateTime.Now);

                        sql += " and o.TransDate between  '" + startDate.ToString("yyyy-MM-dd") + "' and  '" + endDate.ToString("yyyy-MM-dd") + "'";
                    }


                    var list = db.Database.SqlQuery<RStore_OrderSum>(sql).ToList();

                    int totalCount = list.Count();
                    if (parameter.PageIndex == 0)
                    {
                        result = list.Take(parameter.PageSize).ToList();

                        if (result.Count > 0)
                            result[0].TotalCount = totalCount;
                    }
                    else
                        result = list.Skip(parameter.PageIndex * parameter.PageSize).Take(parameter.PageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult Query(Channel Channel,int pageIndex = 0, int pageSize = IQBConstant.PageSize)
        {
            List<EStoreInfo> result = new List<EStoreInfo>();
            IQueryable<EStoreInfo> list = null ;
            try
            {
                string openId = this.GetUserSession().OpenId;

                using (AliPayContent db = new AliPayContent())
                {
                    
                    if (Channel == Channel.All)
                        list = db.DBStoreInfo.Where(i => i.OwnnerOpenId == openId).OrderByDescending(i => i.CreateDate);
                    else
                        list = db.DBStoreInfo.Where(i => i.OwnnerOpenId == openId && i.Channel == Channel).OrderByDescending(i => i.CreateDate);

                    int totalCount = list.Count();
                    if (pageIndex == 0)
                    {
                        result = list.Take(pageSize).ToList();

                        if (result.Count > 0)
                            result[0].TotalCount = totalCount;
                    }
                    else
                        result = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                }
            }
            catch(Exception ex)
            {
                Log.log("Store Query Error:" + ex.Message);
                throw ex;
            }
            return Json(result);
        }

        public ActionResult Save(EStoreInfo store)
        {
            try
            {
               
                using (AliPayContent db = new AliPayContent())
                {
                   

                    store.InitModify();

                    DbEntityEntry<EStoreInfo> entry = db.Entry<EStoreInfo>(store);
                    entry.State = EntityState.Unchanged;

                   
                    entry.Property(t => t.Name).IsModified = true;
                    //entry.Property(t => t.OpenTime).IsModified = true;
                    //entry.Property(t => t.CloseTime).IsModified = true;

                    entry.Property(t => t.RecordStatus).IsModified = true;
                    entry.Property(t => t.Rate).IsModified = true;
                    entry.Property(t => t.Remark).IsModified = true;
                    entry.Property(t => t.DayIncome).IsModified = true;
                    entry.Property(t => t.IsReceiveAccount).IsModified = true;
                    entry.Property(t => t.MaxLimitAmount).IsModified = true;
                    entry.Property(t => t.MinLimitAmount).IsModified = true;
                    entry.Property(t => t.RemainAmount).IsModified = true;

                    entry.Property(t => t.MDate).IsModified = true;
                    entry.Property(t => t.MTime).IsModified = true;
                    entry.Property(t => t.ModifyDate).IsModified = true;

                    db.SaveChanges();

                    if (store.IsReceiveAccount)
                    {
                        EStoreInfo curReceiveStore = db.DBStoreInfo.Where(s => s.IsReceiveAccount == true).FirstOrDefault();
                        if (curReceiveStore.ID != store.ID)
                        {
                            if (curReceiveStore != null)
                                curReceiveStore.IsReceiveAccount = false;
                        }
                        db.SaveChanges();


                    }

                    if (store.IsReceiveAccount)
                    {
                        BaseController.CleanSubAccount();
                    }

                
                }
            }
            catch(Exception ex)
            {
               
               return Content("Save Store Error"+ex.Message);
            }
            return Json("OK");
        }

        [IQBPayAuthorize_Admin]
        public ActionResult Info()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckStoreAuth()
        {
            OutAPIResult result = new OutAPIResult();

            using (AliPayContent db = new AliPayContent())
            {
                var list = db.DBStoreInfo.Where(s=>s.Channel == Channel.League).ToList();
                AliPayManager payMag = new AliPayManager();
                AliPayResult status;
                for(int i=0;i<list.Count;i++)
                {
                    string Res = payMag.PayTest(BaseController.App, list[i], out status);
                    if (status == AliPayResult.FAILED)
                    list[i].Remark = "[Error]Code:" + Res;
                }
                db.SaveChanges();           
            }
            return Json(result);
        }

        
    }
}