using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Common
{
    public class IQBConst
    {
        public const string SessionOpenId = "OpenId";
        public const string SessionToken = "Token";
        public const string SessionUserId = "UserId";

        public const string AdminQQ = "2551038207";
        /// <summary>
        /// 选择的套餐
        /// </summary>
        public const string SessionSelTC = "selTC";

        public static string GetMemberTypeValue(MemberType MemberType)
        {
            if (MemberType == MemberType.Channel)
            {
                return "城市经理";
            }
            else
            {
                return "大区经理";
            }
        }
    }
}