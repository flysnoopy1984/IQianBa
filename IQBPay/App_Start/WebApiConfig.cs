using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IQBPay
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //json 序列化设置  
            GlobalConfiguration.Configuration.Formatters
                .JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                    DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local,
                    DateFormatString = "yyyy-MM-dd HH:mm:ss",
                };
        }
    }
}
