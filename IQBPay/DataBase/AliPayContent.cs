using IQBPay.Models.Store;
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
        public AliPayContent() : base("PPConnection")
        {
             //Database.SetInitializer<AliPayContent>(new DropCreateDatabaseAlways<AliPayContent>());
        }

        public DbSet<EUserInfo> UserInfoDB { get; set; }

        public DbSet<EStoreInfo> StoreInfoDB { get; set; }

        #region User  
        public Boolean IsExistUser(string openId)
        {
            int i = UserInfoDB.Count(u => u.OpenId == openId);
            return (i > 0);
           
        }
        #endregion

        #region Store
        public Boolean IsExistStore(string openId,string name)
        {
            int i = StoreInfoDB.Count(u => u.OwnnerOpenId == openId && u.Name == name);

            return (i > 0);

        }
        #endregion

    }
}