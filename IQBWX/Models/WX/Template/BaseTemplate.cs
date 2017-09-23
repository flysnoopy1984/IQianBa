using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.WX.Template
{
    public class BaseTemplate<T> where T: BaseTemplate<T>, new()
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }
        public string topcolor { get; set; }

        protected T InitObject(string touser,string url,string template_id)
        {
            T obj = new T()
            {
                touser = touser,  
                template_id = template_id,
                topcolor = "#173177",
                url = url,
            };
            return obj;
        }
    }
}