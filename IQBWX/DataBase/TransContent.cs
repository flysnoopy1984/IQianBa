using IQBWX.Models.Order;
using IQBWX.Models.Results;
using IQBWX.Models.Transcation;
using IQBWX.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace IQBWX.DataBase
{
    public class TransContent: DbContext
    {
        public TransContent() : base("MainDBConnection")
        {
           // Database.SetInitializer<TransContent>(new CreateDatabaseIfNotExists<TransContent>());
        }

        public DbSet<EMemberInfo> MemberInfo { get; set; }

        public DbSet<EARUserTrans> ARTransDbSet { get; set; }

        public DbSet<EAPUserTrans> APTransDbSet { get; set; }

        public void ForInit()
        {          
       //     ARTransDbSet.FirstOrDefault<EARUserTrans>(t => t.TransId ==1);
            APTransDbSet.FirstOrDefault<EAPUserTrans>(t => t.TransId == 1);          
        }

        public List<RAPTrans> APPagination(int pageIndex, string openId)
        {
            var list = this.APTransDbSet.Where(t => t.openId == openId).Select(t=>new RAPTrans
                     {                     
                         Amount = t.Amount,
                         TransDateTime = t.TransDateTime,
                         TransId = t.TransId,                      
                     });


            list = list.OrderByDescending(t => t.TransDateTime);
            int totalCount = list.Count();

            List<RAPTrans> result = null;
            if (pageIndex == 0)
            {
                result = list.Take(10).ToList();
                if (result.Count>0)
                    result[0].TotalCount = totalCount;
            }
            else
                result = list.Skip(pageIndex * 10).Take(10).ToList();
            return result;
        }
        public List<RARTrans> ARPagination(int pageIndex,string openId)
        {
            var list = this.ARTransDbSet.Where(t => t.openId == openId).Join(this.MemberInfo, a => a.FromOpenId, b => b.openId,
                     (a, b) => new RARTrans
                     {
                         ChildNickName = b.nickname,
                         openId = a.openId,
                         Amount = a.Amount,
                         TransDateTime = a.TransDateTime,
                         TransId = a.TransId,
                         TransRemark = a.TransRemark
                     });


            list = list.OrderByDescending(t => t.TransDateTime);
            int totalCount = list.Count();

            List<RARTrans> result = null;
            if (pageIndex ==0)
            { 
                result = list.Take(10).ToList();
                if (result.Count > 0)
                    result[0].TotalCount = totalCount;
            }
            else
                result = list.Skip(pageIndex * 10).Take(10).ToList();
            return result;
        }

        public EAPUserTrans InitAPTrans(EMemberInfo mi , decimal amt)
        {
            EAPUserTrans ap = new EAPUserTrans()
            {
                openId = mi.openId,
                Amount = amt,
                TransDateTime = DateTime.Now,
                TransRemark = "提款" + amt + "余额",
            };
            return ap;

        }

        public EARUserTrans InitARTrans(EOrderLine order, EMemberInfo mi, int level,decimal incomeAmt)
        {
            EARUserTrans ar = new EARUserTrans()
            {
                Amount = incomeAmt,
                ARTransType = Common.ARTransType.pop,
                FromOpenId = order.OpenId,
                ChildLevel = level,
                FromMemberType = mi.MemberType,
                FromOrderId = order.OrderId,
                ItemId = order.ItemId,
                openId = mi.openId,
                TransDateTime = DateTime.Now,
            };
            return ar;
        }      

    }
}