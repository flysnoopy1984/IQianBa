using IQBCore.Common.Helper;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBWX.Const;
using IQBWX.BLL;
using IQBWX.BLL.NT;
using IQBWX.DataBase;
using IQBWX.Models.JsonData;
using IQBWX.Models.Order;
using IQBWX.Models.Product;
using IQBWX.Models.Results;
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
    public class Html5Controller : WXBaseController
    {
        IQBLog log = new IQBLog();
        /// <summary>
        /// -1 失败，1 成功
        /// </summary>
        /// <returns></returns>
        private int UpdateMemeberLevel2()
        {
         

            try
            { 
                string msg = Request.QueryString["msg"];
                ViewData.Add("imgTitle", "大区经理");
                ViewData.Add("Title", "恭喜你荣升为");
                ViewData.Add("Content", "大区经理");
               
                if (msg != "get_brand_wcpay_request:ok")         
                    return -1;
                else
                {
                    EOrderLine order = null;
                    EMemberInfo mi = null;
                    using (TransactionScope sc = new TransactionScope())
                    {
                        using (UserContent db = new UserContent())
                        {
                            string openId = this.GetOpenId();
                            db.UpdateToMemberLevel2(openId);
                            mi = db.GetMemberInfoByOpenId(openId);
                            db.SaveChanges();
                        }
                        using (OrderContent odb = new OrderContent())
                        {
                         
                            order = odb.UpdateToPaidOrder(mi);
                           
                        }
                        sc.Complete();
                    }
                }
            }
            catch(Exception ex)
            {
                log.log("UpdateMemeberLevel2 Error"+ex.Message);
                log.log("UpdateMemeberLevel2 StackTrace" + ex.StackTrace);
            }
            return 1;
        }
            
        /// <summary>
        /// -1 失败，1 成功
        /// </summary>
        /// <returns></returns>
        private int FirstApplySuccess()
        {
  
            var imgTitle = "城市经理";
            var Title = "恭喜你成为";
            var Content = "城市经理";
            try
            {
                string msg = Request.QueryString["msg"];
                log.log("ApplySuccess msg:" + msg);
                string selTc = Convert.ToString(Session[IQBWXConst.SessionSelTC]);
                log.log("ApplySuccess selTc:" + selTc);
                if (selTc == null || selTc == "")
                    return -1;
                if (selTc == "2")
                {
                    imgTitle = "大区经理";
                    Content = imgTitle;
                }
                ViewData.Add("imgTitle", imgTitle);
                ViewData.Add("Title", Title);
                ViewData.Add("Content", Content);

                if (msg != "get_brand_wcpay_request:ok")
                {
                    return -1;
                }
                else
                {
                    MemberNew();
                }
            }
            catch (Exception ex)
            {
                log.log("ApplySuccess Error:" + ex.Message);
                log.log("ApplySuccess StackTrace:" + ex.StackTrace);
                return -1;
            }
            finally
            {
                Session[IQBWXConst.SessionSelTC] = null;
            }
            return 1;
        }

        /// <summary>
        /// result 0：购买或升级成功。1：首次购买后出错，停留在会员申请界面。2：更新会员后出错停留在更新界面
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplySuccess()
        {
        
            //1. 购买会员。2 update member. 3. pass by js file
            int applyType = Convert.ToInt32(Request.QueryString["type"]);
            log.log("ApplySuccess applyType:" + applyType);
            int result = -1;
            if (applyType == 1)
            {                 
                result = FirstApplySuccess();
                if (result == -1)          
                    return RedirectToAction("ApplyMember");
             
            }
            if (applyType == 2)
            {               
                result = UpdateMemeberLevel2();
                if (result == -1)
                    return RedirectToAction("MemberUpdate");                
            }

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        private void MemberNew()
        {
      
            try {
                string openId = this.GetOpenId();
                int selTc = Convert.ToInt32(Session[IQBWXConst.SessionSelTC]);
                string accessToken = this.getAccessToken(true);
                EMemberInfo mi = null;
                EOrderLine order = null;
                Dictionary<int, EMemberInfo> relations = null;
                int count = 0;

                using (TransactionScope sc = new TransactionScope())
                {
                    using (UserContent udb = new UserContent())
                    {
                        count = udb.MemberInfo.Count();
                        //当第一个会员时，总计为0。
                        count++;
                        //1. 新增会员
                        mi = udb.UpdateToMember(openId, selTc, count.ToString());
                        log.log("MemberNew UpdateToMember:"+mi.openId);
                        //2. .建立父子关系
                        relations = new Dictionary<int, EMemberInfo>();
                        int level = 1;
                        udb.BuildMemberRelation(mi, mi, ref relations, level);
                        udb.SaveChanges();
                    }
                    //3. 更新支付订单状态
                    using (OrderContent odb = new OrderContent())
                    {
                        order = odb.UpdateToPaidOrder(mi);
                    }
                             
                    EMemberInfo pmi = null;
                    if (relations != null && relations.Count()>0) pmi = relations[1];
                    if(pmi!=null)
                    {
                        //4.支付成功消息通知上级
                        OrderPaidSuccessNT notice = new OrderPaidSuccessNT(accessToken, order, mi,pmi);
                        notice.Push();

                        //5.佣金策略执行
                        PopPolicy policy = new PopPolicy(order);
                        policy.RunCommession(mi,relations);
                    }

                    sc.Complete();
                }

                if (mi != null)
                {
                    this.CreateQR(mi, count.ToString());
                }
            }
            catch(Exception ex)
            {
                log.log("MemberNew Error:" + ex.Message);
                log.log("MemberNew StackTrace:" + ex.StackTrace);
            }
        }
     
        public ActionResult ApplyMember()
        {
            WXParameter WXParameter = this.GetUserInfoAccessCode();
            string openId = WXParameter.OpenId;
            string access_token = WXParameter.AccessToke;
            IQBLog log = new IQBLog();

            openId = "o3nwE0qI_cOkirmh_qbGGG-5G6B0";
            try
            {
           
           /*     if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(access_token))
                {
                    string url_userInfo = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", access_token, openId);
                    WXUserInfo wxUser = HttpHelper.Get<WXUserInfo>(url_userInfo,true);

                    log.log("ApplyMember openId:" + wxUser.openid);
                    int userType = HasMemberOrCreateUserInfo(wxUser);
                    log.log("ApplyMember userType:" + userType);
                    if (userType == 1)
                        return RedirectToAction("MemberUpdate");
                    else if (userType == 2)
                        return RedirectToAction("MemberCenter");
                    else
                        return View();
                    
                }
             */ 
            }
            catch (Exception ex)
            {
                log.log("ApplyMember Error:" + ex.Message);
                log.log("ApplyMember Inner Error:" + ex.InnerException.Message);
            }

            return View();
        }

        /// <summary>
        /// 0代表不是Member,1 代表普通会，2代表高级会员
        /// </summary>
        /// <param name="wxUser"></param>
        /// <returns></returns>
        private int HasMemberOrCreateUserInfo(WXUserInfo wxUser)
        {
           
            try
            {
                using (UserContent db = new UserContent())
                {
                    EUserInfo ui = db.Get(wxUser.openid);

                    //如果用户从来没有被记录过
                    if (ui == null)
                    {
                        ui = new EUserInfo();
                        ui.SetWXUserInfo(wxUser);

                        //  ui.PaymentState = PaymentState.Paying;
                        log.log("HasMemberOrCreateUserInfo Insert");
                        db.InsertUserInfo(ui);
                    }
                    else
                    {
                        Session[IQBWXConst.SessionUserId] = ui.UserId;
                        log.log("HasMemberOrCreateUserInfo UI.UserId:"+ui.UserId);
                        if (ui.IsMember)
                        {
                            log.log("HasMemberOrCreateUserInfo UI.openid:" + ui.openid);
                            MemberType mt = db.GetMemberType(ui.openid);
                            if (mt == MemberType.Channel)
                                return 1;
                            else
                                return 2;
                        }
                        else
                            return 0;
                     
                    }
                    Session[IQBWXConst.SessionUserId] = ui.UserId;
                    log.log("HasMemberOrCreateUserInfo UserId:" + ui.UserId);
                }
                return 0;
            }
            catch (Exception ex)
            {
                log.log("HasMemberOrCreateUserInfo Error:" + ex.Message);
                log.log("HasMemberOrCreateUserInfo StackTrace:" + ex.StackTrace);
                return 0;
            }
        }



        public ActionResult Test()
        {
            try
            {
                MenuEvents menuEvent = new MenuEvents();
                WXMessage wxMsg = new WXMessage();
                string xml = @"<xml><ToUserName><![CDATA[gh_3364510c3e68]]></ToUserName>
<FromUserName><![CDATA[orKUAw16WK0BmflDLiBYsR-Kh5bE]]></FromUserName>
<CreateTime>1502173030</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[SCAN]]></Event>
<EventKey><![CDATA[Jqg20170808021703]]></EventKey>
<Ticket><![CDATA[gQEC8DwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAySWZBY0U1SUJjMDMxQ3JuOWhwY20AAgRfV4lZAwQ8AAAA]]></Ticket>
</xml>";
                wxMsg.LoadXml(xml);
                menuEvent.WXScanLogin(wxMsg, this);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }

   
        public void CreateQR(EMemberInfo mi,string scene_id)
        {
            
            WXController wx = new WXController();
            AccessToken token = wx.getToken();
            WXQRResult resObj = wx.getQR(scene_id, token.access_token);
            resObj.ticket = Uri.EscapeDataString(resObj.ticket);
            string fileName = mi.UserId+".jpg";
            string addr = wx.downloadQR(resObj, fileName);
        }

        public ActionResult MemberQR()
        {
            RMemberQR qr = null;
            try
            {
                string openId = this.GetOpenId();
                log.log("MemberQR openId:" + openId);
                if (!string.IsNullOrEmpty(openId))
                {
                    using (UserContent db = new UserContent())
                    {
                        qr = db.GetMemberQRData(openId);
                        qr.BKImg = ImgHelper.BKPath + qr.UserId + ".jpg";
                    }
                }
             
              
                //EUserInfo ui = null;
                //using (UserContent db = new UserContent())
                //{
                //    ui = db.Get(openId);
                //}
                //string fileName = ui.UserId + ".jpg";
                //ViewData["imgSrc"] = ImgHelper.BKPath + fileName;

                    ////   openId = "222"; //test
                    //if (string.IsNullOrEmpty(openId))
                    //{
                    //    JsApiPay jsApiPay = new JsApiPay(this.HttpContext);
                    //    jsApiPay.GetOpenidAndAccessToken();

                    //    Session[IQBConst.SessionOpenId] = jsApiPay.openid;
                    //    openId = (string)Session[IQBConst.SessionOpenId];
                    //    if (!string.IsNullOrEmpty(openId))
                    //    {
                    //        UserContent db = new UserContent();
                    //        EUserInfo ui = db.Get(openId);

                    //        string fileName = ui.UserId + ".jpg";
                    //        //   fileName = "test.jpg";//test
                    //        ViewData["imgSrc"] = ImgHelper.BKPath + fileName;
                    //    }
                    //}
                    //else
                    //{
                    //    EUserInfo ui = null;
                    //    using (UserContent db = new UserContent())
                    //    { 
                    //       ui= db.Get(openId);
                    //    }
                    //    string fileName = ui.UserId + ".jpg";
                    //    //   fileName = "test.jpg";//test
                    //    ViewData["imgSrc"] = ImgHelper.BKPath + fileName;
                    //}
            }
            catch (Exception ex)
            {
            
                log.log("MemberQR Error:" + ex.Message);
                log.log("MemberQR InnerError:" + ex.InnerException.Message);
            }
            finally
            {
                if(qr == null)
                {
                    qr = new RMemberQR();
                }
            }
            return View(qr);
        }

        public ActionResult MemberInfo()
        {
            string openId = this.GetOpenId();
            EMemberInfo memberInfo = null;
            if (!string.IsNullOrEmpty(openId))
            {           
                using (UserContent db = new UserContent())
                {
                    memberInfo = db.GetMemberInfoByOpenId(openId);              
                }
            }
            if (memberInfo == null)
                memberInfo = new EMemberInfo();

            return View(memberInfo);
        }

        public ActionResult MemberCenter()
        {
            if (base.CheckIsMember())
            {
                DataMemberCenter data = new DataMemberCenter();
                EMemberInfo mi = null;
                string openId = this.GetOpenId();
                using (UserContent udb = new UserContent())
                {
                    mi = udb.GetMemberInfoByOpenId(openId);
                    data.ChildrenNum = udb.GetChildCount(openId);
                }
                data.HeaderImg = mi.headimgurl;
                data.MemberType = mi.MemberType;
                data.MemberTypeValue = IQBWXConst.GetMemberTypeValue(mi.MemberType);
                data.TotalIncome = mi.TotalIncome;
                data.Balance = mi.Balance;
                data.NickName = mi.nickname;


                return View(data);
            }
            else
                return RedirectToRoute(new { Controller = "Home", action = "ErrorMessage", code = Errorcode.NotMember });

        }

        public ActionResult CommissionDetail()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult CommissionDetailData()
        {
            string openId = this.GetOpenId();
            List<RARTrans> data = null;
            int pageIndex = Convert.ToInt32(Request["Page"]);
            using (TransContent db = new TransContent())
            {
                data = db.ARPagination(pageIndex, openId);
                //Session["CommissionPageIndex"] = pageIndex;
            }
            return Json(data);           
        }

        [HttpPost]
        public ActionResult MyChildrenCount()
        {
            List<int> list;
            string sql = @"select COUNT(1) from [dbo].[MemberChildren] where ChildLevel =1 and pOpenId='{0}'
	union all
select COUNT(1) from [dbo].[MemberChildren] where ChildLevel =2 and pOpenId='{0}'
	union all
select COUNT(1) from [dbo].[MemberChildren] where ChildLevel =3 and pOpenId='{0}'
	union all
select COUNT(1) from [dbo].[MemberChildren] where ChildLevel =4 and pOpenId='{0}'";
            sql = string.Format(sql, this.GetOpenId());

            using (UserContent db = new UserContent())
            {
                 list= db.Database.SqlQuery<int>(sql).ToList();
            }

            return Json(list);
        }

        [HttpPost]
        public ActionResult MyChildrenDetail()
        {
            int level = Convert.ToInt32(Request["cLevel"]);
            int pageIndex = Convert.ToInt32(Request["Page"]);

            string openId = this.GetOpenId();
            List<RMyChildren> list = null;
            using (UserContent db = new UserContent())
            {
                list = db.GetMyChildrenData(openId, level,pageIndex);
            }
            return Json(list);
        }

        public ActionResult MyChildren()
        {
            string openId = this.GetOpenId();
            EMemberInfo memberInfo = null;
            log.log("MyChildren openId:"+openId);
            if (!string.IsNullOrEmpty(openId))
            {
                using (UserContent db = new UserContent())
                {
                    memberInfo = db.GetMemberInfoByOpenId(openId);
                }
            }
            if (memberInfo == null)
                memberInfo = new EMemberInfo();
            log.log("MyChildren memberInfo Id:" + memberInfo.MemberId);
            return View(memberInfo);
        }

        public ActionResult MemberUpdate()
        {
            EMemberInfo mi = null;
            using (UserContent db = new UserContent())
            {
                string openId = this.GetOpenId();
                if(!string.IsNullOrEmpty(openId))
                {
                    mi = db.GetMemberInfoByOpenId(openId);
                    if(mi.MemberType == MemberType.City)
                    {
                        return RedirectToAction("MemberCenter");
                    }

                }
            }
            if (mi == null) mi = new EMemberInfo();

            return View(mi);
        }

        public ActionResult LoanMarket()
        {
            if(base.CheckIsMember())
            {
                return View();
            }
            else
                return RedirectToRoute(new { Controller = "Home", action = "ErrorMessage", code = Errorcode.NotMember });
        }

        public ActionResult CashOut()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CashOutData()
        {
            string openId = this.GetOpenId();
            List<RAPTrans> data = null;
            int pageIndex = Convert.ToInt32(Request["Page"]);
            using (TransContent db = new TransContent())
            {
                data = db.APPagination(pageIndex, openId);
                Session["CommissionPageIndex"] = pageIndex;
            }
            return Json(data);
        }


        public ActionResult ErrorMessage()
        {
            return View();
        }
    }
}