using IQBCore.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBWX.Controllers.API
{
    public class OutDataController : ApiController
    {
        public void RefreshGlobelConfig()
        {
            WXBaseController.GlobalConfig = null;
            WXBaseController.RefreshSession = true;
            //IQBLog log = new IQBLog();
            //log.log("RefreshGlobelConfig");
        }

        public void RefreshSession()
        {
            WXBaseController.RefreshSession = true;
            //IQBLog log = new IQBLog();
            //log.log("RefreshSession");
        }
    }

}
