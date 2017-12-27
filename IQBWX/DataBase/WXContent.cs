using IQBCore.Common.Helper;
using IQBWX.Common;
using IQBWX.Models.WX;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBWX.DataBase
{
    public class WXContent: DbContext
    {
        public WXContent() : base("MainDBConnection")
        {

        }

        public ESSOLogin FindSSOForScaned(string ssoToken)
        {
            ESSOLogin entity = this.SSOLoginDB.Where(s => s.ssoToken == ssoToken && s.OpenId != null && s.IsValidate == true).FirstOrDefault();
            return entity;
        }

        public DbSet<ESSOLogin> SSOLoginDB { get; set; }

        public void InserSSOToken(ESSOLogin entity)
        {
            IQBLog log = new IQBLog();
            try
            {
                this.SSOLoginDB.Add(entity);

                this.SaveChanges();
            }
            catch(Exception ex)
            {
                log.log("InserSSOToken Error:" + ex.Message);
                log.log("InserSSOToken StackTrace:" + ex.StackTrace);
            }
       }

        public ESSOLogin GetSSOEntity(string ssoToken)
        {
            ESSOLogin entity = null;
            entity = this.SSOLoginDB.Where(s=>s.ssoToken == ssoToken && s.OpenId == null && s.IsValidate == false).FirstOrDefault();
          
            return entity;
        }

       
    }
}