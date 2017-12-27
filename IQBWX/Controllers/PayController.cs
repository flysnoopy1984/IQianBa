using IQBCore.Common.Helper;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBWX.Const;
using IQBWX.BLL;
using IQBWX.BLL.NT;
using IQBWX.Common;
using IQBWX.DataBase;
using IQBWX.Models.JsonData;
using IQBWX.Models.Order;
using IQBWX.Models.Product;
using IQBWX.Models.Results;
using IQBWX.Models.Transcation;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace IQBWX.Controllers
{
    public class PayController : WXBaseController
    {
        IQBLog log = null;
        public PayController()
        {
            log = new IQBLog();
        }

        // GET: Pay
        public ActionResult Index()
        {          
            return View();
        }

        /// <summary>
        /// 提现测试
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {
            EPPayment epPay = new EPPayment();
 
             ViewData["ResponseStr"] = epPay.PostEPPay("orKUAw16WK0BmflDLiBYsR-Kh5bE", 1);
            return View();
        }
        
        private bool VerifyUserCode()
        {
            return true;
        }

        [HttpPost]
        public ActionResult GetAvailDeposit()
        {

            string openId = this.GetOpenId();
            ROutPay result = new ROutPay();
            using (UserContent db = new UserContent())
            {
                var obj = db.MemberInfo.Where(m => m.openId == openId).Select(m => new ROutPay { Amount = m.AvailDeposit, openId = m.openId });
                var list = obj.ToList();
                if(list.Count>0)
                    result = obj.ToList()[0];
            }
            log.log("GetAvailDeposit Result:" + Json(result));
            return Json(result);
        }

//        [HttpPost]
//        public ActionResult DoDepositTest()
//        {
//            WXError error = null;
//            ROutPay result = new ROutPay();
//            string wxResult;
//            UserContent udb = new UserContent();
//            TransContent tdb = new TransContent();

//            try
//            {
//                string openId = this.GetOpenId(true);
//                string oamt = Request["amt"];
//                decimal amt;
//                if (string.IsNullOrEmpty(oamt)) amt = 0;
//                else amt = Convert.ToDecimal(oamt);
//                if (amt <= 0)
//                {
//                    result.OutResult = -1;
//                    result.ResultRemark = "余额不足不能提款";
//                    return Json(result);
//                }
//                using (TransactionScope ts = new TransactionScope())
//                {
               
//                    EPPayment epPay = new EPPayment();
//                    //对于返回结果还是要判断下
//                    wxResult = @"<xml>
//<return_code><![CDATA[SUCCESS]]></return_code>
//<return_msg><![CDATA[每个红包的平均金额必须在1.00元到200.00元之间.]]></return_msg>
//<result_code><![CDATA[FAIL]]></result_code>
//<err_code><![CDATA[MONEY_LIMIT]]></err_code>
//<err_code_des><![CDATA[每个红包的平均金额必须在1.00元到200.00元之间.]]></err_code_des>
//<mch_billno><![CDATA[1440325302201704281493369062]]></mch_billno>
//<mch_id><![CDATA[1440325302]]></mch_id>
//<wxappid><![CDATA[wx041fa0b86badb080]]></wxappid>
//<re_openid><![CDATA[orKUAw16WK0BmflDLiBYsR-Kh5bE]]></re_openid>
//<total_amount>10</total_amount>
//</xml>";
                    
//                    error = epPay.GetResult(wxResult);
              
//                    ts.Complete();
//                }
//            }
//            catch (Exception ex)
//            {
//                result.OutResult = -2;
//                result.ResultRemark = ex.Message + "- 请联系管理员";

//                log.log("DoDeposit Error:" + ex.Message);
//                log.log("DoDeposit StackTrace:" + ex.StackTrace);
//            }
//            finally
//            {
//                tdb.Dispose();
//                udb.Dispose();

//            }
//            return Json(result);
//        }


        [HttpPost]
        public ActionResult DoDeposit()
        {
            WXError error = null;
            ROutPay result = new ROutPay();
            string wxResult;
            UserContent udb = new UserContent();
            TransContent tdb = new TransContent();

            try
            { 
                string openId = this.GetOpenId();
                object oamt = Request["amt"];
                decimal amt;
                if (oamt == null) amt = 0;
                else amt =Convert.ToDecimal(oamt);
                if(amt<=0)
                {
                    result.OutResult = -1;
                    result.ResultRemark = "余额不足不能提款";
                    return Json(result);
                }
                using (TransactionScope ts = new TransactionScope())
                {
                    EMemberInfo mi = udb.GetMemberInfoByOpenId(openId);
                    amt = mi.AvailDeposit;
                    EPPayment epPay = new EPPayment();
                    //对于返回结果还是要判断下
                    wxResult = epPay.PostEPPay(openId, amt);
                    log.log("DoDeposit ResponseXml:" + wxResult);
                    error = epPay.GetResult(wxResult);
                    if (error == null)
                    {
                        //交易记录
                        EAPUserTrans trans = tdb.InitAPTrans(mi, amt);
                        tdb.APTransDbSet.Add(trans);
                        tdb.SaveChanges();

                        //更新会员提款字段信息
                        mi.OutComeAmount();
                        udb.SaveChanges();
                        result.OutResult = 1;
                    }
                    else
                    {
                        result.OutResult = -1;
                        result.ResultRemark = error.errorMsg;
                    }
                    ts.Complete();                
                }               
            }
            catch(Exception ex)
            {
                result.OutResult = -2;
                result.ResultRemark = ex.Message + "- 请联系管理员";

                log.log("DoDeposit Error:" + ex.Message);
                log.log("DoDeposit StackTrace:" + ex.StackTrace);
            }
            finally
            {
                tdb.Dispose();
                udb.Dispose();

            }
            return Json(result);
        }

        public ActionResult OutPay()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostPayUpdateMember(jMemberUpdte data)
        {
            WxPayOrder objResult = null;
            EOrderLine order = null;
            string openId = this.GetOpenId();
            EItemInfo item = EItemInfo.getL1ToL2Item();
            Payment payment = new Payment();

            using (OrderContent db = new OrderContent())
            {
                log.log("PostPayUpdateMember begin CreateOrder");
                order = db.CreateOrderForUpdateMember(data.UserId, openId, item);
                db.SaveChanges();
            }
            objResult = payment.PostPay(this.HttpContext, item, openId);

            return Json(objResult);
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostPay(jsonApplyMember data)
        {
           
            WxPayOrder objResult = null;
            EOrderLine order = null;
            int userId = 0;
            string openId;
            try
            {
           
                if (!VerifyUserCode())
                    return Json(jsonError.GetErrorObj(Errorcode.IncorrectVerifyCode));

                EItemInfo item = null;
                int selTc = data.seltc;
                Session[IQBWXConst.SessionSelTC] = selTc;
                openId = this.GetOpenId();
                Payment payment = new Payment();
                if (selTc == 1)
                    item = EItemInfo.get158Item();
                else
                    item = EItemInfo.get358Item();

                //HasMemberOrCreateUserInfo 中设置UserId
                if (data .UserId== 0)
                {
                    using (UserContent db = new UserContent())
                    {
                        if (string.IsNullOrEmpty(openId))
                            throw new Exception("Open Id 没有获取，不能支付");
                        userId = db.Get(openId).UserId;
                        data.UserId = userId;
                    }
                }
                else
                    userId = data.UserId;

                using (TransactionScope sc = new TransactionScope())
                {
                    using (UserContent udb = new UserContent())
                    {
                        log.log("PostPay begin UpdateForApplyMember");
                        try
                        {
                            string ds = JsonConvert.SerializeObject(data);
                            log.log("PostPay UpdateForApplyMember"+ ds);
                            udb.UpdateForApplyMember(data);
                            udb.SaveChanges();
                        }
                        catch(Exception ex)
                        {
                            log.log("PostPay UpdateForApplyMember Error:" + ex.Message);
                            log.log("PostPay In Ex:" + ex.InnerException);
                        }
                    }
                    using (OrderContent db = new OrderContent())
                    {
                        log.log("PostPay begin CreateOrder");
                        order = db.CreateOrder(userId, openId, item);
                        db.SaveChanges();
                    }
                    sc.Complete();
                }

                //通知上级用户下单成功
                if (order != null)
                {
                    EUserInfo ui = null;
                    using (UserContent db = new UserContent())
                    {
                        ui = db.Get(openId);
                    }
                    if (!string.IsNullOrEmpty(ui.ParentOpenId))
                    {
                        string accessToken = this.getAccessToken(true);
                        log.log("PostPay accessToken" + accessToken);
                        OrderCreationNT notice = new OrderCreationNT(ui, order, accessToken);
                        notice.Push();
                    }
                }
                log.log("PostPay ing...");
                objResult = payment.PostPay(this.HttpContext, item, openId);
            }
            catch(Exception ex)
            {
                
                log.log("PostPay Error:"+ex.Message);
                log.log("PostPay StackTrace:" + ex.StackTrace);
            }

            return Json(objResult); 
        }

      
    }
}