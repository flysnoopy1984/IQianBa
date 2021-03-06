﻿using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.Json;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.Model;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;
using IQBCore.IQBPay.Models.User;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBWX.Models.WX.Template.ReviewRemind;
using IQBCore.IQBWX.Models.WX.Template.ConfirmSign;
using IQBCore.IQBPay.Models.Result;

namespace IQBPay.Controllers
{
    public class O2OWapController : O2OBaseController
    {
        // GET: O2OWap
        public ActionResult Index()
        {
            string aoId = CheckaoId();
            string act = Request["act"];
            if (act == "switchUser")
            {
                Session[IQBConstant.SK_O2OBuyerSession] = null;
                CookieHelper.setCookie(IQBConstant.ck_O2OUser, "");
                @ViewBag.BuyerPhone = null;
                return View();
            }

            //if (!canRunO2O())
            //{
            //    return RedirectToAction("ErrorPage", new { aoId= aoId, ec = 3 });
            //}
          
          
            string un = Request.QueryString["un"];
            //不为空说明代下单
            if(!string.IsNullOrEmpty(un))
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2OBuyer buyer = db.DBO2OBuyer.Where(a => a.BuyerName == un).FirstOrDefault();

                    if (buyer == null)
                    {
                        buyer = new EO2OBuyer();
                        buyer.AgentOpenId = aoId;
                        buyer.BuyerName = un;
                        db.DBO2OBuyer.Add(buyer);
                        db.SaveChanges();
                    }
                      
                    SetBuyerSession(buyer);


                }
            }
            InitO2OPage();
            //int r = InitO2OPage();
            //if(r==-1)
            //    return  RedirectToAction("ErrorPage", new { ec = 2 });



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


