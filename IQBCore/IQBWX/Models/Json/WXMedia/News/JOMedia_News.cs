﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.Json.WXMedia.News
{
    public class JOMedia_News
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ItemItem> item { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int total_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int item_count { get; set; }
    }
}
