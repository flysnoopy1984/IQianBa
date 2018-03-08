using IQBCore.Model;
using IQBCore.IQBPay.BaseEnum;
using IQBPay.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IQBPay.Core
{
    public class O2OShipmentAttribute: FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            UserSession loginUser = filterContext.HttpContext.Session["UserSession"] as UserSession;
            if (loginUser == null || loginUser.Id==0)
            {
                var redirectUrl = "/O2O/Login?&action=" + ExistAction.sessionlost.ToString();
                 filterContext.Result = new RedirectResult(redirectUrl);
                return;
            }
        }
    }
}