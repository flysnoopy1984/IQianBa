using EntityFramework.Extensions;
using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.Json;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.User;
using IQBCore.IQBWX.Models.WX.Template.ReviewResult;
using IQBCore.Model;
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

        [O2OShipment()]
        public ActionResult ItemList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ItemListQuery()
        {
            var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);
            var MallCode =  Convert.ToString(Request["MallCode"]);
            RecordStatus ItemStatus = (RecordStatus)Enum.Parse(typeof(RecordStatus),Convert.ToString(Request["ItemStatus"]));

            string UserOpenId = base.GetUserSession().OpenId;

            List<RO2OItemInfo> result = new List<RO2OItemInfo>();
            using (AliPayContent db = new AliPayContent())
            {
                var list = db.DBO2OItemInfo.Select(o => new RO2OItemInfo
                {
                    Id = o.Id,
                    Name = o.Name,
                    Amount = o.Amount,
                    MallCode = o.MallCode,
                    ImgUrl = o.ImgUrl,
                    Qty = o.Qty,
                    ShipFeeRate = o.ShipFeeRate,
                    RealAddress = o.RealAddress,
                    O2ORuleCode = o.O2ORuleCode,
                    RecordStatus = o.RecordStatus,
                    CreateDateTime = o.CreateDateTime,
                    ModifyDateTime = o.ModifyDateTime,
                    OpenId = o.OpenId,
                    IsLightReceive = o.IsLightReceive,
                    PayMethod = o.PayMethod,
                  
                });
                if (base.GetUserSession().UserRole != UserRole.Administrator)
                    list = list.Where(o => o.OpenId == UserOpenId);
                if (!string.IsNullOrEmpty(MallCode))
                {
                    list = list.Where(o => o.MallCode == MallCode);
                }
                if(ItemStatus !=  RecordStatus.All)
                {
                    list = list.Where(o => o.RecordStatus == ItemStatus);
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

                    //如果没有填写默认是2
                    if (obj.ShipFeeRate <= 0)
                        obj.ShipFeeRate = 2;

                    if (obj.Id > 0)
                    {
                        db.DBO2OItemInfo.Where(o => o.Id == obj.Id).Update(s => new EO2OItemInfo
                        {
                            Name = obj.Name,
                            Amount = obj.Amount,
                            ImgUrl = obj.ImgUrl,
                            Qty = obj.Qty,
                            RealAddress = obj.RealAddress,
                            MallCode = obj.MallCode, 
                            O2ORuleCode = obj.O2ORuleCode,
                            RecordStatus = obj.RecordStatus,
                            ShipFeeRate = obj.ShipFeeRate,
                            ModifyDateTime = DateTime.Now,
                            PriceGroupId = obj.PriceGroupId,
                            PayMethod = obj.PayMethod,
                            IsLightReceive = obj.IsLightReceive,
                        });
                     //   db.SaveChanges();
                      //  db.DBO2OItemInfo.Where(o => o.Id == obj.Id
                        //EO2OItemInfo updateObj = 
                        //updateObj.InitFromUpdate(obj);
                        //db.Update<EO2OItemInfo>(updateObj);
                    }
                    else
                    {
                        
                        obj.OpenId =base.GetUserSession().OpenId;
                        if (base.GetUserSession().Id == 0)
                        {
                            result.IsSuccess = false;
                            result.IntMsg = -1;
                            return Json(result);
                        }
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
                var rule = db.Database.SqlQuery<HashO2ORule>("select Id,Name,Code from O2ORule").ToList();
                if (rule != null)
                    result.HashO2ORule = rule;
                var mall = db.Database.SqlQuery<HashO2OMall>("select Id,Name,Code,O2ORuleCode from O2OMall").ToList();
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
                    O2ORuleCode = o.O2ORuleCode,
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
                        db.DBO2OMall.Where(o => o.Id == obj.Id).Update(a => new EO2OMall
                        {
                            Description = obj.Description,
                            O2ORuleCode = obj.O2ORuleCode,
                            FeeRate = obj.FeeRate,
                            RecordStatus = obj.RecordStatus,
                            Name = obj.Name,
                        });
                        //updateObj.InitFromUpdate(obj);
                        //db.Update<EO2OMall>(updateObj);
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
                result = db.Database.SqlQuery<HashO2ORule>("select Id,Name,Code from O2ORule").ToList();

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

        [O2OShipment()]
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
                        obj.OpenId = base.GetUserSession().OpenId;
                        if(base.GetUserSession().Id ==0)
                        {
                            result.IsSuccess = false;
                            result.IntMsg = -1;
                            return Json(result);

                        }
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

        public ActionResult AddrStatusChange()
        {
            int Id = Convert.ToInt32(Request["AddrId"]);
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2ODeliveryAddr obj = db.DBO2ODeliveryAddr.Where(a => a.Id == Id).FirstOrDefault();
                    if(obj == null)
                    {
                        result.IntMsg = -1;
                        result.ErrorMsg = "没有找到地址编号，无法修改";
                        return Json(result);
                    }
                    if (obj.RecordStatus == RecordStatus.Blocked)
                        obj.RecordStatus = RecordStatus.Normal;
                    else obj.RecordStatus = RecordStatus.Blocked;
                    db.Update<EO2ODeliveryAddr>(obj);
                    //result.SuccessMsg = Convert.ToInt32(item.RecordStatus).ToString();
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
            RecordStatus RecordStatus = (RecordStatus)Enum.Parse(typeof(RecordStatus), Convert.ToString(Request["RecordStatus"]));

            List<RO2ODeliveryAddr> result = new List<RO2ODeliveryAddr>();
            string UserOpenId = base.GetUserSession().OpenId;
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
                    OpenId = o.OpenId,
                });
                if(base.GetUserSession().UserRole != UserRole.Administrator)
                    list = list.Where(o=>o.OpenId == UserOpenId);
                if (RecordStatus != RecordStatus.All)
                    list = list.Where(a => a.RecordStatus == RecordStatus);

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
            string action = Request.QueryString["action"];
            if (action != null && action ==  "exit")
            {
                base.ExitSession();
            }
           
            string UserPhone = CookieHelper.getCookie(IQBConstant.ck_O2OUserPhone);
            if (!string.IsNullOrEmpty(UserPhone))
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
            UserSession userSession = GetUserSession();
            NResult<RO2OOrder> result = new NResult<RO2OOrder>();
            if(userSession.Id == 0)
            {
                result.IsSuccess = false;
                result.IntMsg = -1;
                return Json(result);
            }
            string UserOpenId = base.GetUserSession().OpenId;
            try
            {

                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select o.*,m.Name as MallName,i.Name as ItemName from O2OOrder as o 
left join O2OMall as m on m.Code = o.MallCode
left join O2OItemInfo as i on i.Id = o.ItemId
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
                        //到货确认(供应商)
                        if (InO2OOrder.O2OOrderStatus == O2OOrderStatus.Sign_Settle)
                        {
                            if(InO2OOrder.IsSign)
                            {
                                sql += string.Format(" and (o.O2OOrderStatus={0})",Convert.ToInt32(O2OOrderStatus.Settlement));
                            }
                            else
                            {
                                sql += string.Format(" and (o.O2OOrderStatus ={0} or o.O2OOrderStatus={1})",
                               Convert.ToInt32(O2OOrderStatus.Settlement),
                               Convert.ToInt32(O2OOrderStatus.ComfirmSign));
                            }
                           
                        }
                        //已结算(供应商)
                        else if (InO2OOrder.O2OOrderStatus == O2OOrderStatus.Payment_Complete)
                        {
                            sql += string.Format(" and (o.O2OOrderStatus ={0} or o.O2OOrderStatus={1})",
                                Convert.ToInt32(O2OOrderStatus.Complete),
                                Convert.ToInt32(O2OOrderStatus.Payment));
                        }
                        //全部(供应商)
                        else if (InO2OOrder.O2OOrderStatus == O2OOrderStatus.Sign_Settle_Payment_Complete)
                        {
                            sql += string.Format(" and (o.O2OOrderStatus ={0} or o.O2OOrderStatus={1} or o.O2OOrderStatus={2} or o.O2OOrderStatus={3})",
                              Convert.ToInt32(O2OOrderStatus.Settlement),
                              Convert.ToInt32(O2OOrderStatus.Payment),
                              Convert.ToInt32(O2OOrderStatus.Complete),
                              Convert.ToInt32(O2OOrderStatus.ComfirmSign));
                        }
                      
                        else
                            sql += string.Format(" and o.O2OOrderStatus ={0}", Convert.ToInt32(InO2OOrder.O2OOrderStatus));
                    }
                    if(userSession.UserRole != UserRole.Administrator)
                    {
                        sql  += string.Format(" and o.WHOpenId ='{0}'", userSession.OpenId);
                    }

                    sql += " order by o.CreateDateTime desc";

                    var list = db.Database.SqlQuery<RO2OOrder>(sql);

                    if (InO2OOrder.pageIndex == 0)
                    {
                        result.resultList = list.Take(InO2OOrder.pageSize).ToList();
                    }
                    else
                    {
                        result.resultList = list.Skip(InO2OOrder.pageIndex * InO2OOrder.pageSize).Take(InO2OOrder.pageSize).ToList();
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

            int UserId = base.GetUserSession().Id;
            ViewBag.UserId = UserId;

            RO2OOrder obj = null;
            if (string.IsNullOrEmpty(O2ONo))
            {

            }
            else
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select o.*,a.Address,
AgentUI.Name as AgentName,
AgentUI.AliPayAccount as AgentAccount,
whUI.Name as WHName,
whUI.AliPayAccount as WHAccount 
from O2OOrder as o
left join O2ODeliveryAddress as a on o.AddrId = a.Id
left join UserInfo as AgentUI on AgentUI.OpenId = o.AgentOpenId
left join UserInfo as whUI on whUI.OpenId = o.WHOpenId
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

                    string sql = @"select i.Name as ItemName,agent.Name as AgentName,o.Id,o.UserPhone,o.AgentOpenId,o.WHOpenId,o.CreateDateTime,o.OrderAmount,o.O2ONo,o.O2OOrderStatus 
                    from O2OOrder as o
                    join UserInfo as agent on agent.OpenId = o.AgentOpenId
					join O2OItemInfo as i on i.Id = o.ItemId
                    where o.O2ONo = '{0}'
";
                    sql = string.Format(sql, InOrderReview.O2ONo);
                    RO2OOrder order = db.Database.SqlQuery<RO2OOrder>(sql).FirstOrDefault();
                    

                   // EO2OOrder order = db.DBO2OOrder.Where(o => o.O2ONo == InOrderReview.O2ONo).FirstOrDefault();
                    if (order.O2OOrderStatus != O2OOrderStatus.OrderReview && order.O2OOrderStatus != O2OOrderStatus.OrderRefused)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -2;
                        result.ErrorMsg = string.Format("订单状态【{0}】不正确", order.O2OOrderStatus);
                        return Json(result);

                    }
                    if (InOrderReview.IsApprove)
                    {
                        db.DBO2OOrder.Where(a => a.O2ONo == InOrderReview.O2ONo).Update(a => new EO2OOrder
                        {
                            MallOrderNo = InOrderReview.MallOrderNo,
                            OrderAmount = InOrderReview.OrderAmount,
                            O2OOrderStatus = O2OOrderStatus.ComfirmSign,
                            ReviewDateTime = DateTime.Now

                          });
                        order.O2OOrderStatus = O2OOrderStatus.ComfirmSign;
                    }
                    else
                    {
                        db.DBO2OOrder.Where(a => a.O2ONo == InOrderReview.O2ONo).Update(a => new EO2OOrder
                        {
                            RejectReason = InOrderReview.RejectReason,
                            O2OOrderStatus = O2OOrderStatus.OrderRefused,
                            ReviewDateTime = DateTime.Now

                        });
                        order.RejectReason = InOrderReview.RejectReason;
                        order.O2OOrderStatus = O2OOrderStatus.OrderRefused;
                    }

                    order.ReviewDateTime = DateTime.Now;
                    //微信通知审核结果
                    string accessToken = this.getAccessToken(true);
                    PPReviewResultNT notice = new PPReviewResultNT(accessToken, order);
                    notice.Push();

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
            int UserId = base.GetUserSession().Id;
            ViewBag.UserId = UserId;

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

        /// <summary>
        /// 和出库商结算
        /// -5 出库商余额不足
        /// -4 订单状态不正确无法结算
        /// -3 出库商账户没有设置
        /// -2 订单号未获取
        /// -1 session失效
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderSettlementToPP()
        {
            string O2ONo = Request["O2ONo"];
            OutAPIResult result = new OutAPIResult();
            int UserId = GetUserSession().Id;
            AliPayManager payManager = new AliPayManager();
            try
            {
                if (string.IsNullOrEmpty(O2ONo))
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "订单号未获取";
                    result.IntMsg = -2;
                    return Json(result);
                }
                if (UserId == 0)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "session失效";
                    result.IntMsg = -1;
                    return Json(result);
                }

                using (AliPayContent db = new AliPayContent())
                {
                    //修改订单状态
                    EO2OOrder order = db.DBO2OOrder.Where(o => o.O2ONo == O2ONo).FirstOrDefault();
                    if (order.O2OOrderStatus != O2OOrderStatus.Settlement && order.O2OOrderStatus != O2OOrderStatus.ComfirmSign )
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "订单状态不正确无法结算";
                        result.IntMsg = -4;
                        return Json(result);
                    }

                    order.O2OOrderStatus = O2OOrderStatus.Payment;
                    order.SettlementDateTime = DateTime.Now;
                    order.SettlementUserId = UserId;
                    RUserInfo ui = db.DBUserInfo.Where(a => a.OpenId == order.WHOpenId).Select(a => new RUserInfo
                    {
                        OpenId = a.OpenId,
                        AliPayAccount = a.AliPayAccount
                    }).FirstOrDefault();

                    EUserAccountBalance ub = db.DBUserAccountBalance.Where(a => a.OpenId == order.WHOpenId &&
                                                                            a.UserAccountType == UserAccountType.O2OShippment).FirstOrDefault();

                    if (ub == null || string.IsNullOrEmpty(ui.AliPayAccount))
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "出库商账户没有设置";
                        result.IntMsg = -3;
                        return Json(result);
                    }
                    if(ub.O2OShipBalance - order.OrderAmount<0)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg =string.Format("余额不足.当前余额：{0}，订单金额：{1}",ub.O2OShipBalance, order.OrderAmount);
                        result.IntMsg = -5;
                        return Json(result);
                    }

                     
                    /*创建结算单明细
                     * PP :转账给平台（从押金扣除）
                    */
                    EO2OTranscationWH trans = new EO2OTranscationWH
                    {
                        ItemId = order.ItemId,
                        O2ONo = order.O2ONo,
                        MallCode = order.MallCode,
                        MallOrderNo = order.MallOrderNo,
                        TransferTarget = TransferTarget.PP,
                        ReceiveAccount = ui.AliPayAccount,
                        TransDateTime = DateTime.Now,
                        FeeRate = 100,
                        TransferAmount = order.OrderAmount,
                        OpenId = order.WHOpenId
                    };
                    db.DBO2OTranscationWH.Add(trans);
                    //支出
                    ub.SetBalacne(trans.TransferAmount);
                   

                    /*创建结算单明细
                    * O2OWareHouse :转账给出库商（返回佣金）
                    */
                     trans = new EO2OTranscationWH
                    {
                        ItemId = order.ItemId,
                        O2ONo = order.O2ONo,
                        MallCode = order.MallCode,
                        MallOrderNo = order.MallOrderNo,
                        TransferTarget = TransferTarget.O2OWareHouse,
                        ReceiveAccount = ui.AliPayAccount,
                        TransDateTime = DateTime.Now,
                        FeeRate = order.WHRate,
                        TransferAmount = order.OrderAmount * (order.WHRate/100),
                        OpenId = order.WHOpenId
                   };
                    db.DBO2OTranscationWH.Add(trans);
                    //收入
                    ub.SetBalacne(Math.Round(ub.O2OShipInCome + trans.TransferAmount, 2));
                 
                    ETransferAmount AliTrans = new ETransferAmount();
                    AliTrans.O2OInitForShipment(order);
                    AliTrans.TransferAmount = (float)trans.TransferAmount;
                    AliTrans = payManager.O2OTransferHandler(AliTrans, BaseController.SubApp, BaseController.SubApp);
                    if (AliTrans.TransferStatus == TransferStatus.Failure)
                        return base.ErrorResult("转账失败：" + AliTrans.Log);
                    else
                        db.DBTransferAmount.Add(AliTrans);


                    db.SaveChanges();
                  
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
        public ActionResult OrderPaymentToUser_Agent()
        {
            string O2ONo = Request["O2ONo"];
            OutAPIResult result = new OutAPIResult();
            int UserId = GetUserSession().Id;
            EO2OOrder order = null;
            EUserInfo agentUi,parentUi = null;
            AliPayManager payManager = new AliPayManager();
            EO2OAgentFeeRate agentFee = null;
            int hasError = 0;
           // EO2OMall mall = null;
            double parentCommRate = 0.2;
            try
            {
                if (string.IsNullOrEmpty(O2ONo))
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "订单号未获取";
                    result.IntMsg = -2;
                    return Json(result);
                }
                if (UserId == 0)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "session失效";
                    result.IntMsg = -1;
                    return Json(result);
                }

                using (AliPayContent db = new AliPayContent())
                {
                    order = db.DBO2OOrder.Where(a => a.O2ONo == O2ONo).FirstOrDefault();

                    if (order.O2OOrderStatus != O2OOrderStatus.Payment)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "订单状态不正确无法结算";
                        result.IntMsg = -3;
                        return Json(result);
                    }

                    agentUi = db.DBUserInfo.Where(a => a.OpenId == order.AgentOpenId).FirstOrDefault();
                    agentFee = db.DBO2OAgentFeeRate.Where(a => a.MallCode == order.MallCode && a.OpenId == order.AgentOpenId).FirstOrDefault();
                  //  mall = db.DBO2OMall.Where(a => a.Id == order.MallId).FirstOrDefault();
                    //ToAgent      
                    ETransferAmount AliTrans = new ETransferAmount();
                    AliTrans.O2OInitForAgent(order, agentUi);
                    //Agent Amount
                    AliTrans.TransferAmount = Convert.ToSingle(order.OrderAmount * ((agentFee.MarketRate - order.AgentFeeRate) / 100));
                    AliTrans = payManager.O2OTransferHandler(AliTrans, BaseController.SubApp, BaseController.SubApp);
                    if (AliTrans.TransferStatus == TransferStatus.Failure)
                        hasError++;
                    //return base.ErrorResult("转账失败：" + AliTrans.Log);
                    else
                        db.DBTransferAmount.Add(AliTrans);
                    //ToParent
                    if(!string.IsNullOrEmpty(agentUi.parentOpenId))
                    {
                        parentUi = db.DBUserInfo.Where(a => a.OpenId == agentUi.parentOpenId).FirstOrDefault();
                        AliTrans = new ETransferAmount();
                        AliTrans.O2OInitForAgent(order, parentUi);
                        //Parent Amount
                        AliTrans.TransferAmount = Convert.ToSingle(order.OrderAmount * ((agentFee.MarketRate - order.AgentFeeRate) * parentCommRate / 100));
                        AliTrans = payManager.O2OTransferHandler(AliTrans, BaseController.SubApp, BaseController.SubApp);
                        if (AliTrans.TransferStatus == TransferStatus.Failure)
                            // return base.ErrorResult("转账失败：" + AliTrans.Log);
                            hasError++;
                        else
                            db.DBTransferAmount.Add(AliTrans);
                    }

                    //ToUser
                                                            AliTrans = new ETransferAmount();
                    AliTrans.O2OInitForUser(order);
                    //User Amount
                    AliTrans.TransferAmount = Convert.ToSingle(order.OrderAmount * ((100-agentFee.MarketRate) / 100));
                    AliTrans = payManager.O2OTransferHandler(AliTrans, BaseController.SubApp, BaseController.SubApp);
                    if (AliTrans.TransferStatus == TransferStatus.Failure)
                        hasError++;
                    // return base.ErrorResult("转账失败：" + AliTrans.Log);
                    else
                        db.DBTransferAmount.Add(AliTrans);

                    order.O2OOrderStatus = O2OOrderStatus.Complete;
                    if (hasError>0)
                    {
                        order.O2OTransferStatus = TransferStatus.Failure;
                    }

                    db.SaveChanges();

                }
                         
               

                 
                // AliTrans.
                //ToUser
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult GetTransferByO2ONo()
        {
            string O2ONo = Request["O2ONo"];
            ROrder_Transfer result = new ROrder_Transfer();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    result.Order = new IQBCore.IQBPay.Models.Order.EOrderInfo();
                    result.TransferList  = db.DBTransferAmount.Where(a => a.O2ONo == O2ONo).ToList();
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
            return Json(result);
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
                        MallCode = "JD",
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

        #region Step
        public ActionResult RuleSteps()
        {
            return View();
        }

        public ActionResult StepList()
        {
            return View();
        }

        public ActionResult HashStep()
        {
            List<HashO2ORuleStep> result = null;
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = "select Code,LeftName from O2Ostep";
                    result = db.Database.SqlQuery<HashO2ORuleStep>(sql).ToList();

                }
            }
            catch(Exception ex)
            {
                
            }
            if (result == null)
                result = new List<HashO2ORuleStep>();
            return Json(result);
        }

        [HttpPost]
        public ActionResult StepListQuery()
        {
            var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);
            List<RO2OStep> result = new List<RO2OStep>();
            using (AliPayContent db = new AliPayContent())
            {
                var list = db.DBO2OStep.Select(o => new RO2OStep
                {
                    Id = o.Id,
                    Code = o.Code,
                    LeftName = o.LeftName,
                    BeginContent = o.BeginContent,
                    EndContent = o.EndContent,
                    Seq = o.Seq,
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

        [ValidateInput(false)]
        public ActionResult SaveStep(EO2OStep obj)
        {
           

            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    if (obj.Id > 0)
                    {
                        EO2OStep updateObj = db.DBO2OStep.Where(o => o.Id == obj.Id).FirstOrDefault();
                        updateObj.InitFromUpdate(obj);
                        db.Update<EO2OStep>(updateObj);
                    }
                    else
                    {
                        db.Insert<EO2OStep>(obj);
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
        public ActionResult DeleteStep()
        {
            int Id = Convert.ToInt32(Request["Id"]);
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2OStep obj = db.DBO2OStep.Where(o => o.Id == Id).FirstOrDefault();

                    db.Delete<EO2OStep>(obj);

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            return Json(result);
        }

        public ActionResult SaveRuleSteps(List<RelRuleStep> list)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    foreach (RelRuleStep rel in list)
                    {
                        if (rel.Id > 0)
                        {
                            RelRuleStep updateObj = db.DBO2ORelRuleStep.Where(o => o.Id == rel.Id).FirstOrDefault();
                            updateObj.InitFromUpdate(rel);
                            db.Update<RelRuleStep>(updateObj);
                        }
                        else
                        {
                            db.Insert<RelRuleStep>(rel);
                        }
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
        public ActionResult RuleStepsQuery()
        {
            string RuleCode = Request["RuleCode"];
            List<RelRuleStep> result = null;
            if (!string.IsNullOrEmpty(RuleCode))
            {
                using (AliPayContent db = new AliPayContent())
                {
                    result = db.DBO2ORelRuleStep.OrderBy(a=>a.Seq).ToList();
                }
            }

            if (result == null) result = new List<RelRuleStep>();
            return Json(result);


        }

        [HttpPost]
        public ActionResult DeleteRelRuleStep()
        {
            int Id = Convert.ToInt32(Request["Id"]);
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    RelRuleStep obj = db.DBO2ORelRuleStep.Where(o => o.Id == Id).FirstOrDefault();

                    db.Delete<RelRuleStep>(obj);

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


        #region Transaction 财务资金
        public ActionResult TransWH()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TransWHQuery(InO2OTrans InO2OTrans)
        {
            UserSession userSession = GetUserSession();
            List<RO2OTranscationWH> result = new List<RO2OTranscationWH>();
            try
            {
                string UserOpenId = base.GetUserSession().OpenId;

                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select o.*,m.Name as MallName,i.Name as ItemName from O2OTranscationWH as o 
                    left join O2OMall as m on m.Code = o.MallCode
                    left join O2OItemInfo as i on i.Id = o.ItemId
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
                    if(InO2OTrans.TransferTarget!= TransferTarget.All)
                    {
                        sql += string.Format(" and o.TransferTarget ={0}", Convert.ToInt32(InO2OTrans.TransferTarget));
                    }
                    if (userSession.UserRole != UserRole.Administrator)
                    {
                        sql += string.Format(" and o.OpenId ='{0}'", userSession.OpenId);
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
                        MallCode = "JD",
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

        #region O2OWap

        public ActionResult O2OEntry()
        {
            return View();
        }

        #endregion

        #region WH 商户/出货商
        public ActionResult WHInfo()
        {

            return View();
        }
        #endregion


    }
}