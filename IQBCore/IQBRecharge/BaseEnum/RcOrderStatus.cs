using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBRecharge.BaseEnum
{
    public enum RcOrderStatus
    {
        /// <summary>
        /// 面值不符（高于实际金额）
        /// </summary>
        HigherCardValue = -12,

        /// <summary>
        /// 面值不符（低于实际金额）
        /// </summary>
        LessCardValue = -11,

        /// <summary>
        /// 卡号卡密错误
        /// </summary>
        ErrorNoPwd = -10,

        /// <summary>
        /// 已创建代售
        /// </summary>
        Waiting = 0,


        /// <summary>
        /// 已出售
        /// </summary>
        Selled = 12,

        /// <summary>
        /// 被平台管理员处理
        /// </summary>
        AdminHandler = 90,

        /// <summary>
        /// 完成
        /// </summary>
        Complete = 100,
    }
}
