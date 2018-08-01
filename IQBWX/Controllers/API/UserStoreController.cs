using IQBCore.Common.Helper;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.User;
using IQBCore.Model;
using IQBWX.DataBase.IQBPay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBWX.Controllers.API
{
    public class UserStoreController : ApiController
    {
        [HttpPost]
        public OutAPIResult Save(EStoreInfo inObj)
        {
            OutAPIResult result = new OutAPIResult();
            EStoreInfo newObj = null;
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    if(inObj.ID>0)
                    {
                        EStoreInfo updateObj = db.DBStoreInfo.Where(a => a.ID == inObj.ID).FirstOrDefault();
                        if(updateObj == null)
                        {
                            result.ErrorMsg = "没有获取当前商户，请联系平台！";
                            result.IsSuccess = false;
                            return result;
                        }
                        updateObj.MinLimitAmount = inObj.MinLimitAmount;
                        updateObj.MaxLimitAmount = inObj.MaxLimitAmount;
                        updateObj.DayIncome = inObj.DayIncome;
                        updateObj.RemainAmount = inObj.DayIncome;
                        updateObj.Name = inObj.Name;
                        updateObj.StoreType = inObj.StoreType;

                        EQRStoreAuth qrAuth = db.DBQRStoreAuth.Where(a => a.StoreId == updateObj.ID).FirstOrDefault();
                        if(qrAuth!=null)
                        {
                            qrAuth.MaxLimitAmount = inObj.MaxLimitAmount;
                            qrAuth.MinLimitAmount = inObj.MinLimitAmount;
                            qrAuth.DayIncome = inObj.DayIncome;
                            qrAuth.RemainAmount = inObj.DayIncome;
                            qrAuth.StoreName = inObj.Name;
                            qrAuth.StoreType = inObj.StoreType;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        EUserStore us = db.DBUserStore.Where(a => a.OpenId == inObj.OwnnerOpenId).FirstOrDefault();
                        if (us == null)
                        {
                            result.ErrorMsg = "身份未获取或没有权限创建商户！请联系平台";
                            result.IsSuccess = false;
                            return result;
                        }
                        string sql = @"select AppId from AliPayApplication where IsCurrent = 1";
                        string appId = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                        newObj = new EStoreInfo
                        {
                            MaxLimitAmount = inObj.MaxLimitAmount,
                            MinLimitAmount = inObj.MinLimitAmount,
                            DayIncome = inObj.DayIncome,
                            RemainAmount = inObj.DayIncome,
                            Name = inObj.Name,
                            StoreType = inObj.StoreType,
                            Channel = IQBCore.IQBPay.BaseEnum.Channel.League,
                            Rate = us.OwnerRate,
                            OwnnerOpenId = inObj.OwnnerOpenId,
                            RecordStatus = IQBCore.IQBPay.BaseEnum.RecordStatus.Init,
                            Provider = us.Name,
                        };
                        newObj.InitCreate();
                        db.DBStoreInfo.Add(newObj);
                        db.SaveChanges();

                        EQRStoreAuth qrAuth = new EQRStoreAuth();
                        
                        qrAuth.InitByStore(newObj);
                        qrAuth.APPId = appId;
                        db.DBQRStoreAuth.Add(qrAuth);
                        db.SaveChanges();

                        string url = ConfigurationManager.AppSettings["Site_IQBPay"]+ "api/QRAPI/CreateQRStoreAuth";
                        string json = JsonConvert.SerializeObject(qrAuth);
                        string qrStr = HttpHelper.RequestUrlSendMsg(url, HttpHelper.HttpMethod.Post, json, "application/json");

                        NResult<EQRStoreAuth> qrResult = JsonConvert.DeserializeObject<NResult<EQRStoreAuth>>(qrStr);
                        qrAuth.FilePath = qrResult.resultObj.FilePath;
                        qrAuth.TargetUrl = qrResult.resultObj.TargetUrl;
                        db.Entry(qrAuth).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                   
                }
            }
            catch(Exception ex)
            {
                if(inObj.ID ==0 && newObj!=null)
                {
                    Delete(newObj.ID.ToString());
                }
                result.ErrorMsg = ex.Message;
                result.IsSuccess = false;
            }
            return result;
        }

        [HttpPost]
        public OutAPIResult Delete(string StoreId)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = string.Format("delete from StoreInfo where ID='{0}'; delete from QRStoreAuth where StoreId='{0}';", StoreId);
                    db.Database.ExecuteSqlCommand(sql);

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
        public OutAPIResult OnlineStore(string StoreId)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = string.Format(@"update StoreInfo
set RecordStatus = 0, RemainAmount = DayIncome
where id = '{0}';", StoreId);

                    db.Database.ExecuteSqlCommand(sql);

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
        public OutAPIResult OfflineStore(string StoreId)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    string sql = string.Format(@"update StoreInfo
                        set RecordStatus = 1
                        where id = '{0}';", StoreId);

                    db.Database.ExecuteSqlCommand(sql);

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
