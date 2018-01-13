using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.SMS;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.Sys;
using IQBCore.IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBWX.DataBase.IQBPay
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class AliPayContent : DbContext
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

        public DbSet<EQRHuge> DBQRHuge { get; set; }

        public DbSet<EQRHugeTrans> DBQRHugeTrans { get; set; }



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

      

     
    }
}