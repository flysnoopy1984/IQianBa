using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBPay.Core.BaseEnum
{
    public class IQBPayEnum
    {
    }

    /// <summary>
    /// 表明用户是普通用户还是管理员
    /// </summary>
    public enum UserType
    {
        Normal=0,

        Admin = 99,
    }

}