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
        public DbSet<EUserOrder> DBOrderInfo { get; set; }

        public DbSet<EItemInfo> DBItemInfo { get; set; }



    }
}
