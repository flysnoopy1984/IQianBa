using IQBCore.IQBPay.Models.SMS;
using IQBCore.IQBRecharge.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBRecharge.DataBase
{
    public class RecDbContent:DbContext
    {
        public RecDbContent() : base("RechargeConnection")
        {

        }

        public DbSet<EUserInfo> DBUserInfo { get; set; }

        public DbSet<ERcOrderInfo> DBRcOrderInfo { get; set; }

        public DbSet<EItemInfo> DBItemInfo { get; set; }

        public DbSet<EUserAccountBalance> DbUserAccountBalance { get; set; }

        #region SMS
        public DbSet<ESMSVerification> DBSMSVerification { get; set; }

        public DbSet<ESMSLog> DBSMSLog { get; set; }
        #endregion
    }
}