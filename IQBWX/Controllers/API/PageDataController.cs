using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.Page;
using IQBCore.IQBPay.Models.QR;
using IQBCore.Model;
using IQBWX.DataBase.IQBPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBWX.Controllers.API
{
    public class PageDataController : ApiController
    {
        #region PaySelection
        [HttpGet]
        public NResult<PPaySelection> InitPaySelection(string openId)
        {
            NResult<PPaySelection> result = new NResult<PPaySelection>();
            try
            {
                string sql = string.Format("select Id as QRUserId,QRType as QRType from QRUser where (QRType={1} or QRType ={2}) and OpenId= '{0}'",
                    openId, (int)QRReceiveType.Small, (int)QRReceiveType.CreditCard);

                using (AliPayContent db = new AliPayContent())
                {
                    result.resultList = db.Database.SqlQuery<PPaySelection>(sql).ToList();
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
        #endregion
    }
}
