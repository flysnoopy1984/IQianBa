using IQBWX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Config
{
    public class EPoPCommissionConfigration
    {
        /// <summary>
        /// 当前用户会员等级
        /// </summary>
         public MemberType CurrentLevel { get; set; }

        /// <summary>
        /// 接收佣金对象的会员等级
        /// </summary>
         public MemberType ReceiveMemberLevel { get; set; }

        /// <summary>
        /// 当用用户是佣金接收者第几级（普通3级，高级9级）
        /// </summary>
        public int RelationLevel { get; set; }

         public decimal Commission { get; set; }
    }
}