using IQBWX.Models.Crawler;
using IQBWX.Models.Order;
using IQBWX.Models.Product;
using IQBWX.Models.Results;
using IQBWX.Models.Transcation;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBWX.DataBase
{
    public class InitContent : DbContext
    {
        public InitContent() : base("MainDBConnection")
        {
            
        }
        public DbSet<EItemInfo> ItemInfo { get; set; }

        public DbSet<EInfoSummery> InfoSummery { get; set; }

        public DbSet<EInfoDetail> InfoDetail { get; set; }

        public DbSet<EUserInfo> UserInfo { get; set; }
        public DbSet<EMemberInfo> MemberInfo { get; set; }
        public DbSet<EMemberChildren> MemberChildren { get; set; }
        public DbSet<EUserSMSVerify> UserSMSVerify { get; set; }
        public DbSet<EARUserTrans> ARTransDbSet { get; set; }

        public DbSet<EAPUserTrans> APTransDbSet { get; set; }

        public DbSet<EOrderLine> OrderInfo { get; set; }

        public DbSet<ESSOLogin> SSOLoginDB { get; set; }


    }


}
