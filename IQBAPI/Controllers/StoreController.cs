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
    public class StoreController : BaseAPIController
    {
        [HttpPost]
        public NResult<EStoreInfo> CreateOrUpdate(EStoreInfo obj)
        {
            NResult<EStoreInfo> result = new NResult<EStoreInfo>();
            try
            {
                using (OOContent db = new OOContent())
                {
                    EStoreInfo updateObj = null;
                    if (obj.Id != 0)
                    {
                        updateObj = db.DBStoreInfo.Where(a => a.Id == obj.Id).FirstOrDefault();
                    }

                    //新增
                    if (updateObj == null)
                    {
                        db.DBStoreInfo.Add(obj);
                        db.SaveChanges();
                    }
                    //修改
                    else
                    {
                      
                        updateObj.Name = obj.Name;
                        updateObj.Description = obj.Description;
                        updateObj.UserId = obj.UserId;
                        updateObj.RecordStatus = obj.RecordStatus;

                        db.SaveChanges();
                    }
                    result.resultObj = obj;

                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
                ErrorToDb(ex.Message);
            }
            return result;
        }

    }
}
