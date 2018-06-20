using IQBCore.DataBase;
using IQBCore.Model;
using IQBCore.OO.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBAPI.Controllers
{
    public class OrderController : BaseAPIController
    {
        [HttpPost]
        public NResult<EOrderInfo> GetLatestOrder(long UserId)
        {
            NResult<EOrderInfo> result = new NResult<EOrderInfo>();
            try
            {
                using (OOContent db = new OOContent())
                {
                    var list = db.DBOrderInfo.Where(a => a.BuyerUserId == UserId).OrderByDescending(a => a.CreatedTime).Take(10);

                    result.resultList = list.ToList();

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            if (result == null)
                result = new NResult<EOrderInfo>();

            return result;
        }
    }
}
