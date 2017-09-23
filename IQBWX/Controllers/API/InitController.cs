using IQBWX.DataBase;
using IQBWX.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBWX.Controllers.DBAPI
{
    public class InitController : ApiController
    {
        [HttpPost]
        public bool InitItem()
        {           

            try
            {
                EItemInfo.InitItem();
            }
            catch 
            {
                return false;
            }
            return true;

        }
    }
}
