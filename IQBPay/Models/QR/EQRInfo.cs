using IQBPay.Core.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBPay.Models.QR
{
    public class EQRInfo
    {
        public long Id { get; set; }


        /// <summary>
        /// 拥有者
        /// </summary>
        public string OpenId { get; set; }


        /// <summary>
        /// 所属商户
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// 返点率0-1
        /// </summary>
        public double Rate { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public string FilePath { get; set; }

        public string Url { get; set; }
    }
}