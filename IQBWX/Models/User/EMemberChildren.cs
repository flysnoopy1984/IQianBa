using IQBWX.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.User
{
    /// <summary>
    /// 层级关系表
    /// </summary>
    [Table("MemberChildren")]
    public class EMemberChildren
    {
        [Key]
        [Column(Order=1)]
        [MaxLength(32)]
        public string pOpenId { get; set; }
        [Key]
        [Column(Order = 2)]
        [MaxLength(32)]
        public string cOpenId { get; set; }
        /// <summary>
        /// 孩子会员等级
        /// </summary>
        public MemberType cMemberType { get; set; }

        /// <summary>
        /// 对于父节点孩子位于第几级
        /// </summary>
        public int ChildLevel { get; set; }


    }
}