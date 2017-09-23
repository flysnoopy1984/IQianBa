using IQBWX.Models.Product;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBWX.DataBase
{
    public class ProductContent: DbContext
    {
        public DbSet<EItemInfo> ItemInfo { get; set; }
        public ProductContent() : base("MainDBConnection")
        {

        }
        public void ForInit()
        {
            EItemInfo.InitItem();
        }



    }
}