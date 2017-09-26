﻿using System;
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
    public enum UserRole
    {
        
        /// <summary>
        /// 普通用户，可以使用上级（包括平台）的二维码
        /// </summary>
        NormalUser=1,

        /// <summary>
        /// 可以在平台二维码的基础上生成自己的二维码并发放
        /// </summary>
        StoreVendor = 2,

        /// <summary>
        /// 提供密钥,可使用自己的二维码
        /// </summary>
        StoreMaster =3,

    }

    public enum UserStatus
    {
        /// <summary>
        /// 关注
        /// </summary>
        Follow=1,
        Register =2,


    }

}