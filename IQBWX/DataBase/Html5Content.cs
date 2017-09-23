using IQBWX.Models.Crawler;
using IQBWX.Models.Results;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBWX.DataBase
{
    public class Html5Content : DbContext
    {
        public Html5Content() : base("MainDBConnection")
        {

        }

        public DbSet<EInfoSummery> InfoSummery { get; set; }

        public DbSet<EInfoDetail> InfoDetail { get; set; }
     

        public EInfoDetail GetDetail(int infoId)
        {
            EInfoDetail detail = this.InfoDetail.Where(i => i.EInfoSummeryId == infoId).FirstOrDefault();
            return detail;
        }

        public RInfoDetail GetDetailResult(int infoId)
        {
            RInfoDetail result = null;

            IQueryable<RInfoDetail> list = this.InfoDetail.Where(i => i.EInfoSummeryId == infoId)
                .Join(this.InfoSummery, a=>a.EInfoSummeryId,b=>b.InfoId,
                (a, b) => new RInfoDetail {
                    ArticleContent = a.ArticleContent,
                    DetailId = a.DetailId,
                    PublishDate = b.PublishDate,
                    ReadCount = b.ReadCount,
                    Title = b.Title
                });
            result = list.ToList()[0];
            return result;
        }

        public List<EInfoSummery> SummeryPagination(int pageIndex)
        {
            var list = this.InfoSummery.OrderByDescending(i=>i.PublishDate);
            int totalCount = list.Count();

            List<EInfoSummery> result = null;
            if (pageIndex == 0)
            {
                result = list.Take(10).ToList();
                if (result.Count > 0)
                    result[0].TotalCount = totalCount;
            }
            else
                result = list.Skip(pageIndex * 10).Take(10).ToList();
            return result;
        }

        public bool IsExistSummery(string OrigInfoId)
        {
            int i=InfoSummery.Count<EInfoSummery>(s => s.OrigInfoId == OrigInfoId);
            return (i > 0);
        }

        
    }
}
    
    
