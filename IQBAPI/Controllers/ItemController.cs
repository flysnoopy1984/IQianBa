using IQBCore.DataBase;
using IQBCore.Model;
using IQBCore.OO.BaseEnum;
using IQBCore.OO.Models.Entity;
using IQBCore.OO.Models.In;
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
    }
}
