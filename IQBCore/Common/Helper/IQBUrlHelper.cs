using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class IQBUrlHelper
    {
        public static string Scrpit(this UrlHelper helper, string value)
        {
            string appVersion = ConfigurationManager.AppSettings["appVersion"];
            if (string.IsNullOrEmpty(appVersion))
            {
                return helper.Content(value);
            }
            else
            {
                return helper.Content(string.Format(value + "?_v={0}", appVersion));
            }

        }
    }
}