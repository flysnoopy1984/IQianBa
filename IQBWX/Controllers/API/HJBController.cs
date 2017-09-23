using IQBWX.Models.Crawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;


namespace IQBWX.Controllers.API
{
    public class HJBController : ApiController
    {
        public List<EInfoSummery> SummeryList()
        {
            List<EInfoSummery> list = new List<EInfoSummery>();
            EInfoSummery es = new EInfoSummery();
            es.Summery = "Test Summery";
            es.Title = "Test Title";
            list.Add(es);

            es = new EInfoSummery();
            es.Summery = "Test Summery";
            es.Title = "Test Title";
            list.Add(es);

            return list;
        }
    }
}
