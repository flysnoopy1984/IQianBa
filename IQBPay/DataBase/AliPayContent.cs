using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.PayChannel;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.SMS;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.Sys;
using IQBCore.IQBPay.Models.Tool;
using IQBCore.IQBPay.Models.User;
using IQBPay.Controllers;
using IQBPay.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace IQBPay.DataBase
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class AliPayContent : DbContext, IBaseContent
    {
        public AliPayContent() : base("PPConnection")
        {

        }

        public AliPayContent(bool isInit = false) : base("PPConnection")
        {
            if (isInit)
                Database.SetInitializer<AliPayContent>(new DropCreateDatabaseAlways<AliPayContent>());
            else
                Database.SetInitializer<AliPayContent>(null);
        }

        public DbSet<EAgentCommission> DBAgentCommission { get; set; }

        public DbSet<ESMSLog> DBSMSLog { get; set; }

        public DbSet<ESMSVerification> DBSMSBuyerOrder { get; set; }
        public DbSet<EUserInfo> DBUserInfo { get; set; }

        public DbSet<EStoreInfo> DBStoreInfo { get; set; }

        public DbSet<EQRInfo> DBQRInfo { get; set; }

        public DbSet<EQRUser> DBQRUser { get; set; }
        public DbSet<EAliPayApplication> DBAliPayApp { get; set; }

        public DbSet<EOrderInfo> DBOrder { get; set; }

        public DbSet<ETransferAmount> DBTransferAmount { get; set; }

        public DbSet<EGlobalConfig> DBGlobalConfig { get; set; }

        public DbSet<EBuyerInfo> DBBuyerInfo { get; set; }

        public DbSet<ETool_QR> DBTool_QR { get; set; }

        public DbSet<EQRHuge> DBQRHuge { get; set; }

        public DbSet<EQRHugeTrans> DBQRHugeTrans { get; set; }

        public DbSet<EOrderDetail> DBOrderDetail { get; set; }

        public DbSet<EPayChannel> DBPayChannel { get; set; }

        #region Interface
        public T Update<T>(T entity) where T : class
        {
            var set = this.Set<T>();
            set.Attach(entity);
            this.Entry<T>(entity).State = EntityState.Modified;
            this.SaveChanges();
            return entity;
        }

        public T Insert<T>(T entity) where T : class
        {
            this.Set<T>().Add(entity);
            this.SaveChanges();
            return entity;
        }

        public void Delete<T>(T entity) where T : class
        {
            this.Entry<T>(entity).State = EntityState.Deleted;
            this.SaveChanges();
        }

        public T Find<T>(params object[] keyValues) where T : class
        {
            return this.Set<T>().Find(keyValues);
        }

        public List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            if (conditions == null)
                return this.Set<T>().ToList();
            else
                return this.Set<T>().Where(conditions).ToList();
        }
        #endregion

        #region User  
        public Boolean IsExistUser(string openId)
        {
            int i = DBUserInfo.Count(u => u.OpenId == openId);
            return (i > 0);

        }
        #endregion

        #region Store

        public int StoreCount(string openId)
        {
            int i = DBStoreInfo.Count(u => u.OwnnerOpenId == openId);
            return i;
        }
        public Boolean IsExistStore(string openId, string name)
        {
            int i = DBStoreInfo.Count(u => u.OwnnerOpenId == openId && u.Name == name);

            return (i > 0);

        }

        /// <summary>
        /// 根据支付宝的UserId获取商铺
        /// </summary>
        /// <param name="AliPayUserId"></param>
        /// <returns></returns>
        public EStoreInfo Store_GetByAliPayUserId(string AliPayUserId)
        {
            return DBStoreInfo.Where(s => s.AliPayAccount == AliPayUserId).FirstOrDefault();
        }
        #endregion

        #region QR
        public Boolean IsExistQR(string openId, string name, QRType qrType)
        {
            int i = DBQRInfo.Count(u => u.OwnnerOpenId == openId && u.Name == name && u.Type == qrType);

            return (i > 0);

        }

        public EQRInfo QR_GetById(long Id, QRType qrType)
        {
            return DBQRInfo.Where(a => a.ID == Id && a.Type == qrType).FirstOrDefault();
        }
        #endregion



        #region QRUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQR"></param>
        /// <param name="ui"></param>
        /// <param name="errorCode">1</param>
        /// <returns></returns>
        public EUserInfo UpdateQRUser(EQRInfo pQR, EUserInfo ui, HttpContext context)
        {
            IQBLog _log = new IQBLog();
            try
            {
                EQRUser qrUser = null;
                //RUserInfo pi = null;
                EQRInfo cQR = null;

                //被邀请人的邀请码
                cQR = new EQRInfo();
                cQR.InitByUser(ui);

                cQR.ParentCommissionRate = Convert.ToSingle(1.5);
                cQR.Rate = pQR.Rate - cQR.ParentCommissionRate;
                cQR.ReceiveStoreId = pQR.ReceiveStoreId;
                this.DBQRInfo.Add(cQR);
                this.SaveChanges();

                ui.QRInviteCode = cQR.ID;

                //if (!string.IsNullOrEmpty(pQR.ParentOpenId))
                //{
                //    pi = db.DBUserInfo.Where(u => u.OpenId == pQR.ParentOpenId).Select(o => new RUserInfo
                //    {
                //        Name = o.Name,
                //        OpenId = o.OpenId,
                //    }).FirstOrDefault();
                //    if (pi == null)
                //    {
                //        throw new Exception("没有找到上级代理");
                //    }               
                //    qrUser.ParentName = pi.Name;
                //}


                cQR = QRManager.CreateMasterUrlById(cQR, context);
                DbEntityEntry<EQRInfo> entry = this.Entry<EQRInfo>(cQR);
                entry.State = System.Data.Entity.EntityState.Modified;
                entry.Property(t => t.OrigFilePath).IsModified = true;
                entry.Property(t => t.FilePath).IsModified = true;
                entry.Property(t => t.TargetUrl).IsModified = true;
                this.SaveChanges();
                //    _log.log("UpdateQRUser 邀请码");


                //收款 二维码
                qrUser = new EQRUser();
                qrUser.MarketRate = BaseController.GlobalConfig.MarketRate;
                qrUser.IsCurrent = true;
                qrUser.QRId = pQR.ID;
                qrUser.OpenId = ui.OpenId;
                qrUser.UserName = ui.Name;
                qrUser.Rate = pQR.Rate;
                qrUser.ReceiveStoreId = pQR.ReceiveStoreId;
                qrUser.QRType = QRType.AR;


                if (!string.IsNullOrEmpty(pQR.ParentOpenId))
                {
                    qrUser.ParentOpenId = pQR.ParentOpenId;
                    qrUser.ParentName = pQR.Name;
                    qrUser.ParentCommissionRate = pQR.ParentCommissionRate;
                }
                this.DBQRUser.Add(qrUser);
                this.SaveChanges();
                //     _log.log("UpdateQRUser 收款 二维码");
                qrUser = QRManager.CreateUserUrlById(qrUser, ui.Headimgurl);
                //    _log.log("UpdateQRUser 收款 二维码图片生成");

                this.Entry(qrUser).State = System.Data.Entity.EntityState.Modified;
                this.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.log("UpdateQRUser Error :" + ex.Message);
                _log.log("UpdateQRUser Error Inner :" + ex.InnerException.Message);
            }

            return ui;

        }


        #endregion


        #region O2O

        public DbSet<EO2OItemInfo> DBO2OItemInfo { get; set; }

        public DbSet<EO2OMall> DBO2OMall { get; set; }

        public DbSet<EO2ORule> DBO2ORule { get; set; }

        public DbSet<EO2OPriceGroup> DBO2OPriceGroup { get; set; }

        #endregion




    }
}