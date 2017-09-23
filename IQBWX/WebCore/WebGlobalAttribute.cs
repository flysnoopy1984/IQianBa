using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using System.Web;
using System.Web.Mvc;

namespace IQBWX.WebCore
{
    public class WebGlobalAttribute : ActionFilterAttribute
    {
        public bool IsCheck { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            Boolean WebMaintain = Convert.ToBoolean(ConfigurationManager.AppSettings["WebMaintain"]);

            if (WebMaintain)
            {
               

                //跳转到登陆页
                filterContext.HttpContext.Response.Redirect("/Home/ErrorMessage");
            }
        }
    }
}