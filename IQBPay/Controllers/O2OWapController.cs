using IQBCore.Common.Constant;
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

namespace IQBPay.Controllers
{
    public class O2OWapController : O2OBaseController
    {
        // GET: O2OWap
        public ActionResult Index()
        {


            CheckaoId();
            string act = Request["act"];
            if(act == "switchUser")
            {
                Session["BuyerSession"] = null;
            }
        //    aoId = "1607";

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

            int ec =Convert.ToInt32(Request["ec"]);
            ErrorMsg.Code = ec.ToString();

            switch (ec)
            {
                //代理二维码信息失效
                case 1:
                    ErrorMsg.Msg = "没有获取二维码信息</br>请重新扫码进入</br>或联系中介咨询！";
                    break;

                case 40001:
                     ErrorMsg.Msg = "商品已下架";
                    break;
                default:
                    ErrorMsg.Msg = "系统错误";
                    break;
            }
           
            return View(ErrorMsg);
        }

        public ActionResult UploadOrder()
        {
            InitO2OPage();

            @ViewBag.IsAdmin = O2OBuyerSession.IsAdmin;

            return View();
        }

        public ActionResult OrderDetail()
        {
            InitO2OPage();

            string aoid = base.CheckaoId();

            string Phone = this.GetBuyerPhone();


            if (string.IsNullOrEmpty(Phone))
            {
                return RedirectToAction("Index",new {aoId = aoid });
            }
            //string Order = this.GetCurrentOrder(Phone);
            //if (string.IsNullOrEmpty(Order))
            //{
            //    return RedirectToAction("MallList", new { aoId = aoid });
            //}

            return View();
        }