            return View();
        }

        public ActionResult ErrorPage()
        {
            ErrorMsg ErrorMsg = new ErrorMsg();
            string aoId = Request["aoId"];
            int ec =Convert.ToInt32(Request["ec"]);
            string em = Request["em"];
            ErrorMsg.Code = ec.ToString();
            ErrorMsg.aoId = aoId;

            switch (ec)
            {
                //代理二维码信息失效
                case 1:
                    ErrorMsg.Msg = "没有获取二维码信息</br>请<span class='FontHL'>重新扫码</span>进入</br>或联系中介咨询！";
                    break;
                case 2:
                   
                    ErrorMsg.Msg = "超时，请返回首页重新操作！";
                    break;
                case 3:
                    ErrorMsg.Msg = "商城营业时间为：早上9点到晚上21点";
                    break;

                case 40001:
                     ErrorMsg.Msg = "商品已下架";
                    break;
                case 99:
                    ErrorMsg.Msg = em;
                    break;
                default:
                    ErrorMsg.Msg = "系统错误";
                    break;
            }
           
            return View(ErrorMsg);
        }

        public ActionResult UploadOrder()
        {
            int r = InitO2OPage();
            string aoid = base.CheckaoId();
            string un = Request.QueryString["un"];
            string act = Request.QueryString["act"];
            ViewBag.IsAdmin = false;
           

            foreach (string admin in ReviewOpenIdGroup)
            {
                if (aoid == admin)
                {
                    ViewBag.IsAdmin = true;
                    break;
                }
            }
         
            if(!ViewBag.IsAdmin)
            {
                if (r == -1)
                    return RedirectToAction("ErrorPage", new { ec = 2, aoId = aoid, un = un });

            }




            return View();
        }

        public ActionResult OrderDetail()
        {
          //  int r = InitO2OPage();
            string aoid = base.CheckaoId();
            string un = Request.QueryString["un"];
            string O2ONo = Request.QueryString["O2ONo"];
            //if (r == -1)
            //    return RedirectToAction("ErrorPage", new { ec = 2, aoId = aoid, un = un });

            using (AliPayContent db = new AliPayContent())
            {
                string User =  db.DBO2OOrder.Where(a => a.O2ONo == O2ONo).Select(a=>a.User).FirstOrDefault();
               
                EO2OBuyer buyer = new EO2OBuyer();
                buyer.AgentOpenId = aoid;
                buyer.BuyerName = User;
                this.SetBuyerSession(buyer);

                ViewBag.BuyerPhone = User;
            }

                //string Phone = this.GetBuyerPhone();

                //if (string.IsNullOrEmpty(Phone))
                //{
                //    return RedirectToAction("Index",new {aoId = aoid,un= un });
                //}

                return View();
        }

        public ActionResult OrderList()
        {
            int r = InitO2OPage();
            string aoid = base.CheckaoId();
            string un = Request.QueryString["un"];
            if (r == -1)
                return RedirectToAction("ErrorPage", new { ec = 2, aoId = aoid, un = un });

            string Phone = this.GetBuyerPhone();
            

            @ViewBag.IsAdmin = O2OBuyerSession.IsAdmin;

            if (string.IsNullOrEmpty(Phone))
            {
                return RedirectToAction("Index", new { aoId = aoid,un=un});
            }
            return View();
        }



        public ActionResult Demo()
        {
            return View();
        }

        #region Addr
        public ActionResult AddrConfirm()
        {
            int r = InitO2OPage();
            string aoid = base.CheckaoId();
            string un = Request.QueryString["un"];
            if (r == -1)
                return RedirectToAction("ErrorPage", new { ec = 2, aoId = aoid,un= un });
            return View();
        }

        /// <summary>
        /// -1 phone失效
        /// -2 未获取收货地址。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InitAddr()
        {
            NResult<HashO2ODeliveryAddr> result = new NResult<HashO2ODeliveryAddr>();
            try
            {
                string bueryPhone = this.GetBuyerPhone();
                if (string.IsNullOrEmpty(bueryPhone))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -1;
                    return Json(result);
                }
                int ItemId = Convert.ToInt32(Request["ItemId"]);
              
                string MallICode = Request["MallICode"];
                //出货商Id
                string OpenId = Request["OpenId"];

                string sql = @"select ad.Id ,ad.Address as ReceiveAddress from O2ODeliveryAddress as ad
where not exists
(
	select Id from O2OBuyerReceiveAddr as bd
	where ad.Id =bd.AddrId and bd.ItemId={0}
) and ad.OpenId = '{1}'
union
select top 1 ad.Id ,ad.Address from O2ODeliveryAddress as ad  where ad.OpenId = '{1}'

                ";
                sql = string.Format(sql, ItemId, OpenId);
                using (AliPayContent db = new AliPayContent())
                {
                    HashO2ODeliveryAddr obj =  db.Database.SqlQuery<HashO2ODeliveryAddr>(sql).FirstOrDefault();
                    if(obj == null)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -2;
                        return Json(result);
                    }
                    else
                    {

                        EO2OItemInfo ItemObj =  db.DBO2OItemInfo.Where(a => a.Id == ItemId).FirstOrDefault();
                        obj.ItemRealAddr = ItemObj.RealAddress;

                        result.resultObj = obj;
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
        #endregion

        #region Item
        public ActionResult ItemList()
        {
            int r = InitO2OPage();
            string aoid = base.CheckaoId();
            string un = Request.QueryString["un"];
            if (r == -1)
                return RedirectToAction("ErrorPage", new { ec = 2, aoId = aoid, un = un });
            return View();
        }

        [HttpPost]
        public ActionResult QueryItemList()
        {
            string MallCode = Convert.ToString(Request["MallCode"]);
            int PGId = Convert.ToInt32(Request["PGId"]);

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
                    O2ORuleCode = o.O2ORuleCode,
                    RealAddress = o.RealAddress,
                    RecordStatus=o.RecordStatus,
                    PriceGroupId = o.PriceGroupId,
                    OpenId = o.OpenId,
                   
                }).Where(o=>o.RecordStatus == RecordStatus.Normal);
                if (PGId > 0)
                    list = list.Where(o=>o.PriceGroupId == PGId);
                if(!string.IsNullOrEmpty(MallCode))
                    list = list.Where(o => o.MallCode == MallCode);
                result = list.ToList();


            }
            return Json(result);
        }

      
        #endregion

        [HttpPost]
        public ActionResult QueryPriceGrouplist()
        {
            List<EO2OPriceGroup> result = new List<EO2OPriceGroup>();
            string MallCode = Request["MallCode"];
          
          
            using (AliPayContent db = new AliPayContent())
            {
                var list = db.DBO2OPriceGroup;
                if (!string.IsNullOrEmpty(MallCode))
                {
                    string sql = @"select * from O2OPriceGroup
                            where Id in 
                            (
                                select PriceGroupId from O2OItemInfo where MallCode = '{0}'
                            )";
                    sql = string.Format(sql, MallCode);
                    result = db.Database.SqlQuery<EO2OPriceGroup>(sql).ToList();
                }
                else
                    result = db.DBO2OPriceGroup.ToList();
            }
            return Json(result);
        }

        #region Mall
        public ActionResult MallList()
        {
            int r = InitO2OPage();
            string aoid = base.CheckaoId();
            string un = Request.QueryString["un"];
            if (r == -1)
                return RedirectToAction("ErrorPage", new { ec = 2, aoId = aoid, un = un });


            return View();
        }
        [HttpPost]
        public ActionResult QueryMallList()
        {
            List<RO2OMall> result = new List<RO2OMall>();
            using (AliPayContent db = new AliPayContent())
            {
                string sql = @"select * from O2OMall
                            where Code in 
                            (
                             select MallCode from O2OItemInfo where O2OItemInfo.RecordStatus = 0
                            ) and RecordStatus = 0
                            ";
                result = db.Database.SqlQuery<RO2OMall>(sql).ToList();
            }
            return Json(result);
        }


        #endregion

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

        [HttpPost]
        public ActionResult HasBuyerOrder()
        {
           
            string Phone = GetBuyerPhone();
            int orderNum;
            using (AliPayContent db = new AliPayContent())
            {
                orderNum = db.DBO2OOrder.Where(o => o.User == Phone
                                           && (o.O2OOrderStatus != O2OOrderStatus.Complete && o.O2OOrderStatus != O2OOrderStatus.UserClose)).Count();
                if(orderNum>0)
                {
                    return Json(true);
                }
            }
            return Json(false);
               
        }

        public double GetOnOrderAmount(string WHUserOpenId)
        {
            double result;
            using (AliPayContent db = new AliPayContent())
            {
                result = db.DBO2OOrder.Where(a => a.WHOpenId == WHUserOpenId && 
                                       (
                                        a.O2OOrderStatus== O2OOrderStatus.OrderRefused ||
                                        a.O2OOrderStatus == O2OOrderStatus.OrderReview ||
                                        a.O2OOrderStatus == O2OOrderStatus.WaitingUpload
                                        )
                                    ).ToList().Sum(a=>a.OrderAmount);
            }
            return result;
            

        }

        /// <summary>
        /// 返回null代表没问题
        ///  -1 手机号为空，重新登陆
        /// -2 ItemId未获取，系统错误
        /// -3 AddrId 未获取，系统错误
        /// -4 代理qrUserId,重新登陆 
        /// -5 代理费率没有配置.
        /// -6 出货商没有配置
        /// </summary>
        /// <returns></returns>
        public OutAPIResult VerifyInitOrder()
        {
            OutAPIResult result = new OutAPIResult();
            string Phone = base.GetBuyerPhone();
            string reqItemId = Request["ItemId"];
            string reqAddrId = Request["AddrId"];
            string AgentOpenId = CheckaoId();//Request.QueryString["qrUserId"];
            if (string.IsNullOrEmpty(Phone))
            {
                result.IsSuccess = false;
                result.IntMsg = -1;
                return result;
            }
            if (string.IsNullOrEmpty(reqItemId))
            {
                result.IsSuccess = false;
                result.IntMsg = -2;
                return result;
            }
            if (string.IsNullOrEmpty(reqAddrId))
            {
                result.IsSuccess = false;
                result.IntMsg = -3;
                return result;
            }
            if (string.IsNullOrEmpty(AgentOpenId))
            {
                result.IsSuccess = false;
                result.IntMsg = -4;
                return result;
            }

            return null;
        }
        /// <summary>
        /// 返回null代表没问题
        ///  -1 手机号为空，重新登陆
        /// -2 ItemId未获取，系统错误
        /// -3 AddrId 未获取，系统错误
        /// -4 代理码失效,重新登陆 
        /// -5 代理费率没有配置.
        /// -6 出货商没有配置
        /// -7 出库商余额未配置，请联系管理员
        /// -8 出库商余额不足
        /// -9 商品已下架
        /// -10 订单已创建，请勿重复提交
        /// -11 Session失效
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateO2OOrder()
        {
           
            string Phone = base.GetBuyerPhone();

            string un = Request.QueryString["un"];
            
            
            int ItemId = 0;
            int AddrId = 0;
          //  long qrUserId = 0;
            OutAPIResult result = null;
            try
            {
                result = VerifyInitOrder();
                if (result != null) return Json(result);
                else result = new OutAPIResult();

                if (string.IsNullOrEmpty(Phone))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -11;
                    return Json(result);
                }

                ItemId = Convert.ToInt32(Request["ItemId"]);
                AddrId = Convert.ToInt32(Request["AddrId"]);


               // string sk_qrUserId = Session[IQBConstant.SK_O2OQrUserId] as string;//Convert.ToInt64(Request["qrUserId"]);

                
                string AgentOpenId = this.CheckaoId(); 

                using (AliPayContent db = new AliPayContent())
                {
                    int orderNum = db.DBO2OOrder.Where(o => o.User == Phone
                                         && o.O2OOrderStatus != O2OOrderStatus.Complete 
                                         && o.O2OOrderStatus != O2OOrderStatus.UserClose
                                         && o.ItemId == ItemId 
                                         && o.AddrId == AddrId
                                         && o.O2OOrderStatus == O2OOrderStatus.OpenOrder
                                         && o.AgentOpenId == AgentOpenId).Count();
                   
                    if(orderNum>0)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -10;
                        return Json(result); 
                    }
                    EO2OItemInfo Item = db.DBO2OItemInfo.Where(o => o.Id == ItemId).FirstOrDefault();

                    
                    EUserInfo whUser = db.DBUserInfo.Where(o => o.OpenId == Item.OpenId).FirstOrDefault();
                  //  EQRUser qrUser = db.DBQRUser.Where(a => a.ID == qrUserId);
                    if (Item.RecordStatus == RecordStatus.Blocked)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -9;
                        return Json(result);
                    }
                    //                    //费率通过寻找商城中最大费率商品获得。
                    //                    string sql = @"select max(i.ShipFeeRate) as ShipFee
                    //from O2OItemInfo as i
                    //where i.RecordStatus = 0 and i.MallId = '{0}'";
                    //                    sql = string.Format(sql, Item.MallId);

                    //string sql = "select ShipFeeRate from from O2OItemInfo where i.RecordStatus = 0";

                    //double agentFee = db.Database.SqlQuery<double>(sql).FirstOrDefault();

                    //
                    ////出库商余额是否足够
                    //EUserAccountBalance balance = db.DBUserAccountBalance.Where(a => a.OpenId == Item.OpenId).FirstOrDefault();
                    //if (balance == null)
                    //{
                    //    result.IsSuccess = false;
                    //    result.IntMsg = -7;
                    //    return Json(result);
                    //}
                    //double onOrderAmt = GetOnOrderAmount(Item.OpenId);
                    //if (onOrderAmt + Item.Amount > balance.O2OShipBalance)
                    //{
                    //    result.IsSuccess = false;
                    //    result.IntMsg = -8;
                    //    result.ErrorMsg = string.Format("出库商余额不足，请选择低于等于{0}金额的商品或联系管理员",
                    //        Math.Round((balance.O2OShipBalance - onOrderAmt),2));
                    //    return Json(result);
                    //}
                    //int MallId = Item.MallId;

                    //EO2OMall Mall = db.DBO2OMall.Where(a => a.Id == Item.MallId).FirstOrDefault();

                    EO2OAgentFeeRate AgentFeeRate = db.DBO2OAgentFeeRate
                                                      .Where(o => o.ItemId == Item.Id && o.OpenId == AgentOpenId)
                                                      .FirstOrDefault();
                    if (AgentFeeRate == null || AgentFeeRate.MarketRate == 0)
                    {
                        result.ErrorMsg = string.Format("代理没有对{0}的商品做用户手续费配置", Item.Name);
                        result.IsSuccess = false;
                        result.IntMsg = -5;
                        return Json(result);
                    }
                    double agentFee = Item.ShipFeeRate+ GlobalConfig.AgentFeeBasedShipFee+ AgentFeeRate.DiffFeeRate;

                    //EO2ORoleCharge RoleCharge = db.DBO2ORoleCharge.Where(o => o.UserOpenId == Item.OpenId && o.O2OUserRole == O2OUserRole.Shippment).FirstOrDefault();
                    //if (RoleCharge == null)
                    //{
                    //    result.IsSuccess = false;
                    //    result.IntMsg = -6;
                    //    return Json(result);
                    //}




                    EO2OOrder order = new EO2OOrder
                    {
                        //商品信息
                        ItemId = ItemId,
                        OrderAmount = Item.Amount,

                        //用户信息
                        User = Phone,
                       
                        //代理信息
                        AgentOpenId = AgentOpenId,  
                        AgentFeeRate = agentFee,
                        MarketRate = AgentFeeRate.MarketRate,

                        //出货商户信息
                        WHOpenId = Item.OpenId,
                        WHRate = Item.ShipFeeRate,
                        WHAliPayAccount = whUser.AliPayAccount,

                        //订单信息
                        AddrId = AddrId,
                        O2OOrderStatus = O2OOrderStatus.WaitingUpload,
                        O2ONo = StringHelper.GenerateO2ONo(),
                        MallCode = Item.MallCode,
                      
                        CreateDateTime = DateTime.Now,
                    };

                    if(!string.IsNullOrEmpty(un) && un != "null")
                    {
                        order.O2OOrderType = O2OOrderType.ForUser;
                    }

                    db.DBO2OOrder.Add(order);

                    db.SaveChanges();

                    ////供应商预扣
                    //balance.O2OOnOrderAmount += Item.Amount;
                    //db.Update<EUserAccountBalance>(balance);

                }

                
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
           
        }

        /// <summary>
        /// -1 手机没有
        /// -2 没有订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderDetailQuery()
        {
          //  string BuyerPhone = this.GetBuyerPhone();

            string O2ONo = Request["O2ONo"];


            NResult<RO2OStep> result = new NResult<RO2OStep>();

            try
            {
                //if (string.IsNullOrEmpty(BuyerPhone))
                //{
                //    result.IsSuccess = false;
                //    result.IntMsg = -1;
                //    return Json(result);
                //}
                if (string.IsNullOrEmpty(O2ONo))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -2;
                    result.ErrorMsg = "订单编号未获取";
                    return Json(result);
                }


                using (AliPayContent db = new AliPayContent())
                {
                    var sql = @"select top 1 i.O2ORuleCode,i.NeedSMS,
                    o.OrderAmount,o.O2OOrderStatus,o.O2ONo,o.RejectReason 
                     from O2OOrder as o
	                join O2OItemInfo as i on i.Id = o.ItemId
	                where o.O2ONo = '{0}'";

                    //if (BuyerPhone!="13482710060")
                    //{
                    //    sql += " and o.[User]='" + BuyerPhone + "'";
                    //}
                    sql += " order by o.CreateDateTime desc";

                    sql = string.Format(sql, O2ONo); 

                    RO2OStep tmpObj = db.Database.SqlQuery<RO2OStep>(sql).FirstOrDefault();
                    if(tmpObj == null)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -3;
                     
                        return Json(result);
                    }
                    result.resultObj = tmpObj;

                    sql = @"select s.*,rs.seq as rseq from O2OStep as s
                    join RelRuleStep as rs on rs.StepCode =s.Code
                    where rs.RuleCode ='{0}'
                    order by rs.Seq";
                    sql = string.Format(sql, tmpObj.O2ORuleCode);
                
                    result.resultList = db.Database.SqlQuery<RO2OStep>(sql).ToList();
                    if (result.resultList == null)
                        result.resultList = new List<RO2OStep>();
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
        public ActionResult OrderListQuery()
        {
            string BuyerPhone = this.GetBuyerPhone();
            NResult<RO2OOrder_ForAgent> result = new NResult<RO2OOrder_ForAgent>();

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            int selOS = Convert.ToInt32(Request["selOS"]);
            string aoId = Request["aoId"];
            bool isAgent = false;
            bool isAdmin = false;
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    if (string.IsNullOrEmpty(BuyerPhone))
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -1;
                        return Json(result);
                    }
                    //查看是否代理的手机号码
                    if (!string.IsNullOrEmpty(BuyerPhone))
                    {
                        EUserInfo  ui=   db.DBUserInfo.Where(a => a.UserPhone == BuyerPhone && a.OpenId == aoId).FirstOrDefault();
                        if(ui!= null && Convert.ToInt32(ui.UserRole) >=2)
                        {
                            isAgent = true;
                        }
                    }
                  

                    string sql = @"select o.O2ONo,i.Name as ItemName,o.O2OOrderType,
                                          CONVERT(varchar(100), o.CreateDateTime, 20) as CreateDateTime,
                                          o.O2OOrderStatus,o.OrderAmount,o.[User] 
                                    from O2OOrder as o 
                                   join O2OItemInfo as i on i.Id = o.ItemId";
                    
                                  
                    foreach(string admin in ReviewOpenIdGroup)
                    {
                        if(aoId ==admin)
                        {
                            isAdmin = true;
                            break;
                        }
                    }
                    if(!isAdmin)
                    {
                        if (isAgent)
                        {
                            sql += " where o.AgentOpenId='{0}'";
                            sql = string.Format(sql, aoId);
                        }
                        else
                        {
                            sql += " where o.[User]= '{0}' and o.AgentOpenId='{1}'";
                            sql = string.Format(sql, BuyerPhone, aoId);
                        }
                    }
                   

                    if (selOS !=99)
                    {
                        //对于用户来说订单处理中：
                        /*
                         * WaitingUpload = 2,OrderRefused = 5,OrderReview=6,SMS=8,ComfirmSign=10, SignCode=12, Settlement = 14,Payment=18,
                         */
                        if (selOS == 0)
                        {
                            sql += " and o.O2OOrderStatus in (2,5,6,8,10,12,14,18)";
                        }
                        /*
                           Complete=50,
                         */
                        else if (selOS ==10)
                        {
                            sql += " and o.O2OOrderStatus=50";
                        }
                    }

                    sql += " order by o.CreateDateTime desc";

                    var list = db.Database.SqlQuery<RO2OOrder_ForAgent>(sql);
                    if (pageIndex == 0)
                        result.resultList = list.Take(pageSize).ToList();
                    else
                        result.resultList = list.Skip(pageIndex * pageSize).Take(pageSize).ToList();

                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;

            }
            return Json(result);
        }

    
        /// <summary>
        /// 用户主动关闭订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderClose()
        {
            OutAPIResult result = new OutAPIResult();
            string O2ONo = Request["O2ONo"];
            try
            {
               
                using (AliPayContent db = new AliPayContent())
                {
                    EO2OOrder order = db.DBO2OOrder.Where(a => a.O2ONo == O2ONo).FirstOrDefault();
                    if(order != null)
                    { 
                        if(order.O2OOrderStatus> O2OOrderStatus.WaitingSendSMS)
                        {
                            result.IsSuccess = false;
                            result.IntMsg = -2;
                            result.ErrorMsg = "状态不对，订单不能关闭！";
                            return Json(result);
                        }
                        order.O2OOrderStatus = O2OOrderStatus.UserClose;
                        db.SaveChanges();
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -1;
                        result.ErrorMsg = "订单未获取！请联系中介";
                        return Json(result);
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

        /// <summary>
        /// 用户查看商品物流发现被签收后，确认签收
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderSignConfirm()
        {
            OutAPIResult result = new OutAPIResult();
            string O2ONo = Request["O2ONo"];
            try
            {

                using (AliPayContent db = new AliPayContent())
                {
                  //  EO2OOrder order = db.DBO2OOrder.Where(a => a.O2ONo == O2ONo).FirstOrDefault();

                    string sql = @"select i.Name as ItemName,o.Id,o.MallOrderNo,
                    o.WHOpenId,o.CreateDateTime,o.OrderAmount,o.O2OOrderStatus
                    from O2OOrder as o
					join O2OItemInfo as i on i.Id = o.ItemId
                    where o.O2ONo = '{0}'
";
                    sql = string.Format(sql, O2ONo);

                    RO2OOrder order = db.Database.SqlQuery<RO2OOrder>(sql).FirstOrDefault();

                    if (order != null)
                    {
                        if (order.O2OOrderStatus != O2OOrderStatus.ComfirmSign)
                        {
                            result.IsSuccess = false;
                            result.IntMsg = -2;
                            result.ErrorMsg = "状态不对，订单不能签收！";
                            return Json(result);
                        }
                        db.DBO2OOrder.Where(a=>a.O2ONo == O2ONo).Update(a => new EO2OOrder
                        {
                            O2OOrderStatus = O2OOrderStatus.Settlement
                        });

                        //签收通知
                        string accessToken = this.getAccessToken(true);
                        PPConfirmSignNT notice = new PPConfirmSignNT(accessToken, order);
                        notice.Push();

                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -1;
                        result.ErrorMsg = "订单未获取！请联系中介";
                        return Json(result);
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

        /// <summary>
        /// -1 图片大于2M
        /// -2 手机号未获取
        /// -3 文件格式不正确
        /// -4 订单没有获取
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadOrderInfo()
        {
            //7173 -gif
            Stream stream;
            NResult<string> result = new NResult<string>();
            string OrderNo = Request["OrderNo"];
            Dictionary<string, string> extDir = new Dictionary<string, string>();
        //    extDir.Add("255216", "jpg");
            extDir.Add("6677", "bmp");
            extDir.Add("13780", "png");
            extDir.Add("255216", "jpeg");
            try
            {
                if (string.IsNullOrEmpty(OrderNo))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -4;
                    return Json(result);
                }

                HttpPostedFileBase file0 = Request.Files[0];
                int size = file0.ContentLength / 1024; //文件大小KB
                if (size > 2048)
                {
                    result.IsSuccess = false;
                    result.IntMsg = -1;
                    return Json(result);
                }
               
                //string Phone = GetBuyerPhone();
                //if (string.IsNullOrEmpty(Phone))
                //{
                //    result.IsSuccess = false;
                //    result.IntMsg = -2;
                //    return Json(result);
                //}

                byte[] fileByte = new byte[2];//contentLength，这里我们只读取文件长度的前两位用于判断就好了，这样速度比较快，剩下的也用不到。
                stream = file0.InputStream;
                stream.Read(fileByte, 0, 2);//contentLength，还是取前两位
                                            //  Stream stream;
                string fileFlag = "";
                if (fileByte != null || fileByte.Length <= 0)//图片数据是否为空
                {
                    fileFlag = fileByte[0].ToString() + fileByte[1].ToString();
                }
             //   string[] fileTypeStr = { "255216", "6677", "13780" };//对应的图片格式jpg,bmp,png
                if (extDir.Keys.Contains(fileFlag))
                {

                    string path = "/Content/UploadFile/O2OOrder";

                    string fullpath = path;
                    if (!Directory.Exists(Server.MapPath(fullpath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(fullpath));
                    }
                    string FileName = //StringHelper.GetRnd(3, false, true, true, false, "")+
                                      OrderNo + "_" +
                                      "SN1."+
                                     // DateTime.Now.ToString("mmss") +
                                      //StringHelper.GetRnd(4, false, true, true, false, "") + "." +
                                      extDir[fileFlag];

                    //   fullpath += "/" + file0.FileName;
                    fullpath += "/" + FileName;
                    file0.SaveAs(Server.MapPath(fullpath));
                    using (AliPayContent db = new AliPayContent())
                    {
                        db.DBO2OOrder.Where(a => a.O2ONo == OrderNo).Update(a => new EO2OOrder {
                            OrderImgUrl = fullpath
                        });
                    }
                        result.resultObj = fullpath;
                }
                else
                {
                    result.IsSuccess = false;
                    result.IntMsg = -3;
                    return Json(result);
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                return Json(result);
            }
        
            return Json(result);
        }

        [HttpPost]
        public ActionResult DeleteUploadFile()
        {
            OutAPIResult result = new OutAPIResult();
            string OrderNo = Request["O2ONo"];
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    db.DBO2OOrder.Where(a => a.O2ONo == OrderNo).Update(a => new EO2OOrder
                    {
                        OrderImgUrl = "",
                    });
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
           
        }

        /// <summary>
        /// -1 没有订单编号
        /// -2 没有上传图片地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitUpload()
        {
            string OrderNo = Request["OrderNo"];
            string reqOS = Request["OrderStatus"];
            
            O2OOrderStatus OrderStatus =(O2OOrderStatus)Enum.Parse(typeof(O2OOrderStatus), reqOS);
            string imgUpload1 = Request["imgUpload1"];
            string ReceiveAccount = Request["ReceiveAccount"];
            string MallOrderNo = Request["MallOrderNo"];
            var MallAccount = Request["MallAccount"];
            var MallPwd = Request["MallPwd"];
            var UserPhone = Request["UserPhone"];

            OutAPIResult result = new OutAPIResult();
            try
            {
                if (string.IsNullOrEmpty(OrderNo))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -1;
                    return Json(result);
                }
                //if (string.IsNullOrEmpty(imgUpload1))
                //{
                //    result.IsSuccess = false;
                //    result.IntMsg = -2;
                //    return Json(result);
                //}
                if (string.IsNullOrEmpty(ReceiveAccount))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -3;
                    return Json(result);
                }

                if (string.IsNullOrEmpty(reqOS) || (OrderStatus != O2OOrderStatus.WaitingUpload &&
                                                    OrderStatus != O2OOrderStatus.OrderRefused && 
                                                    OrderStatus != O2OOrderStatus.OrderReview)
                  )
                {
                    result.IsSuccess = false;
                    result.IntMsg = -4; //订单状态不正确
                    return Json(result);
                }


                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select agent.Name as AgentName,w.Name as WHName,o.Id,o.[User],o.AgentOpenId,o.WHOpenId,o.CreateDateTime,o.OrderAmount,o.O2ONo,o.O2OOrderStatus 
                    from O2OOrder as o
                    join UserInfo as agent on agent.OpenId = o.AgentOpenId
                    join UserInfo as w on w.OpenId = o.WHOpenId
                    where o.O2ONo = '{0}'";
                    sql = string.Format(sql, OrderNo);

                    RO2OOrder ROrder = db.Database.SqlQuery<RO2OOrder>(sql).FirstOrDefault();
                    db.DBO2OOrder.Where(a => a.Id == ROrder.Id).Update(a => new EO2OOrder
                    { 
                        O2OOrderStatus = O2OOrderStatus.OrderReview,
                        UserAliPayAccount = ReceiveAccount,
                      //  MallOrderNo = MallOrderNo,
                     //   OrderImgUrl = imgUpload1,
                        MallAccount = MallAccount,
                        MallPwd = MallPwd,
                        UserPhone = UserPhone,

                    });
                     
                     //微信提醒：
                    string accessToken = this.getAccessToken(true);
                    foreach(string openId in O2OBaseController.ReviewOpenIdGroup)
                    {

                        PPReviewRemindNT notice = new PPReviewRemindNT(accessToken,openId, ROrder);
                        notice.Push();
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
        public ActionResult UploadOrderQuery()
        {
            NResult<RO2OOrder> result = new NResult<RO2OOrder>();
            string OrderNo = Request["OrderNo"];
            try
            {
                if (string.IsNullOrEmpty(OrderNo))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -1;
                    return Json(result);
                }
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select o.OrderImgUrl,o.UserAliPayAccount,o.MallOrderNo,o.OrderAmount,o.MallAccount,
                                          o.MallPwd,o.MallCode,o.O2OOrderStatus,o.UserPhone,ui.UserPhone as AgentPhone
                                   from O2OOrder as o 
                                   left join UserInfo as ui on ui.OpenId = o.AgentOpenId 
                                   where o.O2ONo='{0}'";

                    sql = string.Format(sql, OrderNo);

                    result.resultObj = db.Database.SqlQuery<RO2OOrder>(sql).Select(b => new RO2OOrder
                    {
                        OrderImgUrl = b.OrderImgUrl,
                        UserAliPayAccount = b.UserAliPayAccount,
                        MallOrderNo = b.MallOrderNo,
                        OrderAmount = b.OrderAmount,
                        MallAccount = b.MallAccount,
                        MallPwd = b.MallPwd,
                        MallCode = b.MallCode,
                        O2OOrderStatus = b.O2OOrderStatus,
                        UserPhone = b.UserPhone,
                        AgentPhone = b.AgentPhone,
                    }).FirstOrDefault();


                    //result.resultObj = db.DBO2OOrder.Where(a => a.O2ONo == OrderNo).Select(b => new RO2OOrder
                    //{
                    //    OrderImgUrl = b.OrderImgUrl,
                    //    UserAliPayAccount = b.UserAliPayAccount,
                    //    MallOrderNo = b.MallOrderNo,
                    //    OrderAmount = b.OrderAmount,
                    //    MallAccount = b.MallAccount,
                    //    MallPwd = b.MallPwd,
                    //    MallCode = b.MallCode,
                    //    O2OOrderStatus = b.O2OOrderStatus,
                    //    UserPhone = b.UserPhone,

                    //}).FirstOrDefault();
                    //if (result.resultObj == null)
                    //    result.resultObj = new RSimpleEntity();
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }

        //[HttpPost]
        //public ActionResult CloseOrder()
        //{
           

        //    return View();
        //}

        public ActionResult PayToUser()
        {
            return View();
        }

        public ActionResult StepReceiveSMS()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoSMSConfirm()
        {
            OutAPIResult result = new OutAPIResult();
            string O2ONo = Request["O2ONo"];
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2OOrder order = db.DBO2OOrder.Where(a => a.O2ONo == O2ONo).FirstOrDefault();
                    if(order == null)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -1;
                        result.ErrorMsg = "没有获取对应的订单";
                        return Json(result);
                    }
                    else
                    {
                        order.HasSMS = true;
                        if (Convert.ToInt32(order.O2OOrderStatus)<=8)
                        {
                            order.O2OOrderStatus = O2OOrderStatus.SignCodeInfo; 
                        }
                        db.SaveChanges();

                    }
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = result.ErrorMsg;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult Order_SMSInfo()
        {
            NResult<RSimpleEntity> result = new NResult<RSimpleEntity>();
            string O2ONo = Request["O2ONo"];
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    RSimpleEntity order = db.DBO2OOrder.Where(a => a.O2ONo == O2ONo).Select(a => new RSimpleEntity()
                    {
                        v1 = a.O2ONo,
                        v4 = a.HasSMS,
                    }).FirstOrDefault();

                    if (order == null)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -1;
                        result.ErrorMsg = "没有获取对应的订单";
                        return Json(result);
                    }
                    result.resultObj = order;
                    return Json(result);

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = result.ErrorMsg;
            }
            return Json(result);
        }

        public ActionResult StepReceiveConSign()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Order_SignCodeInfo()
        {
            NResult<RO2OOrder_SignCode> result = new NResult<RO2OOrder_SignCode>();
            string O2ONo = Request["O2ONo"];
            try
            {
                using (AliPayContent db = new AliPayContent())
                {

                    string sql = @"select o.O2ONo,o.HasSignCode ,oc.SignCode
from O2OOrder as o 
left join O2OOrder_SignCode as oc
on oc.O2ONo = o.O2ONo where o.O2ONo='{0}'";
                    sql = string.Format(sql, O2ONo);

                    RO2OOrder_SignCode order = db.Database.SqlQuery<RO2OOrder_SignCode>(sql).Select(a => new RO2OOrder_SignCode()
                    {
                       HasSignCode = a.HasSignCode,
                       O2ONo = a.O2ONo,
                       SignCode = a.SignCode
                    }).FirstOrDefault();

                    if (order == null)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -1;
                        result.ErrorMsg = "没有获取对应的订单";
                        return Json(result);
                    }
                    result.resultObj = order;
                    return Json(result);

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = result.ErrorMsg;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult DoSignCode()
        {
            OutAPIResult result = new OutAPIResult();
            string O2ONo = Request["O2ONo"];
            string Code = Request["Code"];
            bool HasCode = Convert.ToBoolean(Request["HasCode"]);
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EO2OOrder order = db.DBO2OOrder.Where(a => a.O2ONo == O2ONo).FirstOrDefault();
                    if (order == null)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -1;
                        result.ErrorMsg = "没有获取对应的订单";
                        return Json(result);
                    }
                    else
                    {
                        order.HasSignCode = HasCode;
                        if (HasCode && !string.IsNullOrEmpty(Code))
                        {
                            EO2OOrder_SignCode CodeObj = db.DBO2OOrder_SignCode.Where(a => a.O2ONo == O2ONo).FirstOrDefault();
                            if (CodeObj == null)
                            {
                                CodeObj = new EO2OOrder_SignCode();
                                CodeObj.O2ONo = O2ONo;
                                CodeObj.SignCode = Code;
                                db.DBO2OOrder_SignCode.Add(CodeObj);

                            }
                            else
                            {
                                CodeObj.SignCode = Code;
                            }
                          
                        }
                        if (Convert.ToInt32(order.O2OOrderStatus) <= 12)
                        {
                            order.O2OOrderStatus = O2OOrderStatus.Settlement;
                        }
                        db.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = result.ErrorMsg;
            }
            return Json(result);
        }


        #endregion

        #region User

        [HttpPost]
        public ActionResult NewBuyer()
        {
            OutAPIResult result = new OutAPIResult();
            string phone = Request["Phone"];
            string aoId = Request["aoId"];
            try
            {
                using (AliPayContent db = new AliPayContent())
                {

                    EO2OBuyer buyer = db.DBO2OBuyer.Where(o => o.Phone == phone).FirstOrDefault();
                    if (buyer == null)
                    {
                        buyer = new EO2OBuyer();
                        buyer.Phone = phone;
                        buyer.AgentOpenId = aoId;
                        db.Insert<EO2OBuyer>(buyer);
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


        /// <summary>
        /// 0 用户不存在.1用户存在没有订单。2用户有订单。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login()
        {
            NResult<EO2OBuyer> result = new NResult<EO2OBuyer>();
            string UserNum = Request["UserNum"];
         
           // string aoId = Request.QueryString["aoId"];

            // string 
            try
            {
                if(!string.IsNullOrEmpty(UserNum))
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        //检查用户是否存在
                        EO2OBuyer buyer = db.DBO2OBuyer.Where(o => o.Phone == UserNum).FirstOrDefault();
                       
                        if (buyer == null)
                        {
                            EUserInfo agent = db.DBUserInfo.Where(o => o.UserPhone == UserNum).FirstOrDefault();
                            if (agent != null)
                            {
                                buyer = new EO2OBuyer();
                                buyer.AgentOpenId = agent.OpenId;
                                buyer.BuyerName = UserNum;
                                buyer.Phone = UserNum;

                            }
                            else
                            {
                                result.IntMsg = 0;
                                return Json(result);
                            }
                                
                        }

                        base.SetBuyerSession(buyer);
                        base.SetBuyerCookie(buyer);
                        result.IntMsg = 1;
                        result.resultObj = buyer;

                    }
                }
                else
                {
                    return ErrorResult("没有输入用户信息");
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginFormPP()
        {
         //   string UserNum = Request.QueryString["UserNum"];
            string gt = Request.QueryString["goto"];
            string aoid = base.CheckaoId();
            using (AliPayContent db = new AliPayContent())
            {
              
                //检查用户是否存在
                EUserInfo ui = db.DBUserInfo.Where(o => o.OpenId == aoid).FirstOrDefault();
                if (ui != null && !string.IsNullOrEmpty(ui.UserPhone))
                {
                    EO2OBuyer buyer = new EO2OBuyer();
                    buyer.AgentOpenId = ui.OpenId;
                    buyer.BuyerName = ui.UserPhone;

                    base.SetBuyerSession(buyer);
                    base.SetBuyerCookie(buyer);

                }
                else
                {
                    return RedirectToAction("ErrorPage", new { ec = 99, em = "没有找到您的手机号", aoId = aoid });
                }
            }
            if(string.IsNullOrEmpty(gt))
            {
                return View();
            }
            else
            {
                return RedirectToAction(gt, new { aoId = aoid });
            }
           
        }
        
        [HttpPost]
        public ActionResult VerifyAliPayAccount()
        {
            AliPayManager payMag = new AliPayManager();
            OutAPIResult result = new OutAPIResult();
            string AliPayAccount = Request["AliPayAccount"];
            string BuyerPhone = this.GetBuyerPhone();
            try
            {
                if(string.IsNullOrEmpty(AliPayAccount))
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "账户为空，请填写";
                    return Json(result);
                }
                if (string.IsNullOrEmpty(BuyerPhone))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -1;
                    result.ErrorMsg = "用户手机号未获取，请重新登陆";
                    return Json(result);
                }
                using (AliPayContent db = new AliPayContent())
                {
                    EBuyerInfo buyer =  db.DBBuyerInfo.Where(a => a.AliPayAccount == AliPayAccount).FirstOrDefault();
                    if(buyer == null)
                    {
                        result = payMag.TestAccountByTransfer(AliPayAccount, BaseController.SubApp);
                        if (result.IsSuccess)
                        {
                            buyer.PhoneNumber = BuyerPhone;
                            buyer.AliPayAccount = AliPayAccount;
                            buyer.BuyerType = BuyerType.O2O;
                            buyer.TransDate = DateTime.Now;
                            db.DBBuyerInfo.Add(buyer);
                            db.SaveChanges();
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


        #endregion

        #region Admin
        public ActionResult WHReCharge()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WHReChargeQuery()
        {
            string WHName = Request["WHName"];
            NResult<RUserAccountBalance> result = new NResult<RUserAccountBalance>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select b.OpenId,b.O2OShipBalance,ui.Name as UserName from UserAccountBalance as b
join UserInfo as ui on ui.OpenId = b.OpenId where 1=1 and b.UserAccountType = {0}";

                    sql = string.Format(sql, Convert.ToInt32(UserAccountType.O2OShippment));
                    if(!string.IsNullOrEmpty(WHName))
                    {
                        sql += string.Format(" and ui.Name like '%{0}%'",WHName);
                    }
                    result.resultList = db.Database.SqlQuery<RUserAccountBalance>(sql).ToList();

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
        public ActionResult DoReCharge()
        {
            string WHOpenId = Request["WHOpenId"];
            double amt =Convert.ToDouble(Request["amt"]);

            OutAPIResult result = new OutAPIResult();
            try
            {
                if(amt <=0)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "金额不能小于零";
                    return Json(result);
                }
                using (AliPayContent db = new AliPayContent())
                {
                    EUserAccountBalance balance = db.DBUserAccountBalance.Where(a => a.OpenId == WHOpenId).FirstOrDefault();
                  
                    RUserInfo ui = db.DBUserInfo.Where(a => a.OpenId == WHOpenId).Select(a => new RUserInfo
                    {
                        OpenId = a.OpenId,
                        AliPayAccount = a.AliPayAccount
                    }).FirstOrDefault();


                    EO2OTranscationWH trans = new EO2OTranscationWH
                    {
                        TransferTarget = TransferTarget.ReCharge,
                        ReceiveAccount = ui.AliPayAccount,
                        TransDateTime = DateTime.Now,
                        TransferAmount = amt,
                        OpenId = WHOpenId
                    };

                    db.DBO2OTranscationWH.Add(trans);
                    balance.SetBalacne(trans.TransferAmount);
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
        public ActionResult ReChargeLog()
        {
            NResult<RO2OTranscationWH> result = new NResult<RO2OTranscationWH>();
            try
            {
                string WHOpenId = Request["WHOpenId"];
                using (AliPayContent db = new AliPayContent())
                {
                    var list = db.DBO2OTranscationWH
                                          .Where(a => a.OpenId == WHOpenId && 
                                                 a.TransferTarget == TransferTarget.ReCharge)
                                                 .Select(a=>new RO2OTranscationWH()
                                         {
                                            TransDateTime = a.TransDateTime,
                                            TransferAmount = a.TransferAmount,
                                            OpenId = a.OpenId
                                         }).OrderByDescending(a=>a.TransDateTime);

                    result.resultList = list.Take(20).ToList();
                }
               
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }

        #endregion

        #region User Wap
        public ActionResult ContactUS()
        {
            this.InitO2OPage();

            string agentOpenId = CheckaoId();
            string un = Request.QueryString["un"];
            if(string.IsNullOrEmpty(agentOpenId))
            {
                return RedirectToAction("ErrorPage", new { ec = 1,aoId=agentOpenId,un=un });
            }
            return View();
        }

        public ActionResult EntryOrderForUser()
        {
            string aoId = CheckaoId();
            if (!canRunO2O())
            {
                return RedirectToAction("ErrorPage", new { aoId = aoId, ec = 3 });
            }

            return View();
        }

        public ActionResult Learning()
        {
            return View();
        }
        #endregion
    }
}