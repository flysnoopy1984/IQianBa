using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.WX.Template
{
    public class TemplateData<T>
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }
        public string topcolor { get; set; }

        public T data { get; set; }
    }
}