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

        //public AliPayContent(bool isInit = false) : base("PPConnection")
        //{
        //    if (isInit)
        //        Database.SetInitializer<AliPayContent>(new DropCreateDatabaseAlways<AliPayContent>());
        //    else
        //        Database.SetInitializer<AliPayContent>(null);
        //}

        public DbSet<EAgentCommission> DBAgentCommission { get; set; }



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

        public DbSet<EQRStoreAuth> DBQRStoreAuth { get; set; }


        public DbSet<EUserStore> DBUserStore { get; set; }


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

        public DbSet<EUserInfo> DBUserInfo { get; set; }

        public Boolean IsExistUser(string openId)
        {
            int i = DBUserInfo.Count(u => u.OpenId == openId);
            return (i > 0);

        }

        public DbSet<EUserAccountBalance> DBUserAccountBalance { get; set; }

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
                EQRUser qrUser,pQrUser = null;
                //RUserInfo pi = null;
                EQRInfo cQR = null;


                //被邀请人的邀请码
                cQR = new EQRInfo();
                cQR.InitByUser(ui);

              //  cQR.ParentCommissionRate = Convert.ToSingle(1.5);
               // cQR.Rate = pQR.Rate - cQR.ParentCommissionRate;
                cQR.ReceiveStoreId = -1;
                this.DBQRInfo.Add(cQR);
                this.SaveChanges();

                ui.QRInviteCode = cQR.ID;

              

                cQR = QRManager.CreateMasterUrlById(cQR, context);
                DbEntityEntry<EQRInfo> entry = this.Entry<EQRInfo>(cQR);
                entry.State = System.Data.Entity.EntityState.Modified;
                entry.Property(t => t.OrigFilePath).IsModified = true;
                entry.Property(t => t.FilePath).IsModified = true;
                entry.Property(t => t.TargetUrl).IsModified = true;
                this.SaveChanges();
            
                //收款 二维码
                string sql = string.Format("select MarketRate-Rate from qrUser where QRType ={1} and openId = '{0}'", pQR.OwnnerOpenId,(int)QRReceiveType.Small);
                float rate = Database.SqlQuery<float>(sql).FirstOrDefault(); //这个是真的费率，不是所得
                qrUser = new EQRUser();
                // = BaseController.GlobalConfig.MarketRate;
                qrUser.IsCurrent = true;
                qrUser.QRId = pQR.ID;
                qrUser.OpenId = ui.OpenId;
                qrUser.UserName = ui.Name;  
                var feeRate = (float)(rate + BaseController.GlobalConfig.ChildFixRate);
                qrUser.Rate = 2;
                qrUser.MarketRate = qrUser.Rate + feeRate;

                if (qrUser.Rate < 0) qrUser.Rate = 1;
                qrUser.ReceiveStoreId = pQR.ReceiveStoreId;
                qrUser.QRType = QRReceiveType.Small;


                if (!string.IsNullOrEmpty(pQR.ParentOpenId))
                {
                    qrUser.ParentOpenId = pQR.ParentOpenId;
                    qrUser.ParentName = pQR.Name;
                    qrUser.ParentCommissionRate = pQR.ParentCommissionRate;
                }
                this.DBQRUser.Add(qrUser);

                this.SaveChanges();
                qrUser = QRManager.CreateUserUrlById(qrUser, ui.Headimgurl);

                this.Entry(qrUser).State = System.Data.Entity.EntityState.Modified;
                this.SaveChanges();

                //信用卡
                sql = string.Format("select MarketRate-Rate from qrUser where QRType ={1} and openId = '{0}'", pQR.OwnnerOpenId, (int)QRReceiveType.CreditCard);
                rate = Database.SqlQuery<float>(sql).FirstOrDefault();

                EQRUser ccQRUser = qrUser;
                ccQRUser.QRType = QRReceiveType.CreditCard;

                feeRate = (float)(rate + BaseController.GlobalConfig.CCChildFixRate);
                ccQRUser.Rate = (float)0.5;
                ccQRUser.MarketRate = ccQRUser.Rate + feeRate;

                //ccQRUser.MarketRate = BaseController.GlobalConfig.CCMarketRate;
                //ccQRUser.Rate = rate + BaseController.GlobalConfig.CCChildFixRate;
                //if (qrUser.Rate < 0) qrUser.Rate = 1;
                this.DBQRUser.Add(ccQRUser);

                //码商
                /* 暂时不支持
                EUserStore pUs = DBUserStore.Where(a => a.OpenId == pQR.OwnnerOpenId).FirstOrDefault();
                if(pUs != null)
                {
                    EUserStore us = new EUserStore
                    {
                        OpenId = ui.OpenId,
                        FixComm = pUs.FixComm,
                        Rate = pUs.Rate - pUs.FixComm,
                        OwnerRate = pUs.Rate,
                    };
                    us.SetName(ui.Name);
                    DBUserStore.Add(us);
                }
                */
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

        public DbSet<EO2OOrder> DBO2OOrder { get; set; }

        public DbSet<EO2ODeliveryAddr> DBO2ODeliveryAddr { get; set; }

        public DbSet<EO2OTranscationWH> DBO2OTranscationWH { get; set; }

        public DbSet<EO2OBuyer> DBO2OBuyer { get; set; }

        public DbSet<EO2OStep> DBO2OStep { get; set; }

        public DbSet<RelRuleStep> DBO2ORelRuleStep { get; set; }

        public DbSet<EO2OAgentFeeRate> DBO2OAgentFeeRate { get; set; }

        public DbSet<EO2ORoleCharge> DBO2ORoleCharge { get; set; }

        public DbSet<EO2OBuyerReceiveAddr> DBO2OBuyerReceiveAddr { get; set; }

        public DbSet<EO2OOrder_SignCode> DBO2OOrder_SignCode { get; set; }

        public DbSet<EO2OTransAgent> DBO2OTransAgent { get; set; }

        #endregion

        #region SMS


        public DbSet<ESMSVerification> DBSMSVerification { get; set; }

        public DbSet<ESMSLog> DBSMSLog { get; set; }
        #endregion

    }
}