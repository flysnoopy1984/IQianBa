using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.Json.WXMedia.News
{
    public class JOMedia_NewsItem
    {
        public string media_id { get; set; }
        public List<JOMedia_Item_Content> content { get; set; }
    }
}
