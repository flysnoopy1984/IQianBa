using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Results
{
    public class RExternalWebResult
    {
        /// <summary>
        /// 微信需要显示给用户的信息
        /// </summary>
        public string WXMessage { get; set; }

        /// <summary>
        /// 外部网站回传状态
        /// </summary>
        public int Status { get; set; }
    }
}