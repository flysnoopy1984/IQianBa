using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.Json.WXMedia.News
{
    public class News_itemItem
    {
        /// <summary>
        /// 平台申明
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string digest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content_source_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string thumb_media_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int show_cover_pic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string thumb_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int need_open_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int only_fans_can_comment { get; set; }
    }
}
