using IQBCore.Common.Constant;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.User;
using IQBCore.Model;
using IQBWX.DataBase.IQBPay;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IQBWX.Controllers.API
{
    public class PPCusController : ApiController
    {
        [HttpGet]
        public OutAPIResult CheckPhone(string phone)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                string sql = string.Format("select count(1) from BuyerInfo where PhoneNumber='{0}'", phone);

                using (AliPayContent db = new AliPayContent())
                {
                    int r = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                    if(r>0)
                    {
                        HttpContext.Current.Session[IQBConstant.BuyerPhone] = phone;
                    }
                    result.IntMsg = r;
                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
         
        }
        [HttpPost]
        public OutAPIResult AddBuyerPhone(string phone)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                EBuyerInfo buyer = new EBuyerInfo
                {
                    BuyerType = IQBCore.IQBPay.BaseEnum.BuyerType.QR,
                    PhoneNumber = phone,
                    LastTransDate = DateTime.Now,
                    TransDate = DateTime.Now,
                };
                using (AliPayContent db = new AliPayContent())
                {
                    db.DBBuyerInfo.Add(buyer);
                    db.SaveChanges();
                }
                HttpContext.Current.Session[IQBConstant.BuyerPhone] = phone;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;

        }
      
        [HttpPost]
        public NResult<EBuyerInfo> GetBuyerInfo(string phone)
        {
            NResult<EBuyerInfo> result = new NResult<EBuyerInfo>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    result.resultObj = db.DBBuyerInfo.Where(a => a.PhoneNumber == phone).FirstOrDefault();
                    if(result.resultObj == null)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "没有找到对应的手机号";
                        return result;
                    }
                    else
                        HttpContext.Current.Session[IQBConstant.BuyerPhone] = phone;

                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public OutAPIResult SetBuyerAliAccount(string phone,string AliAccount)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    SqlParameter[] ps = new SqlParameter[2];
                    ps[0] = new SqlParameter("@Phone", phone);
                    ps[1] = new SqlParameter("@Account", AliAccount);
                    string sql = @"update BuyerInfo 
                    set AliPayAccount = @Account
                    where PhoneNumber = @Phone";
                    db.Database.ExecuteSqlCommand(sql, ps);

                    HttpContext.Current.Session[IQBConstant.BuyerAliAccount] = AliAccount;

                    return result;

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 根据客户手机号，查询对应的加以记录
        /// </summary>
        /// <param name="BuyerPhone"></param>
        /// <returns></returns>
        [HttpPost]
       public NResult<RBuyerTrans> QueryBuyerTrans(InBuyerTrans InBuyerTrans)
       {
            //if(string.IsNullOrEmpty(InBuyerTrans.BuyerAliAccount))
            //{
            //    InBuyerTrans.BuyerAliAccount = this.GetBuyerInfo(InBuyerTrans.BuyerPhone).resultObj.AliPayAccount;
            //}

            NResult<RBuyerTrans> result = new NResult<RBuyerTrans>();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = @"select o.OrderNo,o.BuyerMobilePhone,
                    o.TransDateStr,
                    o.BuyerTransferAmount as Amount,
                    o.OrderStatus
                    from OrderInfo as o 
                    where o.OrderType = 10 and o.OrderStatus in (1,10,2) and o.BuyerMobilePhone = @Phone
                    order by o.OrderStatus,o.TransDate desc";
                    var list = db.Database.SqlQuery<RBuyerTrans>(sql,new SqlParameter("@Phone", InBuyerTrans.BuyerPhone));

                    if (InBuyerTrans.pageIndex == 0)
                        result.resultList = list.Take(InBuyerTrans.pageSize).ToList();
                    else
                        result.resultList = list.Skip(InBuyerTrans.pageIndex * InBuyerTrans.pageSize).Take(InBuyerTrans.pageSize).ToList();
                }
                  
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return result;
       }
    }
}
