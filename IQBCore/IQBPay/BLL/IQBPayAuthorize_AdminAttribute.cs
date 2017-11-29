using IQBCore.IQBPay.BaseEnum;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IQBCore.IQBPay.BLL
{
    public class IQBPayAuthorize_AdminAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            UserSession loginUser = filterContext.HttpContext.Session["UserSession"] as UserSession;
            //When user has not login yet
            if (loginUser == null || string.IsNullOrEmpty(loginUser.OpenId))
            {
                var redirectUrl = "/Main/Login?RedirectPath=" + filterContext.HttpContext.Request.Url + "&action="+ ExistAction.sessionlost.ToString();
                filterContext.Result = new RedirectResult(redirectUrl);
                return;
            }
            if (loginUser.UserRole != BaseEnum.UserRole.Administrator)
            {
                var redirectUrl = "/Main/Login?RedirectPath=" + filterContext.HttpContext.Request.Url + "&action="+ ExistAction.notAdmin.ToString();
                filterContext.Result = new RedirectResult(redirectUrl);
                return;
            }
        }
    }
}
