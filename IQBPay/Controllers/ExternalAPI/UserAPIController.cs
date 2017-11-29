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
using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using System.Text;

namespace IQBPay.Controllers.ExternalAPI
{
    public class UserAPIController : ApiController
    {
        public HttpResponseMessage FormatReturn(string result)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage { Content = new StringContent(result, Encoding.GetEncoding("UTF-8"), "text/plain") };
  
            return responseMessage;
        }
        [HttpPost]
        public HttpResponseMessage  Register([FromBody]EUserInfo ui)
        {
            EUserInfo updateUser = null;
            EQRInfo qr = null;
            EQRUser qrUser = null;
            Boolean isExist=true;
            bool hasParent = false;
          
            try
            {
             
                if (ui != null)
                {
                    //检查是否授权
                    if (ui.QRAuthId > 0)
                    {
                        using (TransactionScope sc = new TransactionScope())
                        {
                            using (AliPayContent db = new AliPayContent())
                            {
                                try
                                {
                                    //检查用户是否已经注册
                                    updateUser = db.DBUserInfo.Where(u => u.OpenId == ui.OpenId).FirstOrDefault();
                                    //没有注册
                                    if (updateUser == null)
                                    {
                                        //新建代理用户
                                        updateUser = new EUserInfo();
                                        updateUser.InitRegiser();
                                        updateUser.OpenId = ui.OpenId;
                                        updateUser.Name = ui.Name;
                                        updateUser.Headimgurl = ui.Headimgurl;
                                        
                                        updateUser.UserRole = IQBCore.IQBPay.BaseEnum.UserRole.Agent;
                                        db.DBUserInfo.Add(updateUser);
                                        isExist = false;
                                       
                                    }
                                    qr = db.DBQRInfo.Where(a => a.ID == ui.QRAuthId).FirstOrDefault();
                                    qrUser = db.UpdateQRUser(qr, updateUser);
                                    if(qrUser == null)
                                    {
                                        return FormatReturn("授权码失效，无法给用户授权,请联系平台！");
                                    }
                                    qr.RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Blocked;
                                   
                                    updateUser.QRAuthId = 0;
                                    updateUser.QRUserDefaultId = qrUser.ID;

                                    //用户返回后，给微信提示做判断
                                    hasParent = !string.IsNullOrEmpty(qrUser.ParentOpenId);
                                      
                                    db.SaveChanges();
                                }
                                catch(Exception ex)
                                {
                                    IQBLog _log = new IQBLog();
                                    _log.log("Register Error "+ex.Message);
                                    return FormatReturn("授权出现错误,请联系平台！");
                                }

                            }
                            sc.Complete();
                        }
                    }
                    else
                    {
                        try
                        {
                            using (AliPayContent db = new AliPayContent())
                            {
                                updateUser = db.DBUserInfo.Where(u => u.OpenId == ui.OpenId).FirstOrDefault();
                                //新用户注册
                                if (updateUser == null)
                                {
                                    if (BaseController.GlobalConfig.AllowRegister)
                                    {

                                        ui.InitRegiser();
                                      
                                        db.DBUserInfo.Add(ui);
                                        db.SaveChanges();
                                        isExist = false;
                                    }
                                    else
                                        return FormatReturn("网站已经关闭，请联系管理员");
                                }
                                //用户登录
                                else
                                {
                                    updateUser.InitModify();
                                    db.SaveChanges();

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            IQBLog _log = new IQBLog();
                            _log.log("Register Error " + ex.Message);
                            return FormatReturn("登陆错误,请联系平台！");
                        }
                       
                           
                    }
/********************************************************************************************************************************************/
                  
                }
                else
                    return FormatReturn("参数传入失败！");
            }
            catch(Exception ex)
            {
                return FormatReturn(ex.Message);
            }

            if (!isExist)
            {
                if (hasParent) return FormatReturn("ParentOK");
                return FormatReturn("OK");
            }
            else
            {
                if (hasParent) return FormatReturn("ParentEXIST");
                return FormatReturn("EXIST");
            
            }
        }
    }
}
/*
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
                            if (IQBConstant.NeedDefaultQRModule)
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
     
     */
