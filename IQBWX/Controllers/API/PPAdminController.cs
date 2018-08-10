using IQBCore.IQBPay.Models.OutParameter;
using IQBWX.DataBase.IQBPay;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBWX.Controllers.API
{
    public class PPAdminController : ApiController
    {
        /// <summary>
        /// 用户支付后，需要等待审核后付款
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        [HttpPost]
        public OutAPIResult AgreePay(string OrderNo)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                if(!string.IsNullOrEmpty(OrderNo))
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        string sql = @"update OrderInfo
					                    set OrderStatus = 1
					                    where OrderNo = @OrderNo";
                        db.Database.ExecuteSqlCommand(sql, new SqlParameter("@OrderNo", OrderNo));


                    }
                }
              
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
    }
}
