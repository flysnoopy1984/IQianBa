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
    public class BaseAPIController : ApiController
    {
        protected NResult<T> GetLatestData<T>(int pageSize) where T : EBaseRecord
        {
            NResult<T> result = new NResult<T>();
            try
            {
                using (OOTContent<T> db = new OOTContent<T>())
                {
                    var list = db.Db.OrderByDescending(a => a.CreatedTime).Take(pageSize);
                  
                    result.resultList = list.ToList();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }

            if (result == null)
                result = new NResult<T>();

            return result;
        }


    }
}
