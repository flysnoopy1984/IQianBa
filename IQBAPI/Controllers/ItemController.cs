using IQBCore.DataBase;
using IQBCore.Model;
using IQBCore.OO.BaseEnum;
using IQBCore.OO.Models.Entity;
using IQBCore.OO.Models.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBAPI.Controllers
{
    public class ItemController : BaseAPIController
    {
        [HttpPost]
        public NResult<EItemInfo> GetAvaliableItem(QItem qItem)
        {
            NResult<EItemInfo> result = new NResult<EItemInfo>();
            try
            {
                using (OOContent db = new OOContent())
                {
                    var list = db.DBItemInfo.Where(a => a.RecordStatus == RecordStatus.Normal).OrderByDescending(a => a.CreatedTime);

                    if (qItem.pageIndex == 0)
                    {
                        result.resultList = list.Take(qItem.pageSize).ToList();
                    }
                    else
                    {
                        result.resultList = list.Skip(qItem.pageIndex * qItem.pageSize).Take(qItem.pageSize).ToList();
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

        [HttpPost]
        public NResult<EItemInfo> CreateOrUpdate(EItemInfo obj)
        {
            NResult<EItemInfo> result = new NResult<EItemInfo>();
            try
            {
                using (OOContent db = new OOContent())
                {
                    EItemInfo updateObj = null;
                    if (obj.Id != 0)
                    {
                        updateObj = db.DBItemInfo.Where(a => a.Id == obj.Id).FirstOrDefault();
                    }

                    //新增
                    if (updateObj == null)
                    {
                        db.DBItemInfo.Add(obj);
                        db.SaveChanges();
                    }
                    //修改
                    else
                    {
                        updateObj.Channel = obj.Channel;
                        updateObj.Name = obj.Name;
                        updateObj.Price = obj.Price;
                        updateObj.RealUrl = obj.RealUrl;
                        updateObj.StoreId = obj.StoreId;
                      
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