        public ActionResult OrderList()
        {
            InitO2OPage();

            string aoid = base.CheckaoId();

            string Phone = this.GetBuyerPhone();

            @ViewBag.IsAdmin = O2OBuyerSession.IsAdmin;

            if (string.IsNullOrEmpty(Phone))
            {
                return RedirectToAction("Index", new { aoId = aoid });
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
            InitO2OPage();
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
              
                string MallId = Request["MallId"];
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
            InitO2OPage();
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
                    O2ORuleCode = o.O2ORuleCode,
                    RealAddress = o.RealAddress,
                    RecordStatus=o.RecordStatus,
                    PriceGroupId = o.PriceGroupId,
                    OpenId = o.OpenId,
                   
                }).Where(o=>o.RecordStatus == RecordStatus.Normal);
                if (PGId > 0)
                    list = list.Where(o=>o.PriceGroupId == PGId);
                if(MallId>0)
                    list = list.Where(o => o.MallId == MallId);
                result = list.ToList();


            }
            return Json(result);
        }

      
        #endregion

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

        #region Mall
        public ActionResult MallList()
        {
            InitO2OPage();


            return View();
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
                orderNum = db.DBO2OOrder.Where(o => o.UserPhone == Phone
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
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateO2OOrder()
        {
            string Phone = base.GetBuyerPhone();   
            int ItemId = 0;
            int AddrId = 0;
          //  long qrUserId = 0;
            OutAPIResult result = null;
            try
            {
                result = VerifyInitOrder();
                if (result != null) return Json(result);
                else result = new OutAPIResult();

                ItemId = Convert.ToInt32(Request["ItemId"]);
                AddrId = Convert.ToInt32(Request["AddrId"]);

               // string sk_qrUserId = Session[IQBConstant.SK_O2OQrUserId] as string;//Convert.ToInt64(Request["qrUserId"]);

                
                string AgentOpenId = this.CheckaoId(); 

                using (AliPayContent db = new AliPayContent())
                {
                    int orderNum = db.DBO2OOrder.Where(o => o.UserPhone == Phone
                                         && (o.O2OOrderStatus != O2OOrderStatus.Complete && o.O2OOrderStatus != O2OOrderStatus.UserClose)).Count();
                   
                    if(orderNum>0)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -10;
                        return Json(result); 
                    }
                    EO2OItemInfo Item = db.DBO2OItemInfo.Where(o => o.Id == ItemId).FirstOrDefault();
                    EUserInfo whUser = db.DBUserInfo.Where(o => o.OpenId == AgentOpenId).FirstOrDefault();
                  //  EQRUser qrUser = db.DBQRUser.Where(a => a.ID == qrUserId);
                    if (Item.RecordStatus == RecordStatus.Blocked)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -9;
                        return Json(result);
                    }

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
                    int MallId = Item.MallId;

                    EO2OMall Mall = db.DBO2OMall.Where(a => a.Id == Item.MallId).FirstOrDefault();

                    EO2OAgentFeeRate AgentFeeRate = db.DBO2OAgentFeeRate.Where(o => o.MallId == MallId && o.OpenId == AgentOpenId).FirstOrDefault();
                    if (AgentFeeRate == null)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -5;
                        return Json(result);
                    }

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
                        UserPhone = Phone,
                       
                        //代理信息
                        AgentOpenId = AgentOpenId,  
                        FeeRate = Mall.FeeRate,
                        MarketRate = AgentFeeRate.MarketRate,

                        //出货商户信息
                        WHOpenId = Item.OpenId,
                        WHRate = Item.ShipFeeRate,
                        WHAliPayAccount = whUser.AliPayAccount,

                        //订单信息
                        AddrId = AddrId,
                        O2OOrderStatus = O2OOrderStatus.WaitingUpload,
                        O2ONo = StringHelper.GenerateO2ONo(),
                        MallId = MallId,
                        MallFeeRate = Mall.FeeRate,
                        CreateDateTime = DateTime.Now,
                    };

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
            string BuyerPhone = this.GetBuyerPhone();

            string O2ONo = Request["O2ONo"];


            NResult<RO2OStep> result = new NResult<RO2OStep>();

            try
            {
                if (string.IsNullOrEmpty(BuyerPhone))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -1;
                    return Json(result);
                }
                

                using (AliPayContent db = new AliPayContent())
                {
                    var sql = @"select top 1 i.O2ORuleCode,o.OrderAmount,o.O2OOrderStatus,o.O2ONo,o.RejectReason from O2OOrder as o
	                join O2OItemInfo as i on i.Id = o.ItemId
	                where o.UserPhone = '{0}'";

                    if(!string.IsNullOrEmpty(O2ONo))
                    {
                        sql += " and o.O2ONo='" + O2ONo + "'";
                    }
	                sql += " order by o.CreateDateTime desc";

                    sql = string.Format(sql, BuyerPhone); 

                    RO2OStep tmpObj = db.Database.SqlQuery<RO2OStep>(sql).FirstOrDefault();
                    if(tmpObj == null)
                    {
                        result.IsSuccess = false;
                        result.IntMsg = -2;
                     
                        return Json(result);
                    }
                    result.resultObj = tmpObj;

                    sql = @"select s.* from O2OStep as s
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

            try
            {
                if (string.IsNullOrEmpty(BuyerPhone))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -1;
                    return Json(result);
                }


                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select o.O2ONo,i.Name as ItemName,CONVERT(varchar(100), o.CreateDateTime, 20) as CreateDateTime,
                                          o.O2OOrderStatus,o.OrderAmount from O2OOrder as o 
                                   join O2OItemInfo as i on i.Id = o.ItemId
                                   where o.UserPhone= '{0}'";

                    sql = string.Format(sql, BuyerPhone);
                    if(selOS !=99)
                    {
                        //对于用户来说订单处理中：
                        /*
                         * WaitingUpload = 2,OrderRefused = 5,OrderReview=6,ComfirmSign=10,  Settlement = 14,Payment=18,
                         */
                        if (selOS == 0)
                        {
                            sql += " and o.O2OOrderStatus in (2,5,6,10,14,18)";
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
                        if(order.O2OOrderStatus> O2OOrderStatus.ComfirmSign)
                        {
                            result.IntMsg = -2;
                            result.ErrorMsg = "状态不对，订单不能关闭！";
                            return Json(result);
                        }
                        order.O2OOrderStatus = O2OOrderStatus.UserClose;
                        db.SaveChanges();
                    }
                    else
                    {
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
                    EO2OOrder order = db.DBO2OOrder.Where(a => a.O2ONo == O2ONo).FirstOrDefault();
                    if (order != null)
                    {
                        if (order.O2OOrderStatus != O2OOrderStatus.ComfirmSign)
                        {
                            result.IntMsg = -2;
                            result.ErrorMsg = "状态不对，订单不能签收！";
                            return Json(result);
                        }
                        order.O2OOrderStatus = O2OOrderStatus.Settlement;
                        db.SaveChanges();
                    }
                    else
                    {
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
        /// -1 图片大于20M
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
                if (size > 10480)
                {
                    result.IsSuccess = false;
                    result.IntMsg = -1;
                    return Json(result);
                }
               
                string Phone = GetBuyerPhone();
                if (string.IsNullOrEmpty(Phone))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -2;
                    return Json(result);
                }

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
                    string FileName = Phone + "_" +
                                      OrderNo + "_" +
                                      "SN1."+
                                     // DateTime.Now.ToString("mmss") +
                                      //StringHelper.GetRnd(4, false, true, true, false, "") + "." +
                                      extDir[fileFlag];

                    //   fullpath += "/" + file0.FileName;
                    fullpath += "/" + FileName;
                    file0.SaveAs(Server.MapPath(fullpath));

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
            OutAPIResult result = new OutAPIResult();
            try
            {
                if (string.IsNullOrEmpty(OrderNo))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -1;
                    return Json(result);
                }
                if (string.IsNullOrEmpty(imgUpload1))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -2;
                    return Json(result);
                }
                if (string.IsNullOrEmpty(ReceiveAccount))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -3;
                    return Json(result);
                }

                if (string.IsNullOrEmpty(reqOS) || (OrderStatus != O2OOrderStatus.WaitingUpload && OrderStatus!= O2OOrderStatus.OrderRefused))
                {
                    result.IsSuccess = false;
                    result.IntMsg = -4; //订单状态不正确
                    return Json(result);
                }


                using (AliPayContent db = new AliPayContent())
                {
                  
                    db.DBO2OOrder.Where(a => a.O2ONo == OrderNo).Update(a => new EO2OOrder {
                        OrderImgUrl = imgUpload1,
                        O2OOrderStatus = O2OOrderStatus.OrderReview,
                        UserAliPayAccount = ReceiveAccount,
                        MallOrderNo = MallOrderNo,

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

        [HttpPost]
        public ActionResult UploadOrderQuery()
        {
            NResult<RSimpleEntity> result = new NResult<RSimpleEntity>();
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

                    result.resultObj= db.DBO2OOrder.Where(a => a.O2ONo == OrderNo).Select(b =>new RSimpleEntity
                    {
                        v1 = b.OrderImgUrl,
                        v2 = b.UserAliPayAccount,

                    }).FirstOrDefault();
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

    
        #endregion

        #region Data
        public EO2OOrder GetO2OOrder()
        {
            EO2OOrder order = new EO2OOrder();
            return order;
        }
        #endregion

        #region User

        [HttpPost]
        public ActionResult NewBuyer()
        {
            OutAPIResult result = new OutAPIResult();
            string phone = Request["Phone"];
            try
            {
                using (AliPayContent db = new AliPayContent())
                {

                    EO2OBuyer buyer = db.DBO2OBuyer.Where(o => o.Phone == phone).FirstOrDefault();
                    if (buyer == null)
                    {
                        buyer = new EO2OBuyer();
                        buyer.Phone = phone;
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
            string Phone = Request["Phone"];
           
           // string 
            try
            {
                if(!string.IsNullOrEmpty(Phone))
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        //检查用户是否存在
                        EO2OBuyer buyer =  db.DBO2OBuyer.Where(o => o.Phone == Phone).FirstOrDefault();
                        if (buyer !=null)
                        {
                            //检查订单是否存在
                            int orderNum = db.DBO2OOrder.Where(o => o.UserPhone == Phone
                                           && (o.O2OOrderStatus != O2OOrderStatus.Complete && o.O2OOrderStatus != O2OOrderStatus.UserClose)).Count();
                            if (orderNum > 0)
                                result.IntMsg = 2;   
                            else
                                result.IntMsg = 1;

                            result.resultObj = buyer;

                            base.SetBuyerSession(buyer);
                            base.SetBuyerCookie(buyer);
                        }
                        else
                            result.IntMsg = 0;  
                    }
                }
                else
                {
                    return ErrorResult("没有获取手机号");
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
    }
}