using IQBPay.Core;
using IQBPay.DataBase;
using IQBPay.Models.QR;
using IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace IQBPay.Controllers.ExternalAPI
{
    public class UserAPIController : ApiController
    {
        [HttpPost]
        public string Register([FromBody]EUserInfo ui)
        {
           
            try
            {
                if (ui != null)
                {
                    using (TransactionScope sc = new TransactionScope())
                    {
                        using (AliPayContent db = new AliPayContent())
                        {
                            if (db.IsExistUser(ui.OpenId))
                            {
                                return "EXIST";
                            }

                            EQRUser qrUser = new EQRUser();

                            EQRInfo qr = db.DBQRInfo.Where(a => a.Channel == Core.BaseEnum.QRChannel.PPAuto).FirstOrDefault();
                            if (qr == null)
                            {
                                throw new Exception("没有配置默认QR,请联系站长！");
                            }
                            
                            qrUser.QRId = qr.ID;

                            qrUser.OpenId = ui.OpenId;
                            qrUser.Rate = qr.Rate;
                            qrUser = QRManager.CreateUserUrlById(qrUser);
                        

                            db.DBQRUser.Add(qrUser);
                            db.SaveChanges();

                            ui.QRDefaultId = qrUser.ID;
                            ui.UserRole = Core.BaseEnum.UserRole.NormalUser;
                            ui.UserStatus = Core.BaseEnum.UserStatus.Register;
                            

                            db.DBUserInfo.Add(ui);
                            db.SaveChanges();

                        }
                       
                        sc.Complete();
                    }
                }
                else
                    return "参数传入失败！";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            return "OK";
        }
    }
}
