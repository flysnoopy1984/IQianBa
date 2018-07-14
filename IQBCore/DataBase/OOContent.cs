using IQBCore.OO.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.DataBase
{
    public class OOContent: DbContext
    {
        public OOContent() : base("OOConnection")
        {

        }

        public DbSet<EBanner> DBBanner { get; set; }
        public DbSet<EUserInfo> DBUserInfo { get; set; }
        public DbSet<EStoreInfo> DBStoreInfo { get; set; }
        public DbSet<EUserTaskOrder> DBUserTaskOrder { get; set; }

        public DbSet<EItemInfo> DBItemInfo { get; set; }

        public DbSet<ETaskInfo> DBTaskInfo { get; set; }

        public DbSet<EUserTask> DBUserTask { get; set; }

        public DbSet<ECurrencyInfo> DBCurrencyInfo { get; set; }

        public DbSet<EUserBalance> DBUserBalance { get; set; }

        public DbSet<EUserCashOutTrans> DBUserCashOutTrans { get; set; }

        public DbSet<EUserQRInvite> DBUserQRInvite { get; set; }

        public DbSet<EUserRelation> DBUserRelation { get; set; }

        public DbSet<EUserReward> DBUserReward { get; set; }

        public DbSet<ESysConfig> DBSysConfig { get; set; }

    }
}
