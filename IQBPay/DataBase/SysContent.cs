using IQBPay.Models.System;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBPay.DataBase
{
    public class SysContent : DbContext
    {
        public SysContent() : base("PPConnection")
        {
            Database.SetInitializer<SysContent>(new DropCreateDatabaseIfModelChanges<SysContent>());
        }

        public DbSet<ESysConfig> DBSysConfig { get; set; }
    }
}