using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.Result;
using IQBCore.Model;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class O2OBaseController : BaseController
    {

        private static List<string> _ReviewOpenIdGroup;

        public static List<string> ReviewOpenIdGroup
        {
            get
            {
                if(_ReviewOpenIdGroup == null)
                {
                    _ReviewOpenIdGroup = null;
                    using (AliPayContent db = new AliPayContent())
                    {
                        _ReviewOpenIdGroup =  db.DBUserInfo.Where(a=>a.UserRole == IQBCore.IQBPay.BaseEnum.UserRole.Administrator)
                            .Select(a => a.OpenId).ToList();

                        if (_ReviewOpenIdGroup == null)
                        {
                            _ReviewOpenIdGroup = new List<string>();
                        }
                            


                    }
                   
                }
                return _ReviewOpenIdGroup;
            }
        }

        public string GetBuyerPhone()
        {
            string buyerPhone = null;
            O2OBuyerSession buyerSession = Session["BuyerSession"] as O2OBuyerSession;
            if (buyerSession == null)
            {
                buyerPhone = CookieHelper.getCookie(IQBConstant.ck_O2OBuyerPhone);
                return buyerPhone;

            }
            else
                return buyerSession.Phone;

        }

        public O2OBuyerSession O2OBuyerSession
        {
            get
            {
                O2OBuyerSession buyerSession = Session["BuyerSession"] as O2OBuyerSession;
                if (buyerSession == null)
                {
                    buyerSession = new O2OBuyerSession();

                    buyerSession.Phone = CookieHelper.getCookie(IQBConstant.ck_O2OBuyerPhone);
                    return buyerSession;

                }
                else
                    return buyerSession;
            }
        }



        public string GetCurrentOrder(string BuyerPhone, AliPayContent db = null)
        {
            var sql = @"select top 1 o.O2ONo from O2OOrder as o
	                where o.UserPhone = '{0}'
	                order by o.CreateDateTime desc";

            sql = string.Format(sql, BuyerPhone);
            string OrderNo = null;
            if (db == null)
            {
                db = new AliPayContent();

            }
            OrderNo = db.Database.SqlQuery<string>(sql).FirstOrDefault();
            db.Dispose();


            return OrderNo;


        }

        public void SetBuyerCookie(EO2OBuyer buyer)
        {
            CookieHelper.setCookie(IQBConstant.ck_O2OBuyerPhone, buyer.Phone);
            CookieHelper.setCookie(IQBConstant.ck_O2OReceiveAccount, buyer.ReceiveAccount);
        }
        public void SetBuyerSession(EO2OBuyer buyer)
        {
            O2OBuyerSession buyerSession = new O2OBuyerSession();
            buyerSession.AliPayAccount = buyer.ReceiveAccount;
            buyerSession.Phone = buyer.Phone;
            buyerSession.BuyerId = buyer.Id;

            Session["BuyerSession"] = buyerSession;
        }
      

        public string CheckaoId()
        {
            string aoId = Request.QueryString["aoId"];
            
            if (string.IsNullOrEmpty(aoId))
                aoId = Request["aoId"];
            if(!string.IsNullOrEmpty(aoId))
                Session[IQBConstant.SK_O2OAgentOpenId] = aoId;

            return aoId;

           
        }

        public void InitO2OPage()
        {
            CheckaoId();
            string Phone = GetBuyerPhone();
            ViewBag.BuyerPhone = Phone;
        }
    }
}