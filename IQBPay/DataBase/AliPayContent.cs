﻿using IQBPay.Core.BaseEnum;
using IQBPay.Models.QR;
using IQBPay.Models.Store;
using IQBPay.Models.System;
using IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBPay.DataBase
{
    public class AliPayContent: DbContext
    {
        public AliPayContent(bool isInit=false) : base("PPConnection")
        {
            if(isInit)
                Database.SetInitializer<AliPayContent>(new DropCreateDatabaseAlways<AliPayContent>());
            else
                Database.SetInitializer<AliPayContent>(null);

            //    Database.SetInitializer<AliPayContent>(new DropCreateDatabaseIfModelChanges<AliPayContent>());
            // Database.SetInitializer<AliPayContent>(new CreateDatabaseIfNotExists<AliPayContent>());
        }

        public DbSet<EUserInfo> DBUserInfo { get; set; }

        public DbSet<EStoreInfo> DBStoreInfo { get; set; }

        public DbSet<EQRInfo> DBQRInfo { get; set; }

        public DbSet<EQRUser> DBQRUser { get; set; }
        public DbSet<EAliPayApplication> DBAliPayApp { get; set; }

       



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
        public Boolean IsExistStore(string openId,string name)
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
        public Boolean IsExistQR(string openId, string name,QRType qrType)
        {
            int i = DBQRInfo.Count(u => u.OwnnerOpenId == openId && u.Name == name && u.Type ==qrType);

            return (i > 0);

        }

        public EQRInfo QR_GetById(long Id,QRType qrType)
        {
           return  DBQRInfo.Where(a => a.ID == Id && a.Type == qrType).FirstOrDefault();
        }
        #endregion

        #region APP

        #endregion
    }
}