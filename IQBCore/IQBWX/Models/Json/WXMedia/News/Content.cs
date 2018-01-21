using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.Json.WXMedia.News
{
    public class Content
    {
        /// <summary>
        /// 
        /// </summary>
        public List<News_itemItem> news_item { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int create_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int update_time { get; set; }
    }
}
