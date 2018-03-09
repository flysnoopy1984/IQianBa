using IQBCore.Common.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBPay.Controllers
{
    public class O2OBaseController : BaseController
    {
        public string CheckaoId()
        {
            string aoId = Request.QueryString["aoId"];
            
            if (string.IsNullOrEmpty(aoId))
                aoId = Request["aoId"];
            if(!string.IsNullOrEmpty(aoId))
                Session[IQBConstant.SK_O2OAgentOpenId] = aoId;

            return aoId;

           
        }
    }
}