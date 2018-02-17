using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.Json;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.User;
using IQBPay.Core;
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
            var MallId =  Convert.ToInt32(Request["MallId"]);

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
                if(MallId>0)
                {
                    list = list.Where(o => o.MallId == MallId);
                }
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
        [IQBPayAuthorize_Admin]
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
                    FeeRate = o.FeeRate,
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

        [IQBPayAuthorize_Admin]
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
        [IQBPayAuthorize_Admin]
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

        #region Address
        public ActionResult AddrList()
        {
            return View();
        }

        public ActionResult SaveAddr(EO2ODeliveryAddr obj)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    if (obj.Id > 0)
                    {
                        EO2ODeliveryAddr updateObj = db.DBO2ODeliveryAddr.Where(o => o.Id == obj.Id).FirstOrDefault();
                        updateObj.InitFromUpdate(obj);
                        db.Update<EO2ODeliveryAddr>(updateObj);
                    }
                    else
                    {
                        db.Insert<EO2ODeliveryAddr>(obj);
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

        public ActionResult AddrListQuery()
        {
            var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);
            List<RO2ODeliveryAddr> result = new List<RO2ODeliveryAddr>();

            using (AliPayContent db = new AliPayContent())
            {
                var list = db.DBO2ODeliveryAddr.Select(o => new RO2ODeliveryAddr
                {
                    Id = o.Id,
                    Name = o.Name,
                    Code = o.Code,
                    Address = o.Address,
                    City = o.City,
                    RecordStatus = o.RecordStatus,
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
        #endregion

        #region O2OUser/Lgoin
        public ActionResult Login()
        {
            string UserPhone = CookieHelper.getCookie(IQBConstant.ck_O2OUserPhone);
            if(!string.IsNullOrEmpty(UserPhone))
                ViewBag.O2OUserPhone = UserPhone;
            
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin()
        {

            string UserPhone = Request["UserPhone"];
            string Pwd = Request["Pwd"];
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EUserInfo ui =  db.DBUserInfo.Where(u => u.UserPhone == UserPhone && u.Pwd == Pwd).FirstOrDefault();
                    if(ui == null)
                    {
                        return base.ErrorResult("用户名或密码不正确");
                    }
                    if(ui.O2OUserRole != O2OUserRole.Shippment && ui.UserRole != UserRole.Administrator)
                    {
                        return base.ErrorResult("权限不足");
                    }
                    this.SetUserSession(ui);
                    CookieHelper.setCookie(IQBConstant.ck_O2OUserPhone, ui.UserPhone);
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

        [IQBPayAuthorize_Admin]
        public ActionResult OrderList()
        {

            return View();
        }

        [HttpPost]
        public ActionResult OrderListQuery(InO2OOrder InO2OOrder)
        {

            List<RO2OOrder> result = new List<RO2OOrder>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select o.*,m.Name as MallName from O2OOrder as o 
left join O2OMall as m on m.ID = o.MallId
where o.CreateDateTime between cast('{0}' as datetime) and cast('{1}' as datetime) ";

                    string toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                    string FromDate = "";

                    if (InO2OOrder.BeforeDay == 99)
                    {
                        FromDate = DateTime.Parse("2000-01-01").ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        int d = -Convert.ToInt32(InO2OOrder.BeforeDay);

                        FromDate = DateTime.Now.AddDays(d).ToString("yyyy-MM-dd");
                    }

                    sql = string.Format(sql, FromDate, toDate);

                    if (!string.IsNullOrEmpty(InO2OOrder.MallOrderNo))
                    {
                        sql += string.Format(" and o.MallOrderNo like '%{0}%'", InO2OOrder.MallOrderNo);
                    }
                    if (InO2OOrder.O2OOrderStatus != IQBCore.IQBPay.BaseEnum.O2OOrderStatus.All)
                    {
                        if (InO2OOrder.O2OOrderStatus == O2OOrderStatus.Settle_Payment)
                        {
                            sql += string.Format(" and (o.O2OOrderStatus ={0} or o.O2OOrderStatus={1})",
                                Convert.ToInt32(O2OOrderStatus.Settlement),
                                Convert.ToInt32(O2OOrderStatus.Payment));
                        }
                        else
                            sql += string.Format(" and o.O2OOrderStatus ={0}", Convert.ToInt32(InO2OOrder.O2OOrderStatus));
                    }
                    sql += " order by o.CreateDateTime desc";

                    var list = db.Database.SqlQuery<RO2OOrder>(sql);

                    if (InO2OOrder.pageIndex == 0)
                    {
                        result = list.Take(InO2OOrder.pageSize).ToList();
                    }
                    else
                    {
                        result = list.Skip(InO2OOrder.pageIndex * InO2OOrder.pageSize).Take(InO2OOrder.pageSize).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult HashOrderStatus()
        {
            List<HashKeyValue> list = new List<HashKeyValue>();
            foreach (O2OOrderStatus item in Enum.GetValues(typeof(O2OOrderStatus)))
            {
                HashKeyValue obj = new HashKeyValue();
                obj.Key = Convert.ToInt32(item).ToString();
                obj.Value = IQBPayEnum.GetO2OName(item);
                list.Add(obj);
            }
            return Json(list);
        }

        [IQBPayAuthorize_Admin]
        public ActionResult OrderReview()
        {
            string O2ONo = Request.QueryString["O2ONo"];
            RO2OOrder obj = null;
            if (string.IsNullOrEmpty(O2ONo))
            {

            }
            else
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select o.*,a.Address from O2OOrder as o
left join O2ODeliveryAddress as a on o.AddrId = a.Id
where o.O2ONo = '{0}'";
                    sql = string.Format(sql, O2ONo);
                    obj = db.Database.SqlQuery<RO2OOrder>(sql).FirstOrDefault();
                }
            }
            if (obj == null)
                obj = new RO2OOrder();
            return View(obj);
        }

        [HttpPost]
        public ActionResult OrderReviewResult(InOrderReview InOrderReview)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2OOrder order = db.DBO2OOrder.Where(o => o.O2ONo == InOrderReview.O2ONo).FirstOrDefault();
                    if (InOrderReview.IsApprove)
                    {
                        order.MallOrderNo = InOrderReview.MallOrderNo;
                        order.OrderAmount = InOrderReview.OrderAmount;
                        order.O2OOrderStatus = O2OOrderStatus.Settlement;
                    }
                    else
                    {
                        order.RejectReason = InOrderReview.RejectReason;
                        order.O2OOrderStatus = O2OOrderStatus.OrderRefused;
                    }
                    db.Update<EO2OOrder>(order);

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);


        }

        public ActionResult OrderListForSettlement()
        {
            return View();
        }

        public ActionResult OrderSettlement()
        {
            string O2ONo = Request.QueryString["O2ONo"];
            EO2OOrder obj = null;
            if (string.IsNullOrEmpty(O2ONo))
            {

            }
            else
            {
                using (AliPayContent db = new AliPayContent())
                {
                    obj = db.DBO2OOrder.Where(o => o.O2ONo == O2ONo).FirstOrDefault();
                }
            }
            if (obj == null)
                obj = new EO2OOrder();
            return View(obj);
        }

        [HttpPost]
        public ActionResult CreateDemoData()
        {
            OutAPIResult result = new OutAPIResult();
            using (AliPayContent db = new AliPayContent())
            {
                for (int i = 0; i < 10; i++)
                {
                    EO2OOrder order = new EO2OOrder()
                    {
                        O2ONo = StringHelper.GenerateO2ONo(),
                        CreateDateTime = DateTime.Now,
                        MallAccount = "yujie@hotmail.com",
                        MallId = 3,
                        MallOrderNo = "HWxxxxxxx",
                        MallPwd = "123456",
                        MallSMSVerify = "",
                        OrderImgUrl = "",
                        O2OOrderStatus = IQBCore.IQBPay.BaseEnum.O2OOrderStatus.OpenOrder,
                        RefOrderNo = 0,
                        UserAliPayAccount = "MyAliPay@hotmail.com",
                        UserPhone = "18221882506",
                    };
                    if (i < 3) order.O2OOrderStatus = IQBCore.IQBPay.BaseEnum.O2OOrderStatus.OpenOrder;
                    //if(i ==3) order.O2OOrderStatus = IQBCore.IQBPay.BaseEnum.O2OOrderStatus.WaitingDeliver;
                    if (i == 4) order.O2OOrderStatus = IQBCore.IQBPay.BaseEnum.O2OOrderStatus.WaitingUpload;
                    if (i == 5) order.O2OOrderStatus = IQBCore.IQBPay.BaseEnum.O2OOrderStatus.Settlement;
                    if (i == 6) order.O2OOrderStatus = IQBCore.IQBPay.BaseEnum.O2OOrderStatus.Payment;
                    if (i == 7) order.O2OOrderStatus = IQBCore.IQBPay.BaseEnum.O2OOrderStatus.OrderReview;
                    if (i == 8) order.O2OOrderStatus = IQBCore.IQBPay.BaseEnum.O2OOrderStatus.OrderRefused;
                    if (i == 9) order.O2OOrderStatus = IQBCore.IQBPay.BaseEnum.O2OOrderStatus.Complete;
                    db.Insert<EO2OOrder>(order);
                }
            }
            return Json(result);
        }


        #endregion


        #region Transaction 财务资金
        public ActionResult TransWH()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TransWHQuery(InO2OTrans InO2OTrans)
        {
            List<RO2OTranscationWH> result = new List<RO2OTranscationWH>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select o.*,m.Name as MallName from O2OTranscationWH as o 
                    left join O2OMall as m on m.ID = o.MallId
                    where o.TransDateTime between cast('{0}' as datetime) and cast('{1}' as datetime) ";

                    string toDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                    string FromDate = "";

                    if (InO2OTrans.BeforeDay == 99)
                    {
                        FromDate = DateTime.Parse("2000-01-01").ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        int d = -Convert.ToInt32(InO2OTrans.BeforeDay);

                        FromDate = DateTime.Now.AddDays(d).ToString("yyyy-MM-dd");
                    }

                    sql = string.Format(sql, FromDate, toDate);

                    if (!string.IsNullOrEmpty(InO2OTrans.MallOrderNo))
                    {
                        sql += string.Format(" and o.MallOrderNo like '%{0}%'", InO2OTrans.MallOrderNo);
                    }
                    sql += " order by o.TransDateTime desc";

                    var list = db.Database.SqlQuery<RO2OTranscationWH>(sql);

                    if (InO2OTrans.pageIndex == 0)
                    {
                        result = list.Take(InO2OTrans.pageSize).ToList();
                    }
                    else
                    {
                        result = list.Skip(InO2OTrans.pageIndex * InO2OTrans.pageSize).Take(InO2OTrans.pageSize).ToList();
                    }
                }
               
            }
            
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result);
         }

        [HttpPost]
        public ActionResult CreateTransData()
        {
            OutAPIResult result = new OutAPIResult();
            using (AliPayContent db = new AliPayContent())
            {
                for (int i = 0; i < 10; i++)
                {
                    EO2OTranscationWH trans = new EO2OTranscationWH()
                    {
                        FeeRate = 3,
                        MallId = 3,
                        MallOrderNo = "dddddd",
                        ReceiveAccount = "aaaa@12",
                        TransDateTime=DateTime.Now,
                        TransferAmount= 1000000,
                    
                    
                       
                    };
                  
                    db.Insert<EO2OTranscationWH>(trans);
                }
            }
            return Json(result);
        }

        #endregion
    }
}