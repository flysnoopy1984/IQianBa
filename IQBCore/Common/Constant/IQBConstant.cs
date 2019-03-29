using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Common.Constant
{
    public class IQBConstant
    {
        #region IQB
        public const string WXQR_IQBPAY_PREFIX = "IQBPay_QR_";

        public const int PageSize = 80;

        /// <summary>
        /// 用户注册时是否直接给予默认QRUser
        /// </summary>
        public const bool NeedDefaultQRModule = false;

        public const string BuyerPhone = "BuyerPhone";
        public const string BuyerOpenId = "BuyerOpenId";

        public const string BuyerAliAccount = "BuyerAliAccount";

        public const string SK_AgentQRId = "PPAgentQRId";

        /* Session Key begin */
        /// <summary>
        /// Session Key 
        /// </summary>
        public const string SK_UserPayTime = "UserPayTime";

        /// <summary>
        /// O2O对应的代理QrUserId
        /// </summary>
        public const string SK_O2OQrUserId = "O2OQrUserId";

        public const string SK_O2OAgentOpenId = "O2OAgentOpenId";

        public const string SK_O2OBuyerSession = "BuyerSession";

        /* Session Key end */

        /* Cookie Key begin */
        /// <summary>
        /// 后台仓库登陆使用
        /// </summary>
        public const string ck_O2OUserPhone = "IQB_O2OUserPhone";

        /// <summary>
        /// 买家登陆O2O使用
        /// </summary>
        public const string ck_O2OBuyerPhone = "IQB_O2OBuyerPhone";

        public const string ck_O2OReceiveAccount = "IQB_O2OReceiveAccount";
        public const string ck_O2OUser = "IQB_O2OUser";

        /* Cookie Key end */

        #endregion

        #region OO
        public const string DefaultDateTime= "1900-1-1";

        #endregion

        #region ShuaDan
        public const string WXQR_ShuaDan_PREFIX = "SD";
        #endregion
    }
}
