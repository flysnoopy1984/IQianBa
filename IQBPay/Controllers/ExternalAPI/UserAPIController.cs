﻿using IQBPay.Core;
using IQBPay.DataBase;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.User;
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
            EUserInfo updateUser = null;
            EQRInfo qr = null;
            EQRUser qrUser = null;
            try
            {
             
                if (ui != null)
                {
                    using (TransactionScope sc = new TransactionScope())
                    {
                        using (AliPayContent db = new AliPayContent())
                        {
                            //检查用户是否已经注册
                            updateUser = db.DBUserInfo.Where(u => u.OpenId == ui.OpenId).FirstOrDefault();
                            if (updateUser!=null)
                            {
                                //授权
                                if(ui.QRAuthId > 0)
                                {
                                    qr = db.DBQRInfo.Where(a => a.ID == ui.QRAuthId).FirstOrDefault();
                                    if (qr == null)
                                        return "没有找到对应的二维码,请联系平台！";
                                    if (qr.RecordStatus == IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked)
                                        return "授权码已经失效";
                                    if (qr.ReceiveStoreId == 0)
                                        return "授权码错误，没有收款账户，请联系平台！";
                                    
                                   
                                    db.UpdateQRUser(qr, updateUser);
                                }
                                //登陆
                                else
                                {
                                    updateUser.InitModify();
                                    db.SaveChanges();
                                }
                              
                                sc.Complete();
                                return "EXIST";
                            }
                            //通过QR模板获取QRUser
                            if (IQBConfig.NeedDefaultQRModule)
                            {
                                qrUser = new EQRUser();
                                qr = db.DBQRInfo.Where(a => a.Channel == IQBCore.IQBPay.BaseEnum.Channel.PPAuto).FirstOrDefault();
                                if (qr == null)
                                    throw new Exception("没有找到对应的二维码,请联系平台！");
                                qrUser.UserName = ui.Name;
                                qrUser.QRId = qr.ID;
                                qrUser.OpenId = ui.OpenId;
                                qrUser.Rate = qr.Rate;
                                db.DBQRUser.Add(qrUser);
                                db.SaveChanges();
                                qrUser = QRManager.CreateUserUrlById(qrUser);
                                ui.QRUserDefaultId = qrUser.ID;
                            }
                            ui.UserRole = IQBCore.IQBPay.BaseEnum.UserRole.NormalUser;
                            ui.UserStatus = IQBCore.IQBPay.BaseEnum.UserStatus.JustRegister;

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
