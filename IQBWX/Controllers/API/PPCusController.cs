using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.User;
using IQBWX.DataBase.IQBPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;

        }
    }
}
