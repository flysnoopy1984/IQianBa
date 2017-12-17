using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Store;
using IQBPay.Core;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace IQBPay.Controllers.ExternalAPI
{
    public class StoreAPIController : ApiController
    {
        
        public OutAPIResult JoinPT([FromBody]EStoreInfo si)
        {
            OutAPIResult result = new OutAPIResult();
            result.IsSuccess = true;
         
            try
            {
               
                si.Channel = IQBCore.IQBPay.BaseEnum.Channel.League;
                si.InitCreate();
                si.InitModify();
                si.IsReceiveAccount = false;
                si.FromIQBAPP = BaseController.App.ID.ToString();
                si.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked;
                si.StoreAuthStatus = IQBCore.IQBPay.BaseEnum.StoreAuthStatus.NoAuth;

                EQRInfo qr = new EQRInfo();
                qr.InitByStore(si);
                using (TransactionScope sc = new TransactionScope())
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        
                      
                        db.DBQRInfo.Add(qr);
                        db.SaveChanges();

                        qr = QRManager.CreateStoreAuthUrlById(qr);
                        db.Entry(qr).State = System.Data.Entity.EntityState.Modified;

                        si.QRId = qr.ID;
                        db.DBStoreInfo.Add(si);
                        db.SaveChanges();

                    }
                    sc.Complete();
                }

               
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;

                IQBLog _log = new IQBLog();
                _log.log("Register Error " + ex.Message);
            }
           
           return result;
        }
    }
}
