using IQBPay.Core.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBPay.Models.Store
{
    public class EStoreInfo
    {
        public int ID { get; set; }

        /// <summary>
        /// 拥有者
        /// </summary>
        public string OpenId { get; set; }

        
        public string Name { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 可能被禁用
        /// </summary>
        public RecordStatus RecordStatus{ get; set; }

    }
}