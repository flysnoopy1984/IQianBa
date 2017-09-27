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

        }

        public DbSet<EUserInfo> UserInfoDB { get; set; }

        #region User  
        public Boolean IsExistUser(string openId)
        {
            int i = UserInfoDB.Count(u => u.OpenId == openId);
            return (i > 0);
           
        }
        #endregion


    }
}