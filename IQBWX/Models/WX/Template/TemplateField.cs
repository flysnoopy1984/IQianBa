using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.WX.Template
{
    public class TemplateField
    {
        public TemplateField()
        {
            color = "#000000";
        }
        public string value { get; set; }
        public string color { get; set; }
    }
}