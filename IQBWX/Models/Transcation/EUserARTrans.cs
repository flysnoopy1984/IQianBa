using IQBCore.IQBWX.BaseEnum;
using IQBWX.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Transcation
{

    /// <summary>
    /// 用户收入
    /// </summary>
    [Table("ARUserTrans")]
    public class EARUserTrans: BaseTrans
    {
        [MaxLength(20)]
        public string FromOrderId { get; set; }

        [MaxLength(32)]
        public string FromOpenId { get; set; }

        /// <summary>
        /// 孩子会员类型
        /// </summary>
        public MemberType FromMemberType { get; set; }

        /// <summary>
        /// 第几级孩子
        /// </summary>
        public int ChildLevel { get; set; }

        [MaxLength(20)]
        public string ItemId { get; set; }

        public ARTransType ARTransType { get; set; }

    }
}