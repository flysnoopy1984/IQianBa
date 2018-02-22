﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Common.Constant
{
    public class IQBConstant
    {
        public const string WXQR_IQBPAY_PREFIX = "IQBPay_QR_";

        public const int PageSize = 80;

        /// <summary>
        /// 用户注册时是否直接给予默认QRUser
        /// </summary>
        public const bool NeedDefaultQRModule = false;

        /* Session Key begin */
        /// <summary>
        /// Session Key 
        /// </summary>
        public const string SK_UserPayTime = "UserPayTime";

        /* Session Key end */

        /* Cookie Key begin */
        /// <summary>
        /// 后台仓库登陆使用
        /// </summary>
        public const string ck_O2OUserPhone = "IQB_O2OUserPhone";

        /// <summary>
        /// 用户登陆O2O使用
        /// </summary>
        public const string ck_O2OBuyerPhone = "IQB_O2OBuyerPhone";

        public const string ck_O2OReceiveAccount = "IQB_O2OReceiveAccount";

        
        /* Cookie Key end */
    }
}
