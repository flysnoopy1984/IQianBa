using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace IQBWX.BLL
{
    public class WXAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            UserSession loginUser = filterContext.HttpContext.Session["UserSession"] as UserSession;
            if (loginUser == null || string.IsNullOrEmpty(loginUser.OpenId))
            {
                var redirectUrl = "/Home/ErrorMessage?code=2002";
                filterContext.Result = new RedirectResult(redirectUrl);
                return;
            }
        }
    }
}