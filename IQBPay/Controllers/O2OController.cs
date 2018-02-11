using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.OutParameter;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class O2OController : BaseController
    {
        // GET: O2O
        public ActionResult Index()
        {
            return View();
        }

        #region Item

        public ActionResult ItemList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ItemListQuery()
        {
            var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);
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
                    RealAddress = o.RealAddress,
                    O2ORuleId = o.O2ORuleId,
                    RecordStatus = o.RecordStatus,
                    CreateDateTime = o.CreateDateTime,
                    ModifyDateTime = o.ModifyDateTime,
                  
                });
                list = list.OrderBy(o => o.Id);
                if (pageIndex == 0)
                {
                    result = list.Take(pageSize).ToList();
                }
                else
                {
                    result = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }

            return Json(result);

           
        }


        public ActionResult SaveItem(EO2OItemInfo obj)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = string.Format("select * from O2OPriceGroup where {0}>=FromPrice and {0}<=ToPrice",obj.Amount);
                    EO2OPriceGroup pg = db.Database.SqlQuery<EO2OPriceGroup>(sql).FirstOrDefault();
                    if(pg == null)
                    {
                       return ErrorResult("没有找到对应的金额价格组,请联系管理员");
                    }
                    obj.PriceGroupId = pg.Id;
                    if (obj.Id > 0)
                    {
                        EO2OItemInfo updateObj = db.DBO2OItemInfo.Where(o => o.Id == obj.Id).FirstOrDefault();
                        updateObj.InitFromUpdate(obj);
                        db.Update<EO2OItemInfo>(updateObj);
                    }
                    else
                    {
                        db.Insert<EO2OItemInfo>(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult ItemDelete()
        {
            int Id = Convert.ToInt32(Request["ItemId"]);
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2OItemInfo item = db.DBO2OItemInfo.Where(o => o.Id == Id).FirstOrDefault();
                   
                    db.Delete<EO2OItemInfo>(item);
                 
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult ItemStatusChange()
        {
            int Id = Convert.ToInt32(Request["ItemId"]);
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2OItemInfo item = db.DBO2OItemInfo.Where(o => o.Id == Id).FirstOrDefault();
                    if (item.RecordStatus == IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked)
                        item.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Normal;
                    else
                        item.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked;
                    db.Update<EO2OItemInfo>(item);
                    result.SuccessMsg = Convert.ToInt32(item.RecordStatus).ToString();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }
        #endregion

        #region Mall
        public ActionResult MallList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InitRule_Mall()
        {

            HashO2OMall_Rule result = new HashO2OMall_Rule();
            using (AliPayContent db = new AliPayContent())
            {
                var rule = db.Database.SqlQuery<HashO2ORule>("select Id,Name from O2ORule").ToList();
                if (rule != null)
                    result.HashO2ORule = rule;
                var mall = db.Database.SqlQuery<HashO2OMall>("select Id,Name,O2ORuleId from O2OMall").ToList();
                if (mall != null)
                    result.HashO2OMall = mall;

            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult MallListQuery()
        {
            var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);
            List<RO2OMall> result = new List<RO2OMall>();
            using (AliPayContent db = new AliPayContent())
            {
                var list = db.DBO2OMall.Select(o => new RO2OMall
                {
                    Id = o.Id,
                    Name = o.Name,
                    Code = o.Code,
                    RecordStatus = o.RecordStatus,
                    Description = o.Description,
                    O2ORuleId = o.O2ORuleId,
                });
                list = list.OrderByDescending(o => o.Id);
                if (pageIndex == 0)
                {
                    result = list.Take(pageSize).ToList();
                }
                else
                {
                    result = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }

            return Json(result);
        }

       
        public ActionResult SaveMall(EO2OMall obj)
        {

            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    if (obj.Id > 0)
                    {
                        EO2OMall updateObj = db.DBO2OMall.Where(o => o.Id == obj.Id).FirstOrDefault();
                        updateObj.InitFromUpdate(obj);
                        db.Update<EO2OMall>(updateObj);
                    }
                    else
                    {
                        db.Insert<EO2OMall>(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult DeleteMall()
        {
            int Id = Convert.ToInt32(Request["Id"]);
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2OMall item = db.DBO2OMall.Where(o => o.Id == Id).FirstOrDefault();

                    db.Delete<EO2OMall>(item);

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }
        #endregion

        #region Rule

        public ActionResult RuleList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RuleHash()
        {
            List<HashO2ORule> result = new List<HashO2ORule>();
            using (AliPayContent db = new AliPayContent())
            {
                result = db.Database.SqlQuery<HashO2ORule>("select Id,Name from O2ORule").ToList();

            }
            return Json(result);
        }
        [HttpPost]
        public ActionResult RuleListQuery()
        {
            var pageIndex =Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);
            List<RO2ORule> result = new List<RO2ORule>();
            using (AliPayContent db = new AliPayContent())
            {
                var list = db.DBO2ORule.Select(o => new RO2ORule
                {
                    Id = o.Id,
                    IsGeneralPayQR = o.IsGeneralPayQR,
                    IsMoneyImmediate = o.IsMoneyImmediate,
                    NeedMallAccount = o.NeedMallAccount,
                    NeedMallSMSVerify = o.NeedMallSMSVerify,
                    Name = o.Name,
                    Code = o.Code,
                });
                if (pageIndex == 0)
                {
                    result = list.Take(pageSize).ToList();
                }
                else
                {
                    result = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }

            return Json(result);
        }

       
        public ActionResult SaveRule(EO2ORule obj)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    if(obj.Id>0)
                    {
                        EO2ORule updateObj =  db.DBO2ORule.Where(o => o.Id == obj.Id).FirstOrDefault();
                        updateObj.InitFromUpdate(obj);
                        db.Update<EO2ORule>(updateObj);
                    }
                    else
                    {
                        db.Insert<EO2ORule>(obj);
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

        [HttpPost]
        public ActionResult DeleteRule()
        {
            int Id = Convert.ToInt32(Request["Id"]);
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2ORule item = db.DBO2ORule.Where(o => o.Id == Id).FirstOrDefault();

                    db.Delete<EO2ORule>(item);

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }

        #endregion

        #region PriceGroup
        public ActionResult PriceGroupList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PriceGroupListQuery()
        {
            List<EO2OPriceGroup> result = new List<EO2OPriceGroup>();
            using (AliPayContent db = new AliPayContent())
            {
                result = db.DBO2OPriceGroup.ToList(); 
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult SavePriceGroup(EO2OPriceGroup obj)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    if (obj.Id > 0)
                    {
                        EO2OPriceGroup updateObj = db.DBO2OPriceGroup.Where(o => o.Id == obj.Id).FirstOrDefault();
                        updateObj.InitFromUpdate(obj);
                        db.Update<EO2OPriceGroup>(updateObj);
                    }
                    else
                    {
                        db.Insert<EO2OPriceGroup>(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
           
        }

        [HttpPost]
        public ActionResult DeletePriceGroup()
        {
            int Id = Convert.ToInt32(Request["Id"]);
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2OPriceGroup item = db.DBO2OPriceGroup.Where(o => o.Id == Id).FirstOrDefault();

                    db.Delete<EO2OPriceGroup>(item);

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }

        #endregion

        #region O2OOrder

        public ActionResult Order()
        {
            return View();
        }

        public ActionResult OrderListQuery()
        {

        }
        #endregion
    }
}