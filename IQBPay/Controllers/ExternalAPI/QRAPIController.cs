using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using IQBCore.Model;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBPay.Controllers.ExternalAPI
{
    public class QRAPIController : ApiController
    {
        public OutAPI_QRHuge CreateQRHuge([FromBody]RQRHuge qrHuge)
        {
            OutAPI_QRHuge result = new OutAPI_QRHuge();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    EQRHuge obj = new EQRHuge();
                    obj.ID = qrHuge.ID;
                    obj.Amount = qrHuge.Amount;
                    obj = QRManager.CreateQRHuge(obj);

                    DbEntityEntry<EQRHuge> entry = db.Entry<EQRHuge>(obj);
                    entry.State = System.Data.Entity.EntityState.Unchanged;
                    entry.Property(t => t.FilePath).IsModified = true;
                    entry.Property(t => t.QRUrl).IsModified = true;
                    db.SaveChanges();

                    qrHuge.FilePath = obj.FilePath;
                    qrHuge.QRUrl = obj.QRUrl;
                    result.RQRHuge = qrHuge;
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
        public NResult<EQRStoreAuth> CreateQRStoreAuth(EQRStoreAuth qr)
        {
            NResult<EQRStoreAuth> result = new NResult<EQRStoreAuth>();
            try
            {
                qr = QRManager.CreateStoreAuthUrlById(qr);
                result.resultObj = qr;
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
