using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
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
        public OutAPI_QRHuge CreateQRHuge([FromBody]EQRHuge qrHuge)
        {
            OutAPI_QRHuge result = new OutAPI_QRHuge();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    qrHuge = QRManager.CreateQRHuge(qrHuge);

                

                    DbEntityEntry<EQRHuge> entry = db.Entry<EQRHuge>(qrHuge);
                    entry.State = System.Data.Entity.EntityState.Unchanged;
                    entry.Property(t => t.FilePath).IsModified = true;
                    entry.Property(t => t.QRUrl).IsModified = true;
                    db.SaveChanges();

                    result.EQRHuge = qrHuge;
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
