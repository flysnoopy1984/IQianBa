using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IQBCore.Common.Helper
{
    public class CookieHelper
    {
        
        public static HttpCookie setCookie(string key, string value)
        {
            HttpCookie cookie = null;
            if (HttpContext.Current.Request.Cookies[key] == null)
            {
                cookie = new HttpCookie(key);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            else
                cookie = HttpContext.Current.Response.Cookies[key];

            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(30);
            

            return cookie;
        }

        public static string getCookie(string key)
        {
            if(HttpContext.Current.Request.Cookies[key]!=null)
            {
                return HttpContext.Current.Request.Cookies[key].Value;
            }
            return null;
        }
    }
}
